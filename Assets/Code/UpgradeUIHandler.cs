using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouse_over = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;      // so the mous  e hovering over the Upgrade recognizes it
        UIManager.main.SetHoveringState(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;     // so the mouse doesn't accidentally click besides the Upgrade to create a tower
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);        // this will close the UI for us immediately
    }


}
