using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private float lobbyBase = 0.5f;
    private float cutsceneBase = 0.2f;
    private float levelBase = 0.04f;
    public float masterVolume = 1f;
    public bool isMuted = false;

    private readonly Dictionary<string, float> categoryBaseVolumes = new();
    private readonly Dictionary<AudioSource, string> audioSources = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeCategories();
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCategories()
    {
        categoryBaseVolumes.Add("Lobby", lobbyBase);
        categoryBaseVolumes.Add("Cutscene", cutsceneBase);
        categoryBaseVolumes.Add("Level", levelBase);
    }

    public void RegisterSource(AudioSource source, string category)
    {
        if (source == null) return;

        if (categoryBaseVolumes.ContainsKey(category))
        {
            audioSources[source] = category;
            UpdateSourceVolume(source);
        }
    }

    public void UnregisterSource(AudioSource source)
    {
        if (source != null && audioSources.ContainsKey(source))
        {
            audioSources.Remove(source);
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
        SaveSettings();
    }

    public void SetMuted(bool muted)
    {
        isMuted = muted;
        UpdateAllVolumes();
        SaveSettings();
    }

    private void UpdateAllVolumes()
    {
        // Create a list to collect null references
        List<AudioSource> toRemove = new();

        foreach (var source in audioSources.Keys)
        {
            if (source == null)
            {
                toRemove.Add(source);
                continue;
            }

            if (audioSources.TryGetValue(source, out string category))
            {
                if (categoryBaseVolumes.TryGetValue(category, out float baseVol))
                {
                    source.volume = isMuted ? 0 : baseVol * masterVolume;
                }
            }
        }

        // Remove null references
        foreach (var source in toRemove)
        {
            audioSources.Remove(source);
        }
    }

    private void UpdateSourceVolume(AudioSource source)
    {
        if (!audioSources.TryGetValue(source, out string category)) return;
        if (!categoryBaseVolumes.TryGetValue(category, out float baseVol)) return;

        source.volume = isMuted ? 0 : baseVol * masterVolume;
    }

    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    }
}