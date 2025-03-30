using UnityEngine.Audio;
using UnityEngine;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance { get; private set; }

    [SerializeField] private AudioMixer mainMixer;
    public const string MASTER_VOLUME = "masterVolume";
    public const string MUSIC_VOLUME = "musicVolume";
    public const string SFX_VOLUME = "soundFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeVolumes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MASTER_VOLUME, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_VOLUME, 1f));
    }

    public void SetMasterVolume(float volume) => SetVolume(MASTER_VOLUME, volume);
    public void SetMusicVolume(float volume) => SetVolume(MUSIC_VOLUME, volume);
    public void SetSFXVolume(float volume) => SetVolume(SFX_VOLUME, volume);

    private void SetVolume(string parameter, float volume)
    {
        float dB = volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
        mainMixer.SetFloat(parameter, dB);
        PlayerPrefs.SetFloat(parameter, volume);
    }
}