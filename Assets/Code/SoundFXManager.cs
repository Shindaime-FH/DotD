using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [Header("SFX Configuration")]
    [SerializeField] private GameObject soundFXPrefab;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip coinPickup;
    [SerializeField] private AudioClip towerPlaced;
    [SerializeField] private AudioClip enemyDamage;
    [SerializeField] private AudioClip mageAttack;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip knightDeath;
    [SerializeField] private AudioClip zombieDeath;
    [SerializeField] private AudioClip goblinDeath;
    [SerializeField] private AudioClip gateDamage;
    [SerializeField] private AudioClip castleDamage;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip gameWon;

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
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        var soundObj = Instantiate(soundFXPrefab, position, Quaternion.identity);
        var audioSource = soundObj.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(soundObj, clip.length);
    }

    // Specific sound methods
    public void PlayEnemyDamage(Vector3 position) => PlaySFX(enemyDamage, position);
    public void PlayTowerPlaced(Vector3 position) => PlaySFX(towerPlaced, position);
    public void PlayMageAttack(Vector3 position) => PlaySFX(mageAttack, position);
    public void PlayButtonClick() => PlaySFX(buttonClick, Vector3.zero);
    public void PlayKnightDeath(Vector3 position) => PlaySFX(knightDeath, position);
    public void PlayZombieDeath(Vector3 position) => PlaySFX(zombieDeath, position);
    public void PlayGoblinDeath(Vector3 position) => PlaySFX(goblinDeath, position);
    public void PlayGateDamage(Vector3 position) => PlaySFX(gateDamage, position);
    public void PlayCastleDamage(Vector3 position) => PlaySFX(castleDamage, position);
    public void PlayGameOver() => PlaySFX(gameOver, Vector3.zero);
    public void PlayGameWon() => PlaySFX(gameWon, Vector3.zero);
    public void PlayCoinPickup() => PlaySFX(coinPickup, Vector3.zero);
}