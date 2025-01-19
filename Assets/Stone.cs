using UnityEngine;

public class Stone : MonoBehaviour
{
    private Transform target;

    public void SetTarget (Transform _target)
    {
        this.target = _target;
    }

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float stoneSpeed = 5f;


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * stoneSpeed;

        Debug.Log("Stone velocity: " + rb.linearVelocity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   // Take health from enemy
        Destroy(gameObject);
    }
}
