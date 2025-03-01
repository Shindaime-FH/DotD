using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string settingsScene = "SettingsMenu";

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(settingsScene, LoadSceneMode.Additive);
        SceneManager.sceneUnloaded += OnSettingsClosed;
        pauseMenuUI.SetActive(false);
    }

    private void OnSettingsClosed(Scene scene)
    {
        if (scene.name == settingsScene)
        {
            pauseMenuUI.SetActive(true);
            SceneManager.sceneUnloaded -= OnSettingsClosed;
        }
    }

    public void Resume() => TogglePause();

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}