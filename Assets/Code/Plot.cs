using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject turret;
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
        if (turret != null) return;

        GameObject turretToBuild = BuildManager.main.GetSelectedTurret();
        Vector3 newPosition = new Vector3(0,0.2F,0); // correction of prefab position for y 
        turret = Instantiate(turretToBuild, transform.position + newPosition, Quaternion.identity);
       
    }


    // Update is called once per frame
    void Update()
    {

    }
}
