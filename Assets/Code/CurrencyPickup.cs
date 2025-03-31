using UnityEngine;
using TMPro;
using System.Collections;

public class CurrencyPickup : MonoBehaviour
{
    private int currencyWorth;
    private float timeSinceSpawn;
    [SerializeField] private TextMeshProUGUI currencyUI;
    private bool isCollected = false;
    [SerializeField] private float autoCollectTime = 10f; // Configurable in Inspector
    private Animator animator;
    public int GetCurrencyWorth() => currencyWorth;

    public static CurrencyPickup Instance { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (currencyUI == null)
        {
            // Replace null propagation with explicit null check
            GameObject currencyObject = GameObject.Find("Currency");
            if (currencyObject != null)
            {
                currencyUI = currencyObject.GetComponent<TextMeshProUGUI>();
            }

            if (currencyUI == null)
            {
                Debug.LogError("Currency UI TextMeshProUGUI not found!");
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        // Auto-collect when time reaches threshold
        if (timeSinceSpawn >= autoCollectTime && !isCollected)
        {
            CollectCoin();
        }
    }

    public void SetCurrencyWorth(int worth)
    {
        currencyWorth = worth;
    }

    private void OnMouseDown()
    {
        SoundFXManager.Instance.PlayCoinPickup();

        CollectCoin();
   
    }

    public void CollectCoin()
    {
        // Prevent double collection
        if (isCollected) return;

        // Trigger the destruction animation
        if (animator != null)
        {
            animator.SetTrigger("Destroy");
        }

        // Wait for the destruction animation to finish before destroying the coin
        StartCoroutine(DestroyAfterAnimation());
    }

    // Coroutine to wait for the destruction animation to finish
    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the length of the destruction animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Update the player's currency
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerCurrency += currencyWorth;
        }

        // Update the currency UI
        if (currencyUI != null)
        {
            currencyUI.text = GameManager.Instance.playerCurrency.ToString();
        }

        // Mark the coin as collected
        isCollected = true;

        // Destroy the coin GameObject
        Destroy(gameObject);
    }
    /* public void CollectCoin()
    {
        if (isCollected) return;

        GameManager.Instance.playerCurrency += currencyWorth;
        isCollected = true;
        Destroy(gameObject);

    }*/

    // Optional: Add this if you need to check from elsewhere
    public bool ShouldAutoCollect()
    {
        return timeSinceSpawn >= autoCollectTime;
    }
}