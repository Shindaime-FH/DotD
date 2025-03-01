using UnityEngine;

public class AudioSourceRegistrar : MonoBehaviour
{
    [SerializeField] private string audioCategory;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (AudioManager.Instance != null && audioSource != null)
        {
            AudioManager.Instance.RegisterSource(audioSource, audioCategory);
        }
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null && audioSource != null)
        {
            AudioManager.Instance.UnregisterSource(audioSource);
        }
    }
}