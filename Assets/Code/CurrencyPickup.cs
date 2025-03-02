using UnityEngine;
using TMPro;

public class CurrencyPickup : MonoBehaviour
{
    private int currencyWorth;
    private float timeSinceSpawn;
    [SerializeField] private TextMeshProUGUI currencyUI;
    private bool isCollected = false;
    [SerializeField] private float autoCollectTime = 10f; // Configurable in Inspector

    public int GetCurrencyWorth() => currencyWorth;

    private void Start()
    {
        // Existing UI finding code
        if (currencyUI == null)
        {
            currencyUI = GameObject.Find("Currency")?.GetComponent<TextMeshProUGUI>();
            if (currencyUI == null)
            {
                Debug.LogError("Currency UI TextMeshProUGUI not found!");
            }
        }
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
        CollectCoin();
    }

    public void CollectCoin()
    {
        if (isCollected) return;

        GameManager.Instance.playerCurrency += currencyWorth;
        isCollected = true;
        Destroy(gameObject);
    }

    // Optional: Add this if you need to check from elsewhere
    public bool ShouldAutoCollect()
    {
        return timeSinceSpawn >= autoCollectTime;
    }
}