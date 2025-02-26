using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
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