using System.Collections.Generic;
using UnityEngine;

public class ChainLightningStone : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float stoneSpeed = 5f;
    [SerializeField] private int baseDamage = 1;

    private Transform currentTarget;
    private int remainingChains;
    private float chainRange;
    private float damageReductionPerJump;
    private List<Transform> hitTargets = new List<Transform>();

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform target, int maxChains, float range, float reduction)
    {
        currentTarget = target;
        remainingChains = maxChains;
        chainRange = range;
        damageReductionPerJump = reduction;

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector2 direction = (currentTarget.position - transform.position).normalized;
            rb.linearVelocity = direction * stoneSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform == currentTarget)
        {
            // Schaden zuf�gen
            Health healthScript = currentTarget.GetComponent<Health>();
            if (healthScript != null)
            {
                healthScript.takeDamage(CalculateDamage());
            }

            // F�ge Ziel der Liste hinzu, um Mehrfachtreffer zu vermeiden
            hitTargets.Add(currentTarget);

            // Kette fortsetzen, wenn m�glich
            if (remainingChains > 0)
            {
                FindNextTarget();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void FindNextTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(currentTarget.position, chainRange, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            if (!hitTargets.Contains(hit.transform)) // Verhindere Wiederholungstreffer
            {
                currentTarget = hit.transform;
                remainingChains--;
                MoveTowardsTarget();
                return;
            }
        }

        // Kein neues Ziel gefunden, Projektil zerst�ren
        Destroy(gameObject);
    }

    private int CalculateDamage()
    {
        return Mathf.RoundToInt(baseDamage * Mathf.Pow(damageReductionPerJump, hitTargets.Count));
    }

    private void OnDrawGizmosSelected()
    {
        if (currentTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(currentTarget.position, chainRange); // Visualisiere die Kettenreichweite
        }
    }
}
