using UnityEngine;

public class EnemyMovementSecondLevelAlt : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;     // Reference to Animator
    [SerializeField] private SpriteRenderer spriteRenderer;     // Reference to Sprite Renderer

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    // This determines which path array we are currently following
    private Transform[] currentPath;

    private Transform target;
    private int pathIndex = 0;
    private float baseSpeed;

    [Header("Level Toggle")]
    [SerializeField] public bool isFirstLevel = false;
    [SerializeField] public bool isSecondLevel = false;
    [SerializeField] public bool isThirdLevel = false;

    private void Start()
    {
        baseSpeed = moveSpeed;

        // 1) THIRD LEVEL LOGIC: pick 1 of 4 possible arrays at random
        if (isThirdLevel)
        {
            // Random.Range(0,4) ? 0, 1, 2, or 3
            int randomChoice = Random.Range(0, 4);
            switch (randomChoice)
            {
                case 0:
                    currentPath = LevelManager.main.Pathsthirdlvlmain;
                    break;
                case 1:
                    currentPath = LevelManager.main.PathsthirdlvlmainVar;
                    break;
                case 2:
                    currentPath = LevelManager.main.Paththirdlvlalt;
                    break;
                case 3:
                    currentPath = LevelManager.main.PaththirdlvlaltVar;
                    break;
            }
        }
        // 2) SECOND LEVEL LOGIC: pick 1 of 2 possible arrays at random
        else if (isSecondLevel)
        {
            // Random.Range(0, 2) ? 0 or 1
            int randomChoice = Random.Range(0, 2);
            if (randomChoice == 0)
            {
                currentPath = LevelManager.main.Pathscndlvlmain;
            }
            else
            {
                currentPath = LevelManager.main.Pathscndlvlalt;
            }
        }
        // 3) FIRST LEVEL LOGIC: just use the original path
        else if (isFirstLevel)
        {
            currentPath = LevelManager.main.path;
        }
        else
        {
            // If none of the toggles are true, you could default to first level or do nothing.
            currentPath = LevelManager.main.path;
            // Or leave it null if you want to ensure you set at least one toggle.
        }

        // Ensure currentPath is valid, then set the first target
        if (currentPath != null && currentPath.Length > 0)
        {
            target = currentPath[pathIndex];
        }
    }

    private void Update()
    {
        // If there's no assigned path, bail out
        if (currentPath == null || currentPath.Length == 0) return;

        // Check if we've reached the current target
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            // If we've reached the end of the path, destroy the enemy
            if (pathIndex >= currentPath.Length)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                // Move on to the next waypoint
                target = currentPath[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        // If there's no target or path, skip
        if (currentPath == null || currentPath.Length == 0) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        UpdateAnimationAndSprite(direction);
    }

    private void UpdateAnimationAndSprite(Vector2 direction)
    {
        // Horizontal movement: Flip sprite if moving right
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
            animator.Play("Zombie_WalkDiagonalLeft");
            animator.Play("Knight_Sword_WalkDiagonalLeft");
            animator.Play("GoblinRider_WalkDiagonalLeft");
        }
        else if (direction.x < 0) // moving left
        {
            spriteRenderer.flipX = false;
            animator.Play("Zombie_WalkDiagonalLeft");
            animator.Play("Knight_Sword_WalkDiagonalLeft");
            animator.Play("GoblinRider_WalkDiagonalLeft");
        }
        else if (direction.y > 0) // moving up
        {
            spriteRenderer.flipX = false;
            animator.Play("Zombie_WalkUp");
            animator.Play("Knight_Sword_WalkUp");
            animator.Play("GoblinRider_WalkUp");
        }
        else if (direction.y < 0) // moving down
        {
            spriteRenderer.flipX = false;
            animator.Play("Zombie_WalkDown");
            animator.Play("Knight_Sword_WalkDown");
            animator.Play("GoblinRider_WalkDown");
        }
    }

    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }
}