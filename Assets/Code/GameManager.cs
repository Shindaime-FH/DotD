using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Persistent Data")]
    public int currentLevel = 1;
    public int playerCurrency = 100;

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
        foreach (EnemyMovement enemy in FindObjectsOfType<EnemyMovement>())
        {
            enemy.SetLevelFlags(currentLevel);
        }
    }

    public void CompleteLevel()
    {
        if (currentLevel < 3)
        {
            currentLevel++;
            SceneManager.LoadScene($"Level{currentLevel}");
        }
        else
        {
            Debug.Log("All levels completed!");
            // Optionally show a win screen.
        }
    }

    public void FailLevel()
    {
        // Clear any enemy objects to avoid carrying over old state.
        foreach (EnemyMovement enemy in FindObjectsOfType<EnemyMovement>())
        {
            Destroy(enemy.gameObject);
        }

        if (currentLevel == 1)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene("Level1");
        }
        else if (currentLevel == 2)
        {
            // Clear defences before going back to level 1.
            BuildManager.main.ClearTowers();
            currentLevel = 1;
            SceneManager.LoadScene("Level1");
        }
        else if (currentLevel == 3)
        {
            // On level 3 failure, go back to level 2 (keeping level 2 defences intact).
            currentLevel = 2;
            SceneManager.LoadScene("Level2");
        }
    }

    public void ReturnToMainMenu()
    {
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
}