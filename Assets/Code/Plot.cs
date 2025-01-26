using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    public Turret turret;
    private Color startColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        if (towerObj != null)
        {
            turret.OpenUpgradeUI();
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You can't afford this tower");
            return;
        }
        
        LevelManager.main.SpendCurrency(towerToBuild.cost);

        /*Vector3 newPosition = new Vector3(0,0.2f,0); // correction of prefab position for y 
        tower = Instantiate(towerToBuild.prefab, transform.position + newPosition, Quaternion.identity);*/    //old code

        /* Vector3 newPosition;

        // Check for the type of tower and apply specific position correction
        if (towerToBuild.prefab.name == "Mage Turret") // Replace "MageTower" with the exact prefab name
        {
            newPosition = new Vector3(0, 0.3f, 0); // Adjust the Y offset for the mage tower
        }
        else
        {
            newPosition = new Vector3(0, 0.2f, 0); // Default offset for other towers
        }

        tower = Instantiate(towerToBuild.prefab, transform.position + newPosition, Quaternion.identity);*/  // alt. code for repositioning the second spirte --> very unclean

        Vector3 adjustedPosition = transform.position + towerToBuild.offset;

        towerObj = Instantiate(towerToBuild.prefab, adjustedPosition, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();

        if (EventSystem.current.IsPointerOverGameObject()) return;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
