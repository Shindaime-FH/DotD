using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private Animator anim;

    private bool isMenuOpen = true;

    private void OnGUI()
    {
        if (currencyUI != null && GameManager.Instance != null)
        {
            currencyUI.text = GameManager.Instance.playerCurrency.ToString();
        }
    }

    public void ToogleMenu()
    {
        isMenuOpen = !isMenuOpen;
        if (anim != null)
        {
            anim.SetBool("MenuOpen", isMenuOpen);
        }
    }
}
