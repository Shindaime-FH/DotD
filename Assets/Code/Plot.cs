using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    // We now only keep one tower reference.
    private GameObject towerObj;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (UIManager.main.IsHoveringUI()) return; // Don’t allow clicking through UI.

        // If a tower is already here, open its upgrade UI.
        if (towerObj != null)
        {
            // Check which type of tower is here.
            Turret turret = towerObj.GetComponent<Turret>();
            if (turret != null)
            {
                turret.OpenUpgradeUI();
            }
            else
            {
                MageSlomo mageTurret = towerObj.GetComponent<MageSlomo>();
                if (mageTurret != null)
                {
                    mageTurret.OpenUpgradeUIMage();
                }
            }
            return;
        }

        // Get the tower to build.
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild.cost > GameManager.Instance.playerCurrency)
        {
            Debug.Log("You can't afford this tower");
            return;
        }

        if (!GameManager.Instance.SpendCurrency(towerToBuild.cost))
            return;

        Vector3 adjustedPosition = transform.position + towerToBuild.offset;

        // Instantiate only one tower.
        towerObj = Instantiate(towerToBuild.prefab, adjustedPosition, Quaternion.identity);

        if (towerObj.GetComponent<Turret>() != null)
        {
            // It’s a turret – nothing more needed.
        }
        else if (towerObj.GetComponent<MageSlomo>() != null)
        {
            // It’s a mage turret – nothing more needed.
        }

        // (Optional) Check if clicking on UI.
        if (EventSystem.current.IsPointerOverGameObject()) return;
    }
}
