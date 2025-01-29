using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    public bool isHoveringUI;

    public void Awake()
    {
        main = this;
    }

    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }
}
