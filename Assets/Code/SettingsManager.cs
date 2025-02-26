using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle musicToggleButton;
    [SerializeField] private Image musicToggleCheckmark;
    [SerializeField] private Button closeButton; 

    private CanvasGroup canvasGroup;
    public static SettingsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        settingsCanvas.SetActive(false);
    }

    private void Start()
    {
        // Load settings
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bool isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        volumeSlider.value = savedVolume;
        musicToggleButton.isOn = !isMusicMuted;
        musicToggleCheckmark.enabled = !isMusicMuted;

        // Set up listeners
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        musicToggleButton.onValueChanged.AddListener(HandleToggleMusic);
        closeButton.onClick.AddListener(CloseSettings); 

        canvasGroup = settingsCanvas.GetComponent<CanvasGroup>();
    }

    public void ToggleSettings()
    {
        bool newState = !settingsCanvas.activeSelf;
        settingsCanvas.SetActive(newState);
        Time.timeScale = newState ? 0f : 1f;
    }

    private IEnumerator FadeIn()
{
    canvasGroup.alpha = 0f;
    while (canvasGroup.alpha < 1f)
    {
        canvasGroup.alpha += Time.unscaledDeltaTime * 5f;
        yield return null;
    }
}

private IEnumerator FadeOut()
{
    while (canvasGroup.alpha > 0f)
    {
        canvasGroup.alpha -= Time.unscaledDeltaTime * 5f;
        yield return null;
    }
    settingsCanvas.SetActive(false);
    Time.timeScale = 1f; // Unpause the game
    Debug.Log("Settings faded out");
}

    public void CloseSettings()
    {
        StartCoroutine(FadeOut());
    }

    private void UpdateVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        MusicManager.Instance.SetVolume(value);
    }

    private void HandleToggleMusic(bool isOn)
    {
        bool isMuted = !isOn;
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        MusicManager.Instance.SetMute(isMuted);
        musicToggleCheckmark.enabled = isOn;
    }

    public void OpenSettings()
    {
        StartCoroutine(FadeIn());
    }
}