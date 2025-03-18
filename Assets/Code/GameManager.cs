using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Persistent Data")]
    public int currentLevel = 1;
    public int playerCurrency = 500;

    private float savedLevel1Health = -1f;
    private float savedLevel2Health = -1f;
    private float savedLevel3Health = -1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // When a scene loads, update enemy level flags.
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        UpdateEnemyLevelFlags();
    }

    private void UpdateEnemyLevelFlags()
    {
        foreach (EnemyMovement enemy in FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None))
        {
            enemy.SetLevelFlags(currentLevel);
        }
    }

    [Obsolete]
    public void CompleteLevel()
    {
        Debug.Log($"Attempting to complete level {currentLevel}");

        CollectAllCoins();
        SaveCurrentLevelHealth();

        if (currentLevel == 3)
        {
            Debug.Log("Loading winning screen...");
            currentLevel = 1; // Reset progression
            SceneManager.LoadScene("WinningScreenMenu");
        }
        else
        {
            currentLevel++;
            Debug.Log($"Loading level {currentLevel}");
            SceneManager.LoadScene($"Level{currentLevel}");
        }
    }

    [Obsolete]
    public void CollectAllCoins()
    {
        CurrencyPickup[] coins = FindObjectsOfType<CurrencyPickup>();
        foreach (CurrencyPickup coin in coins)
        {
            if (coin.ShouldAutoCollect())
            {
                playerCurrency += coin.GetCurrencyWorth();
                Destroy(coin.gameObject);
            }
        }
    }

    [Obsolete]
    public void FailLevel()
    {
        SaveCurrentLevelHealth();

        foreach (EnemyMovement enemy in FindObjectsOfType<EnemyMovement>())
        {
            Destroy(enemy.gameObject);
        }

        if (currentLevel == 1)
        {
            Debug.Log("Game Over!");
            // SceneManager.LoadScene("Level1"); immidiate restart
            SceneManager.LoadScene("GameOverMenu");
        }
        else if (currentLevel == 2)
        {
            BuildManager.main.ClearTowers();
            currentLevel = 1;
            SceneManager.LoadScene("Level1");
        }
        else if (currentLevel == 3)
        {
            BuildManager.main.ClearTowers();
            currentLevel = 2;
            SceneManager.LoadScene("Level2");
        }
    }

    [Obsolete]
    private void SaveCurrentLevelHealth()
    {
        switch (currentLevel)
        {
            case 1:
                HealthManager hm = FindObjectOfType<HealthManager>();
                if (hm != null) savedLevel1Health = hm.healthAmount;
                break;
            case 2:
            case 3:
                DestructibleGate gate = FindObjectOfType<DestructibleGate>();
                if (gate != null)
                {
                    if (currentLevel == 2) savedLevel2Health = gate.currentHealth;
                    else savedLevel3Health = gate.currentHealth;
                }
                break;
        }
    }

    public float GetSavedHealth(int level)
    {
        return level switch
        {
            1 => savedLevel1Health,
            2 => savedLevel2Health,
            3 => savedLevel3Health,
            _ => -1f
        };
    }

    public void ReturnToMainMenu()
    {
        savedLevel1Health = -1f;
        savedLevel2Health = -1f;
        savedLevel3Health = -1f;

        currentLevel = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public bool SpendCurrency(int amount)
    {
        if (playerCurrency >= amount)
        {
            playerCurrency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough currency");
            return false;
        }
    }

    internal void ResetForNewGame()
    {
        currentLevel = 1;
        playerCurrency = 100;
        savedLevel1Health = -1f;
        savedLevel2Health = -1f;
        savedLevel3Health = -1f;
        PlayerPrefs.DeleteAll();
    }
    public bool IsInLevel()
    {
        return SceneManager.GetActiveScene().name.StartsWith("Level");
    }

    public void SetPauseState(bool paused)
    {
        if (IsInLevel())
        {
            Time.timeScale = paused ? 0 : 1;
        }
    }

}