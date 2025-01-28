using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyworth = 10;

    /*[Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private SpriteRenderer spriteRenderer;*/

    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {

        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            Debug.Log("onEnemyDestroy wird ausgelöst!");
            LevelManager.main.IncreaseCurrency(currencyworth);
            isDestroyed = true;
            Destroy(gameObject);     // To initialize the death animation, we have to initialize it before it is getting destroyed


        }
    }
}
