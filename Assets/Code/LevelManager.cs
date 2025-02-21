using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("Path References")]
    public Transform startPoint;

    public Transform[] path;

    public Transform[] Pathscndlvlalt;
    public Transform[] Pathscndlvlmain;

    public Transform[] Pathsthirdlvlmain;
    public Transform[] PathsthirdlvlmainVar;
    public Transform[] Paththirdlvlalt;
    public Transform[] PaththirdlvlaltVar;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    public void IncreaseCurrency(int amount)
    {
        GameManager.Instance.playerCurrency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        return GameManager.Instance.SpendCurrency(amount);
    }
}
