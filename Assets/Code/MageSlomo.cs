using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class MageSlomo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int baseUpgradeCost = 250;
    [SerializeField] private GameObject freezeEffectPrefab;
    [SerializeField] private TextMeshProUGUI currencyUI;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 4f;    // attacks per second
    [SerializeField] private float freezeTime = 1f;

    private float apsBase;
    private float targetingRangeBase;

    public Transform target;
    private float timeUntilFire;
    private int level = 1;

    private void Start()
    {
        apsBase = aps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            if (EnemiesInRange())
            {
                FreezeEnemies();
                timeUntilFire = 0f;
            }
            else
            {
                // Cap the timer at the attack interval to prevent immediate attack after enemy enters range
                timeUntilFire = Mathf.Min(timeUntilFire, 1f / aps);
            }
        }
    }

    private bool EnemiesInRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        foreach (var hit in hits)
        {
            if (hit.transform.GetComponent<EnemyMovement>() != null)
                return true;
        }
        return false;
    }

    public void OpenUpgradeUIMage()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUIMage()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        int bank = GameManager.Instance.playerCurrency;

        if (CalculateCostMage() > bank) return;

        LevelManager.main.SpendCurrency(CalculateCostMage());

        level++;

        aps = CalculateAPS();
        targetingRange = CalculateRange();

        CloseUpgradeUIMage();
        Debug.Log("New APS: " + aps);
        Debug.Log("New AR: " + targetingRange);
        Debug.Log("New Cost: " + CalculateCostMage());
        currencyUI.text = CalculateCostMage().ToString();
    }

    private int CalculateCostMage()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 1f));
    }

    private float CalculateAPS()
    {
        return apsBase * Mathf.Pow(level, 0.1f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.15f);
    }

    private IEnumerator FadeAndDestroy(SpriteRenderer sprite, GameObject freezeEffect)
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        Color startColor = sprite.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Destroy(freezeEffect);
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        bool hitEnemies = false;

        foreach (var hit in hits)
        {
            EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
            SpriteRenderer enemySprite = hit.transform.GetComponent<SpriteRenderer>();

            if (em == null) continue;

            hitEnemies = true;

            em.UpdateSpeed(0.5f);
            enemySprite.color = Color.cyan;

            StartCoroutine(ResetEnemySpeed(em, enemySprite));
        }

        if (hitEnemies)
        {
            // Create smoke effect
            GameObject freezeEffect = Instantiate(freezeEffectPrefab, transform.position, Quaternion.identity);
            float effectScale = targetingRange * 1.5f;
            freezeEffect.transform.localScale = new Vector3(effectScale, effectScale, 1);
            SpriteRenderer freezeEffectSprite = freezeEffect.GetComponent<SpriteRenderer>();
            StartCoroutine(FadeAndDestroy(freezeEffectSprite, freezeEffect));

            // Play sound
            SoundFXManager.Instance.PlayMageAttack(transform.position);
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em, SpriteRenderer enemySprite)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();

        // Fade out the color
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        Color startColor = enemySprite.color;
        Color normalColor = Color.white;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            enemySprite.color = Color.Lerp(startColor, normalColor, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
#endif
    }
}
