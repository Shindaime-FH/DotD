using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouse_over = false;

    private void Update()
    {
        // Close the UI if clicking outside the upgrade UI
        if (Input.GetMouseButtonDown(0) && !mouse_over && !EventSystem.current.IsPointerOverGameObject())
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true; // Mouse is hovering over the upgrade UI
        UIManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false; // Mouse is no longer hovering over the upgrade UI
        UIManager.main.SetHoveringState(false);
    }
}