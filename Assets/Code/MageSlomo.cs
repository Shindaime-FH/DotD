using System.Collections;
using UnityEditor;
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
        return apsBase * Mathf.Pow(level, 0.4f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.2f);
    }
    private void FreezeEnemies()
    {
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
        }
    }

    private IEnumerator ResetEnemySpeed (EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}

