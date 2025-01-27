using System.Collections.Generic;
using UnityEngine;

public class ChainLightningTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject lightStonePrefab;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;  // Reichweite des Turms
    [SerializeField] private float rotationSpeed = 5f;  // Rotationsgeschwindigkeit
    [SerializeField] private float bps = 1f;            // Schüsse pro Sekunde (Bullets per second)
    [SerializeField] private int maxChainTargets = 3;   // Maximale Anzahl an Zielen pro Kette
    [SerializeField] private float chainRange = 3f;     // Reichweite des Blitzes zwischen Zielen
    [SerializeField] private float damageReductionPerJump = 0.8f; // Schaden pro Sprung reduzieren (20% weniger)

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform; // Wähle das erste Ziel
        }
    }

    private void Shoot()
    {
        if (target != null)
        {
            // Erzeuge das erste Projektil (Stone)
            GameObject chainLightningStoneObj= Instantiate(lightStonePrefab, firingPoint.position, Quaternion.identity);
            ChainLightningStone lightStoneScript = lightStonePrefab.GetComponent<ChainLightningStone>();
            lightStoneScript.Initialize(target, maxChainTargets, chainRange, damageReductionPerJump);
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRange); // Zeigt die Zielreichweite des Turms
    }
}
