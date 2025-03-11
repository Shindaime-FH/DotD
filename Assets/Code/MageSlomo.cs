using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MageSlomo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int baseUpgradeCost = 200;
    [SerializeField] private GameObject freezeEffectPrefab;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 4f;    // attacks per second
    [SerializeField] private float freezeTime = 1f;

    private float apsBase;
    private float targetingRangeBase;

    public Transform target;
    private float timeUntilFire;
    private int level = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        apsBase = aps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);     //So it will close the UI
    }

    // Update is called once per frame
    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    public void OpenUpgradeUIMage()
    {
        upgradeUI.SetActive(true);
    }
    public void CloseUpgradeUIMage()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);     // to prevent the bug where you can't open up the UI anymore after opening it up once
    }

    public void Upgrade()
    {
        if (CalculateCostMage() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCostMage());

        level++;

        aps = CalculateAPS();
        targetingRange = CalculateRange();

        CloseUpgradeUIMage();
        Debug.Log("New APS: " + aps);
        Debug.Log("New APS: " + targetingRange);
        Debug.Log("New Cost: " + CalculateCostMage());
    }
    private int CalculateCostMage()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 1f));      // Cost get more expensive after upgrade
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
        float fadeDuration = 1f;  // Time to fade out
        float elapsedTime = 0f;
        Color startColor = sprite.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Destroy(freezeEffect); // Remove the effect after fading
    }

    private void FreezeEnemies()
    {
        /* Old Code
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }*/
        // Instantiate and scale the freeze effect
        GameObject freezeEffect = Instantiate(freezeEffectPrefab, transform.position, Quaternion.identity);

        float effectScale = targetingRange * 1.5f; // Adjust size to match AoE
        freezeEffect.transform.localScale = new Vector3(effectScale, effectScale, 1);

        // Start fade-out animation
        SpriteRenderer freezeEffectSprite = freezeEffect.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeAndDestroy(freezeEffectSprite, freezeEffect));

        // Apply slow effect to enemies in range
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        foreach (var hit in hits)
        {
            EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
            SpriteRenderer enemySprite = hit.transform.GetComponent<SpriteRenderer>();

            if (em == null) continue;

            if (em != null && enemySprite != null)
            {
                em.UpdateSpeed(0.5f); // Slow down enemy speed by 50%
                enemySprite.color = Color.cyan;

                StartCoroutine(ResetEnemySpeed(em, enemySprite));
            }
        }
    }

    private IEnumerator ResetEnemySpeed (EnemyMovement em, SpriteRenderer enemySprite)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();

        // Fade out the baby blue color
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        Color startColor = enemySprite.color;
        Color normalColor = Color.white;  // Change to default color

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

