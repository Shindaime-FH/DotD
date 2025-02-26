using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    // Public static instance for global access.
    public static MusicManager Instance;

    private AudioSource musicSource;
    private float targetVolume = 1.0f;
    [SerializeField] private float fadeDuration = 1.0f; // Duration for fade transitions

    void Awake()
    {
        // Singleton: if another instance exists, hand off new track and destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            AudioClip newTrack = GetComponent<AudioSource>().clip;
            if (newTrack != null && Instance.musicSource.clip != newTrack)
            {
                Instance.SwitchToTrack(newTrack);
            }
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        targetVolume = musicSource.volume;

        // If a track is already assigned, play it with a fade-in.
        if (musicSource.clip != null)
        {
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeInMusic());
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Optional: Implement scene-specific mapping here if needed.
    }

    /// <summary>
    /// Switches to a new music track with fade-out and fade-in.
    /// </summary>
    public void SwitchToTrack(AudioClip newClip)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndIn(newClip));
    }

    private System.Collections.IEnumerator FadeOutAndIn(AudioClip newClip)
    {
        if (musicSource.isPlaying)
        {
            float startVolume = targetVolume;
            for (float t = 0f; t < fadeDuration; t += Time.unscaledDeltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }
        }
        musicSource.volume = 0f;
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
        for (float t = 0f; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeInMusic()
    {
        musicSource.volume = 0f;
        for (float t = 0f; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }

    /// <summary>
    /// Sets the master volume.
    /// </summary>
    public void SetVolume(float volume)
    {
        targetVolume = volume;
        musicSource.volume = volume;
    }

    /// <summary>
    /// Mutes or unmutes the music.
    /// </summary>
    public void SetMute(bool mute)
    {
        if (mute)
        {
            musicSource.volume = 0f;
        }
        else
        {
            musicSource.volume = targetVolume;
        }
    }
}