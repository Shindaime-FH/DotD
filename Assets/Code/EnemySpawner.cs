using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 16;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Timing Settings")]
    [SerializeField] private float totalTime = 180f;
    [SerializeField] private float timeBetweenWaves = 1f;
    [SerializeField] private float initialDelay = 2f;

    [Header("Level Timing")]
    [SerializeField] private float level3TotalTime = 420f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    // Private state variables
    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; // Enemies per second
    private float timeElapsed = 0f;
    private bool levelActive = true;
    private bool shouldSpawnWaves = true;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        // Set level-specific timing
        if (GameManager.Instance.currentLevel == 3)
        {
            totalTime = level3TotalTime;
        }

        StartCoroutine(SpawnWavesContinuously());
    }

    private void Update()
    {
        if (!levelActive || !shouldSpawnWaves) return;

        // Track total level time
        timeElapsed += Time.deltaTime;

        // Handle per-wave spawning
        if (enemiesLeftToSpawn > 0)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= (1f / eps))
            {
                SpawnEnemy();
                enemiesLeftToSpawn--;
                enemiesAlive++;
                timeSinceLastSpawn = 0f;
            }
        }

        // Check level completion when time expires
        if (timeElapsed >= totalTime)
        {
            levelActive = false;
            shouldSpawnWaves = false;
            StopAllSpawning();
            CheckLevelCompletion();
        }
    }

    private IEnumerator SpawnWavesContinuously()
    {
        yield return new WaitForSeconds(initialDelay);

        while (shouldSpawnWaves && timeElapsed < totalTime)
        {
            StartWave();

            // Wait until current wave is fully spawned
            while (enemiesLeftToSpawn > 0 && timeElapsed < totalTime)
            {
                yield return null;
            }

            // Wait for next wave
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWave++;
        }
    }

    private void StartWave()
    {
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void CheckLevelCompletion()
    {
        if (GameManager.Instance.currentLevel == 3)
        {
            StartCoroutine(CheckEnemiesBeforeWin());
        }
        else
        {
            // Existing logic for other levels
            if (enemiesAlive <= 0)
            {
                StopAllSpawning();
            }
        }
    }

    private IEnumerator CheckEnemiesBeforeWin()
    {
        while (enemiesAlive > 0)
        {
            yield return null;
        }
        GameManager.Instance.CompleteLevel();
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[index], LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor),
            0f, enemiesPerSecondCap);
    }

    public void StopAllSpawning()
    {
        StopAllCoroutines();
        shouldSpawnWaves = false;
        enemiesLeftToSpawn = 0;
    }
}