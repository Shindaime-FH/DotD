using UnityEngine;
using TMPro;

public class CurrencyPickup : MonoBehaviour
{
    private int currencyWorth;
    [SerializeField] private TextMeshProUGUI currencyUI;

    private void Start()
    {
        // Attempt to find UI if not set in inspector
        if (currencyUI == null)
        {
            currencyUI = GameObject.Find("Currency")?.GetComponent<TextMeshProUGUI>();

            // Log error if still not found
            if (currencyUI == null)
            {
                Debug.LogError("Currency UI TextMeshProUGUI not found!");
            }
        }
    }

    public void SetCurrencyWorth(int worth)
    {
        currencyWorth = worth;
    }

    private void OnMouseDown()
    {
        // Update LevelManager's currency
        LevelManager.main.IncreaseCurrency(currencyWorth);
        Destroy(gameObject); // Remove the coin after collection

        // Optional: Update UI directly if needed (not necessary if Menu's OnGUI is working)
        // currencyUI.text = LevelManager.main.currency.ToString();
    }
}