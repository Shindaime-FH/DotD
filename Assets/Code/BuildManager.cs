using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    private int selectedTower = 0;

    public event System.Action<int> OnTowerSelected; // New event

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
        OnTowerSelected?.Invoke(_selectedTower); // Trigger event
    }

    public int GetSelectedTowerIndex() // Helper method
    {
        return selectedTower;
    }

    private void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    // Call this when failing level 2 to clear defences.
    [System.Obsolete]
    public void ClearTowers()
    {
        Turret[] turrets = Object.FindObjectsOfType<Turret>();
        foreach (Turret t in turrets)
        {
            Destroy(t.gameObject);
        }
        MageSlomo[] mageTowers = FindObjectsOfType<MageSlomo>();
        foreach (MageSlomo m in mageTowers)
        {
            Destroy(m.gameObject);
        }
    }
}