using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private SpriteRenderer turretSpriteRenderer;
    [SerializeField] private Sprite[] upgradeSprites;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f;        //bullet per second

    public int UpgradeCost => CalculateCost();
    private float bpsBase;
    private float targetingRangeBase;

    private Transform target;
    private float timeUntilFire;
    private int level = 1;

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);     //So it will close the UI

        if (upgradeSprites.Length < 0)
        {
            turretSpriteRenderer.sprite = upgradeSprites[0];
        }
    }
    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
#endif
    }
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
        upgradeCostText.text = "Upgrade Cost: " + UpgradeCost.ToString();
    }
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);     // to prevent the bug where you can't open up the UI anymore after opening it up once
    }

    public void Upgrade()
    {
        if (UpgradeCost > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(UpgradeCost);

        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();

        if (upgradeSprites.Length > level - 1)
        {
            turretSpriteRenderer.sprite = upgradeSprites[level - 1];
        }

        CloseUpgradeUI();
        Debug.Log("New BPS: " + bps);
        Debug.Log("New BPS: " + targetingRange); 
        Debug.Log("New Cost: " + UpgradeCost);
    }
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));      // Cost get more expensive after upgrade
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.2f);
    }
    
    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.15f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }
    private void Shoot()
    {
        GameObject stoneObj = Instantiate(stonePrefab, firingPoint.position, Quaternion.identity);
        Stone stoneScript = stoneObj.GetComponent<Stone>();
        stoneScript.SetTarget(target);
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
