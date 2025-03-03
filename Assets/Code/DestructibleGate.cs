using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DestructibleGate : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    [Header("Sprites")]
    [SerializeField] private Sprite healthySprite;
    [SerializeField] private Sprite damagedSprite;
    [SerializeField] private Sprite destroyedSprite;

    [Header("UI")]
    [SerializeField] private Image healthBar;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        int level = GameManager.Instance.currentLevel;
        float savedHealth = GameManager.Instance.GetSavedHealth(level);
        currentHealth = savedHealth > 0 ? savedHealth : maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthUI();
        UpdateSprite();
    }

    [System.Obsolete]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();
        UpdateSprite();

        if (currentHealth <= 0f)
        {
            // Trigger GameManager's failure handling
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

    private void UpdateSprite()
    {
        float healthPercent = currentHealth / maxHealth;

        if (healthPercent >= 0.66f)
        {
            spriteRenderer.sprite = healthySprite;
        }
        else if (healthPercent >= 0.33f)
        {
            spriteRenderer.sprite = damagedSprite;
        }
        else
        {
            spriteRenderer.sprite = destroyedSprite;
        }
    }
}