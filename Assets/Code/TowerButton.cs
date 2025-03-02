using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private int towerIndex;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    private void Start()
    {
        // Subscribe to the selection event
        BuildManager.main.OnTowerSelected += UpdateSelection;
        // Set initial color
        UpdateSelection(BuildManager.main.GetSelectedTowerIndex());
    }

    private void OnDestroy()
    {
        BuildManager.main.OnTowerSelected -= UpdateSelection;
    }

    public void SelectTower()
    {
        BuildManager.main.SetSelectedTower(towerIndex);
    }

    private void UpdateSelection(int selectedIndex)
    {
        buttonImage.color = (selectedIndex == towerIndex) ? selectedColor : normalColor;
    }
}