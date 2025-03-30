using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class Health : MonoBehaviour
{
    public enum EnemyType { Knight, Zombie, Goblin }

    [Header("Attributes")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyworth = 10;

    [Header("References")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement;


    private bool isDestroyed = false;
    private void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in Start!");
        }
        else
        {
            Debug.Log("Animator is assigned: " + animator.name);
        }

        if (enemyMovement == null)
        {
            Debug.LogError("EnemyMovement is not assigned in Start!");
        }
        else
        {
            Debug.Log("EnemyMovement is assigned: " + enemyMovement.name);
        }
    }
    public void TakeDamage(int dmg)
    {
        Debug.Log("TakeDamage called with damage: " + dmg);
        hitPoints -= dmg;
        Debug.Log("Hitpoints after taking damage: " + hitPoints);
        Debug.Log("isDestroyed: " + isDestroyed);
        SoundFXManager.Instance.PlayEnemyDamage(transform.position);
        StartCoroutine(FlashRed());
        if (hitPoints <= 0 && !isDestroyed)
        {
            Debug.Log("Enemy is being destroyed. HitPoints: " + hitPoints + ", isDestroyed: " + isDestroyed);
            Debug.Log("Entering death animation logic");
            EnemySpawner.onEnemyDestroy.Invoke();
            Debug.Log("onEnemyDestroy wird ausgelöst!");
            // LevelManager.main.IncreaseCurrency(currencyworth); old code --> without coin collecting

            if (animator == null) //
            {
                Debug.LogError("Animator is not assigned!");
            }
            if (enemyMovement == null)
            {
                Debug.LogError("EnemyMovement is not assigned!");   //
            }


            if (animator != null && enemyMovement != null)
            {
                Debug.Log("Calling animator.SetTrigger with Die");
                animator.SetTrigger("Die");

                string deathAnimation = enemyMovement.GetDeathAnimation();
                Debug.Log("Calling animator.Play with animation: " + deathAnimation);
                animator.Play(deathAnimation);

                enemyMovement.StopMovement();
            }
            else
            {
                Debug.Log("Animator or EnemyMovement is null!");
            }
      
            switch (enemyType)
            {
                case EnemyType.Knight:
                    SoundFXManager.Instance.PlayKnightDeath(transform.position);
                    break;
                case EnemyType.Zombie:
                    SoundFXManager.Instance.PlayZombieDeath(transform.position);
                    break;
                case EnemyType.Goblin:
                    SoundFXManager.Instance.PlayGoblinDeath(transform.position);
                    break;
            }

            SpawnCoin();        // new code which should spawn the coin after the enemy dies
            isDestroyed = true;
            StartCoroutine(DestroyAfterAnimation());
        }
        // Destroy(gameObject);     // To initialize the death animation, we have to initialize it before it is getting destroyed
        else
        {
            Debug.Log("Enemy not destroyed. HitPoints: " + hitPoints + ", isDestroyed: " + isDestroyed);
        }
        
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color original = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = original;
        }
    }

    private void SpawnCoin()
    {
        // Add this null check
        if (this == null || gameObject == null) return;

        if (coinPrefab != null)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            if (coin.TryGetComponent<CurrencyPickup>(out var coinScript))
            {
                coinScript.SetCurrencyWorth(currencyworth);
            }
        }
    }
    private IEnumerator DestroyAfterAnimation()
    {
        Debug.Log("DestroyAfterAnimation called: ");
        if (animator != null)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
        }
        Destroy(gameObject);
    }
}
