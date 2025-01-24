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

    private GameObject tower;
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
        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You can't afford this tower");
            return;
        }
        
        LevelManager.main.SpendCurrency(towerToBuild.cost);

        Vector3 newPosition = new Vector3(0,0.2F,0); // correction of prefab position for y 
        tower = Instantiate(towerToBuild.prefab, transform.position + newPosition, Quaternion.identity);
        if (EventSystem.current.IsPointerOverGameObject()) return;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
