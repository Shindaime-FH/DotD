using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CurrencyPickup : MonoBehaviour
{
    private int currencyWorth;
    [SerializeField] private TextMeshProUGUI currencyUI;

    private void Start()
    {
        if (currencyUI == null)
        {
            currencyUI = GameObject.Find("Currency").GetComponent<TextMeshProUGUI>();     // Auto-find the UI
            if (currencyUI != null)
            {
                Debug.LogError("Currency UI TextMeshProUGUI not found!");
            }
        }
    }
    public void SetCurrencyWorth(int worth)
    {
        currencyWorth = worth;      // Set coin value based on enemy
        Debug.Log("Currency Worth Set To: " + currencyWorth);
    }

    private void OnMouseDown()
    {
        if (currencyUI != null)
        {
            int currentCurrency = int.Parse(currencyUI.text);
            currentCurrency += currencyWorth;
            currencyUI.text = currentCurrency.ToString();
            Destroy(gameObject); // Remove coin after clicking
        }
    }
}