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

    private void Start()
    {
        // Initialize button
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipCutscene);
        }
        else
        {
            Debug.LogWarning("Skip button reference missing!");
        }

        // Auto-advance setup
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += EndReached;
        }
        else
        {
            Debug.LogError("VideoPlayer reference missing!");
        }
    }

    public void SkipCutscene()
    {
        if (videoPlayer != null) videoPlayer.Stop();
        SceneManager.LoadScene(nextScene);
    }

    void EndReached(VideoPlayer vp) => SceneManager.LoadScene(nextScene);
}