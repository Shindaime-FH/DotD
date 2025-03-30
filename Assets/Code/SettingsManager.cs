using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeButton;
    [SerializeField] private RectTransform blurPanel;
    [SerializeField] private CanvasGroup canvasGroup; // Add this serialized field

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        // Load saved values or default to 1
        masterSlider.value = PlayerPrefs.GetFloat(SoundMixerManager.MASTER_VOLUME, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(SoundMixerManager.MUSIC_VOLUME, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(SoundMixerManager.SFX_VOLUME, 1f);

        // Add listeners
        masterSlider.onValueChanged.AddListener(SoundMixerManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SoundMixerManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SoundMixerManager.Instance.SetSFXVolume);
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