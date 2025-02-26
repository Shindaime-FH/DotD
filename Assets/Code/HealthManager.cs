using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverUI; // UI für Game Over
    private bool isGameOver = false;

    public Image healthBar;
    [SerializeField] public float healthAmount = 100f;

    void Start()
    {
        // Add null check for GameManager
        if (GameManager.Instance != null)
        {
            float savedHealth = GameManager.Instance.GetSavedHealth(1);
            healthAmount = savedHealth > 0 ? savedHealth : 100f;
        }
        else
        {
            healthAmount = 100f; // Default value
        }

        // Add null check for healthBar
        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / 100f;
        }
        else
        {
            Debug.LogError("HealthBar reference missing in HealthManager!");
        }
    }

    void Update()
    {
       /* if(healthAmount <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }*/

        /*
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(5);
        }
        */
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100f); // to make sure its not going under 0
        healthBar.fillAmount = healthAmount / 100f;
        Debug.Log("We took damage");

        // check if life is under 0
        if (healthAmount <= 0 && !isGameOver)
        {
            TriggerGameOver();
        }

    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }
    private void TriggerGameOver()
    {
        isGameOver = true;

        // Show the Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Activate Game Over UI

            // Ensure the "Game Over" Text renders above other children
            Transform panelTransform = gameOverUI.transform.Find("Panel");
            if (panelTransform != null)
            {
                Transform gameOverText = panelTransform.Find("Text (TMP)"); // Find the Text object
                if (gameOverText != null)
                {
                    gameOverText.SetAsLastSibling(); // Ensure it renders above the image
                }
            }
        }

        // Pause game
        Time.timeScale = 0f;

        Debug.Log("Game Over!");
    }
    /*private void TriggerGameOver()
    {
        isGameOver = true;

        // show UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // activate UI
        }

        // pause game
        Time.timeScale = 0f;

        Debug.Log("Game Over!");
    }*/
}
