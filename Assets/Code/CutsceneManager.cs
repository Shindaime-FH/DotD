using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button skipButton;
    [SerializeField] private string nextScene = "Level1";

    [SerializeField] private string videoURL = "https://cdn.jsdelivr.net/gh/Shindaime-FH/DotD@main/Assets/Video/Cutscene_2.mp4";

    private void Start()
    {
        ConfigureVideoPlayer();
        ConfigureSkipButton();
    }

    void ConfigureVideoPlayer()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer reference missing!");
            return;
        }

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;

        // Essential for WebGL audio
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;

        // Required for browser autoplay policies
        videoPlayer.playOnAwake = false;

        // Prepare video asynchronously
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.errorReceived += HandleVideoError;
        videoPlayer.Prepare();
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
    }

    void HandleVideoError(VideoPlayer source, string message)
    {
        Debug.LogError($"Video Error: {message}");
        // Fallback: Skip to next scene if video fails
        SceneManager.LoadScene(nextScene);
    }

    void ConfigureSkipButton()
    {
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipCutscene);
        }
        else
        {
            Debug.LogWarning("Skip button reference missing!");
        }
    }

    public void SkipCutscene()
    {
        if (videoPlayer != null) videoPlayer.Stop();
        SceneManager.LoadScene(nextScene);
    }

    void EndReached(VideoPlayer vp) => SceneManager.LoadScene(nextScene);
}