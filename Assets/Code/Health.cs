using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyworth = 10;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {

        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyworth);
            isDestroyed = true;
            Destroy(gameObject);     // --> old code. To initialize the death animation, we have to initialize it before it is getting destroyed
            enemyMovement.DisableMovement();               // Stopping movement
            //PlayDeathAnimation();                          // Trigger the appropriate death animation
            //StartCoroutine(DestroyAfterAnimation());       // Delay Destruction to allow animation to play
        }
    }
}
    /*private void PlayDeathAnimation()
    {
        Vector2 lastDirection = enemyMovement.lastDirection;

        if (lastDirection.y > 0)        // moving up
        {
            spriteRenderer.flipX = false;       // Ensure it's not flipped
            animator.SetTrigger("Zombie_DeathUp");
            animator.SetTrigger("Knight(Sword)_DeathUp");
            animator.SetTrigger("GoblinRider_DeathUp");
        }
        else if (lastDirection.y < 0)       // moving down
        {
            spriteRenderer.flipY = false;       // Ensure it's not flipped
            animator.SetTrigger("Zombie_DeathDown");
            animator.SetTrigger("Knight(Sword)_DeathDown");
            animator.SetTrigger("GoblinRider_DeathDown");
        }
        else        // Horizontal Movement (left or right)
        {
            if (lastDirection.x > 0)        // moving right
            {
                spriteRenderer.flipX = true;                                // Flip sprite for moving right
                animator.SetTrigger("Zombie_DeathDiagonalLeft");                  // Use of flipped animation
                animator.SetTrigger("Knight(Sword)_DeathDiagonalLeft");
                animator.SetTrigger("GoblinRider_DeathDeathDiagonalLeft");
            }
            else if (lastDirection.x < 0)       // moving left
            {
                spriteRenderer.flipY = false;                               // No Flup for moving left
                animator.SetTrigger("Zombie_DeathDiagonalLeft");                  // Use of unflipped animation
                animator.SetTrigger("Knight(Sword)_DeathDiagonalLeft");
                animator.SetTrigger("GoblinRider_DeathDeathDiagonalLeft");
            }
        }
    }*/


    /*private System.Collections.IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(GetAnimationClipLength(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name));
        Destroy(gameObject);
    }
    private float GetAnimationClipLength(string clipName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return 0;
        
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0;
    }
}*/