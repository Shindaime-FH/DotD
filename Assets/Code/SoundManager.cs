using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip coinPickupSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Singleton-Muster für globalen Zugriff
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayCoinSound()
    {
        if (coinPickupSound != null)
        {
            audioSource.PlayOneShot(coinPickupSound);
        }
    }
}
