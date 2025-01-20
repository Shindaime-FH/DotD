using UnityEngine;

public class Stone : MonoBehaviour
{


    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float stoneSpeed = 5f;
    [SerializeField] private int stoneDamage =  1;

    private Transform target;
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * stoneSpeed;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Health>().takeDamage(stoneDamage);
        Destroy(gameObject);
    }
}
