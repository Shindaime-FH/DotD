using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currency = 100;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item");
            return false;
        }
    }
}
