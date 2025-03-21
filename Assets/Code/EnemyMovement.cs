using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    private Transform[] currentPath;
    private Transform target;
    private int pathIndex = 0;
    private float baseSpeed;

    // Level flags (set via SetLevelFlags)
    public bool isFirstLevel = false;
    public bool isSecondLevel = false;
    public bool isThirdLevel = false;

    [Header("Animation")]
    [SerializeField] private string animationPrefix;

    private Vector2 lastDirection; // Store the last movement direction

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            SetLevelFlags(GameManager.Instance.currentLevel);
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            SetLevelFlags(GameManager.Instance.currentLevel);
        }
        baseSpeed = moveSpeed;
    }

    // Update level flags and assign the proper path from LevelManager.
    public void SetLevelFlags(int level)
    {
        isFirstLevel = (level == 1);
        isSecondLevel = (level == 2);
        isThirdLevel = (level == 3);

        // Assign the correct path based on level.
        if (isThirdLevel)
        {
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
        else if (isSecondLevel)
        {
            int randomChoice = Random.Range(0, 2);
            currentPath = (randomChoice == 0) ? LevelManager.main.Pathscndlvlmain : LevelManager.main.Pathscndlvlalt;
        }
        else // Level 1
        {
            currentPath = LevelManager.main.path;
        }

        if (currentPath == null || currentPath.Length == 0)
        {
            Debug.LogWarning("EnemyMovement: No path found! Ensure LevelManager.main.path is assigned in Level 1.");
            return;
        }

        target = currentPath[0];
        pathIndex = 0;
    }

    [System.Obsolete]
    private void Update()
    {
        if (currentPath == null || currentPath.Length == 0)
            return;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex >= currentPath.Length)
            {
                // On level 1, damage the castle via HealthManager; otherwise damage the gate.
                if (isFirstLevel)
                {
                    HealthManager hm = Object.FindObjectOfType<HealthManager>();
                    if (hm != null)
                    {
                        hm.TakeDamage(10f);
                    }
                }
                else
                {
                    DestructibleGate gate = Object.FindObjectOfType<DestructibleGate>();
                    if (gate != null)
                    {
                        gate.TakeDamage(10f);
                    }
                }
                Destroy(gameObject);
                return;
            }
            else
            {
                target = currentPath[pathIndex];
            }
        }
    }
    private bool canMove = true;

    private void FixedUpdate()
    {
        if (!canMove) return; // Stop movement

        if (currentPath == null || currentPath.Length == 0)
            return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        UpdateAnimationAndSprite(direction);
    }

    public void StopMovement()
    {
       canMove = false;
       rb.linearVelocity = Vector2.zero;
    }

    private void UpdateAnimationAndSprite(Vector2 direction)
    {
        lastDirection = direction;  // Storing the last movement direction
        Debug.Log("Last direction: " + lastDirection);  

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                spriteRenderer.flipX = false;
                animator.Play(animationPrefix + "WalkUp");
            }
            else if (direction.y < 0)
            {
                spriteRenderer.flipX = false;
                animator.Play(animationPrefix + "WalkDown");
            }
        }
        else
        {
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
                animator.Play(animationPrefix + "WalkDiagonalLeft");
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = false;
                animator.Play(animationPrefix + "WalkDiagonalLeft");
            }
        }
    }
    // Method to get the death animation based on the last direction
    public string GetDeathAnimation()
    {
        Debug.Log("GetDeathAnimation called");
        if (Mathf.Abs(lastDirection.y) > Mathf.Abs(lastDirection.x))
        {
            // Vertical movement
            if (lastDirection.y > 0)
            {
                Debug.Log("Playing DeathUp");
                return animationPrefix + "DeathUp"; // Death animation for upward direction
            }
            else
            {
                Debug.Log("Playing DeathDown");
                return animationPrefix + "DeathDown"; // Death animation for downward direction
            }
        }
        else
        {
            // Horizontal movement
            if (lastDirection.x > 0)
            {
                Debug.Log("Playing DeathLeft (flipped for right)");
                // Flip the sprite for right direction
                spriteRenderer.flipX = true;
                return animationPrefix + "DeathDiagonalLeft"; // Use the same death animation for both left and right
            }
            else
            {
                Debug.Log("Playing DeathLeft");
                // No flip for left direction
                spriteRenderer.flipX = false;
                return animationPrefix + "DeathDiagonalLeft"; // Use the same death animation for both left and right
            }
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
