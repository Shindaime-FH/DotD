using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int baseUpgradeCost = 150;
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private GameObject animationHolder;

    private Animator animator;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f;        //bullet per second

    //public static CurrencyPickup CurrencyPickup = new CurrencyPickup();

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

        animator = animationHolder.GetComponent<Animator>();
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
    }
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);     // to prevent the bug where you can't open up the UI anymore after opening it up once
    }

    public void Upgrade()
    {
        Debug.Log("Start Upgrading...");

        int bank = GameManager.Instance.playerCurrency; //optimizing needed
        Debug.Log("You got:" + bank + " gold.");

        if (CalculateCost() > bank ) return; //whats our current balance

        Debug.Log("Time for upgrading...");

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();

        //Showing the upgrade animation
        StartCoroutine(PlayUpgradeEffect());

        CloseUpgradeUI();
        Debug.Log("New BPS: " + bps);
        Debug.Log("New TR: "+ targetingRange); 
        Debug.Log("New Cost: "+ CalculateCost());
        currencyUI.text = CalculateCost().ToString();
    }

    private IEnumerator PlayUpgradeEffect()
    {
        // Get SpriteRenderer component
        SpriteRenderer sr = animationHolder.GetComponent<SpriteRenderer>();
        sr.enabled = true;  // Make visible

        // Play animation
        animator.Play("UpgradePoof", -1, 0f);

        // Wait for animation length
        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        sr.enabled = false; // Hide again
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
