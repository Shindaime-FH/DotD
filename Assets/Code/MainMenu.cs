using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPrefab;

    public void ShowSettings()
    {
        if (SettingsManager.Instance == null)
        {
            Instantiate(settingsPrefab);
        }
        SettingsManager.Instance.ToggleSettings();
    }

    public void PlayGame()
    {
        // Save audio settings before reset
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bool isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        // Reset game state on fresh start
        if (!PlayerPrefs.HasKey("IntroPlayed"))
        {
            GameManager.Instance.ReturnToMainMenu();
            PlayerPrefs.DeleteAll(); // Deletes all keys

            // Restore audio settings
            PlayerPrefs.SetFloat("MusicVolume", savedVolume);
            PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
            PlayerPrefs.Save();
        }

        StartCoroutine(LoadGameRoutine());
    }

    private IEnumerator LoadGameRoutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("IntroCutscene");
        while (!operation.isDone)
        {
            // Add loading screen progress here if needed
            yield return null;
        }

        // Ensure GameManager is ready
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetForNewGame();
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}