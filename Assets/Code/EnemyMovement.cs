using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;     // Reference to Animator
    [SerializeField] private SpriteRenderer spriteRenderer;     // Reference to Sprite Renderer
    [SerializeField] private HealthManager healthManager;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private float baseSpeed;

    public Vector2 lastDirection;
    private bool isMovementDisabled = false;        // Flag to disable movement

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        healthManager = Object.FindFirstObjectByType<HealthManager>();
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                if (healthManager != null)
                {
                    healthManager.TakeDamage(10f); // 10 damage for each surviving enemy 
                }
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        UpdateAnimationAndSprite(direction);        // Update animation and sprite flipping based on direction
    }

    /*private void UpdateAnimationAndSprite(Vector2 direction)
    {

        //  Horizontal movement: Flip the sprite based on movement to the right/left
        if (direction.x > 0)    //moving right
        {
            spriteRenderer.flipX = true;        // flip sprite horizontally
            animator.Play("Zombie_WalkDiagonalLeft");
            animator.Play("Knight_Sword_WalkDiagonalLeft");       // Use the diagonal left animation flipped for diagnoal right
            animator.Play("GoblinRider_WalkDiagonalLeft");
        }
        else if (direction.x < 0)       // moving left
        {
            spriteRenderer.flipX = false;        // Ensuring the sprite doesn't flip
            animator.Play("Zombie_WalkDiagonalLeft");
            animator.Play("Knight_Sword_WalkDiagonalLeft");
            animator.Play("GoblinRider_WalkDiagonalLeft");
        }
        else if (direction.y > 0)        // moving up
        {
            spriteRenderer.flipX = false;       // Ensure its not flipped
            animator.Play("Zombie_WalkUp");
            animator.Play("Knight_Sword_WalkUp");
            animator.Play("GoblinRider_WalkUp");
        }
        else if (direction.y < 0)       // moving down
        {
            spriteRenderer.flipX = false; // Ensure its not flipped
            animator.Play("Zombie_WalkDown");
            animator.Play("Knight_Sword_WalkDown");
            animator.Play("GoblinRider_WalkDown");
        }
    }*/
    private void UpdateAnimationAndSprite(Vector2 direction)
    {
        // Prioritize vertical movement if it’s stronger
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)        // Moving up
            {
                spriteRenderer.flipX = false;  // Ensure it's not flipped
                animator.Play("Zombie_WalkUp");
                animator.Play("Knight_Sword_WalkUp");
                animator.Play("GoblinRider_WalkUp");
            }
            else if (direction.y < 0)   // Moving down
            {
                spriteRenderer.flipX = false; // Ensure it's not flipped
                animator.Play("Zombie_WalkDown");
                animator.Play("Knight_Sword_WalkDown");
                animator.Play("GoblinRider_WalkDown");
            }
        }
        else
        {
            // Horizontal movement: Flip sprite accordingly
            if (direction.x > 0)    // Moving right
            {
                spriteRenderer.flipX = true;  // Flip sprite horizontally
                animator.Play("Zombie_WalkDiagonalLeft");
                animator.Play("Knight_Sword_WalkDiagonalLeft"); // Use diagonal left but flipped
                animator.Play("GoblinRider_WalkDiagonalLeft");
            }
            else if (direction.x < 0)   // Moving left
            {
                spriteRenderer.flipX = false;  // Ensure it's not flipped
                animator.Play("Zombie_WalkDiagonalLeft");
                animator.Play("Knight_Sword_WalkDiagonalLeft");
                animator.Play("GoblinRider_WalkDiagonalLeft");
            }
        }
    }
    public void DisableMovement()
    {
        isMovementDisabled = true;      // Set the flag to disable movement
        rb.linearVelocity = Vector2.zero;       // stop the Rigidbody's velocity
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
