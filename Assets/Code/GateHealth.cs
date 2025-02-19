using UnityEngine;
using UnityEngine.UI;

public class GateHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float repairRate = 5f; // Health regenerated per second

    [Header("Sprites")]
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private Sprite midRepairedSprite;
    [SerializeField] private Sprite fullyRepairedSprite;

    [Header("UI")]
    [SerializeField] private Image healthBar; // Assign a smaller UI Image for the gate

    private SpriteRenderer spriteRenderer;
    private float currentHealth;
    private bool isFullyRepaired;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = 0f;
        UpdateHealthUI();
        UpdateGateSprite();
    }

    private void Update()
    {
        if (!isFullyRepaired)
        {
            // Gradually repair the gate
            currentHealth += repairRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            UpdateHealthUI();
            UpdateGateSprite();

            // Check if fully repaired
            if (currentHealth >= maxHealth)
            {
                FullyRepairGate();
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    private void UpdateGateSprite()
    {
        float healthPercent = currentHealth / maxHealth;

        if (healthPercent >= 0.90f)
        {
            spriteRenderer.sprite = fullyRepairedSprite;
        }
        else if (healthPercent >= 0.33f)
        {
            spriteRenderer.sprite = midRepairedSprite;
        }
        else
        {
            spriteRenderer.sprite = brokenSprite;
        }
    }

    private void FullyRepairGate()
    {
        isFullyRepaired = true;

        // Stop enemy spawning
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopAllSpawning(); // You'll add this method next
        }
    }
}