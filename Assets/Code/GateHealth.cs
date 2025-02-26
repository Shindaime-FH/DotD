using UnityEngine;
using UnityEngine.UI;

public class GateHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float maxHealth = 100f;
    // repairRate is now defined per level in the scene via the inspector
    public float repairRate = 0.5f;
    
    [Header("Sprites")]
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private Sprite midRepairedSprite;
    [SerializeField] private Sprite fullyRepairedSprite;
    
    [Header("UI")]
    [SerializeField] private Image healthBar;
    
    private SpriteRenderer spriteRenderer;
    private float currentHealth;
    private bool isFullyRepaired = false;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Start at 0 so that if no enemy damage occurs, repair will eventually complete the level.
        currentHealth = 0f;
        UpdateHealthUI();
        UpdateGateSprite();
    }
    
    private void Update()
    {
        if (!isFullyRepaired)
        {
            // Gradually repair the gate.
            currentHealth += repairRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            UpdateHealthUI();
            UpdateGateSprite();
            if (currentHealth >= maxHealth)
            {
                FullyRepairGate();
            }
        }
        
        // If the gate is broken, trigger a failure.
        if (currentHealth <= 0f)
        {
            GameManager.Instance.FailLevel();
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
        UpdateGateSprite();
        if (currentHealth <= 0f)
        {
            GameManager.Instance.FailLevel();
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
        if (spriteRenderer == null) return;
        
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
        GameManager.Instance.CollectAllCoins();
        GameManager.Instance.CompleteLevel();
    }
}
