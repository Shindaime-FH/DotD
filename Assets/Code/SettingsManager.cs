using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Button closeButton;
    [SerializeField] private RectTransform blurPanel;
    [SerializeField] private CanvasGroup canvasGroup; // Add this serialized field

    private void Awake()
    {

    }

    private void Start()
    {
        InitializeUI();
        StartCoroutine(FadeIn());
    }

    private void InitializeUI()
    {
        // Audio Controls
        volumeSlider.value = AudioManager.Instance.masterVolume;
        muteToggle.isOn = !AudioManager.Instance.isMuted;

        volumeSlider.onValueChanged.AddListener(v => {
            AudioManager.Instance.SetMasterVolume(v);
        });

        muteToggle.onValueChanged.AddListener(m => {
            AudioManager.Instance.SetMuted(!m);
        });

        // Close Button
        closeButton.onClick.AddListener(CloseSettings);
    }

    public void CloseSettings()
    {
        StartCoroutine(CloseRoutine());
    }

    private IEnumerator FadeIn()
    {
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime * 5f;
            yield return null;
        }
    }

    private IEnumerator CloseRoutine()
    {

        if (canvasGroup != null)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime * 5f;
                yield return null;
            }
        }

        SceneManager.UnloadSceneAsync("SettingsMenu");
    }
}