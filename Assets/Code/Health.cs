using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyworth = 10;

    private bool isDestroyed = false;

    public void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            Debug.Log("onEnemyDestroy wird ausgelöst!");
            LevelManager.main.IncreaseCurrency(currencyworth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

}
