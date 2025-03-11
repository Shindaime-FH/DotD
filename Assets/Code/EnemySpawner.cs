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

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new();

    // Private state variables
    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; // Enemies per second
    private float timeElapsed = 0f;
    private bool levelActive = true;
    private bool shouldSpawnWaves = true;

    public int EnemiesAlive => enemiesAlive; // Public accessor

    public void ForceUpdateEnemyCount()
    {
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        
        if (GameManager.Instance.currentLevel == 3)
        {
            totalTime = 300f; // Directly set to 300 seconds
        }

        StartCoroutine(SpawnWavesContinuously());
    }

    [System.Obsolete]
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

    [System.Obsolete]
    private void CheckLevelCompletion()
    {
        StartCoroutine(CheckEnemiesBeforeWin());
            // Existing logic for other levels
            if (enemiesAlive <= 0)
            {
                StopAllSpawning();
            }
    }

    [System.Obsolete]
    private IEnumerator CheckEnemiesBeforeWin()
    {
        Debug.Log("Starting enemy check...");

        while (true)
        {
            // Double-check both tracking methods
            bool noSpawnerEnemies = enemiesAlive <= 0;
            bool noSceneEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length == 0;

            if (noSpawnerEnemies && noSceneEnemies)
            {
                Debug.Log("All enemies cleared!");
                GameManager.Instance.CompleteLevel();
                yield break;
            }

            Debug.Log($"Tracking: {enemiesAlive} in spawner, {GameObject.FindGameObjectsWithTag("Enemy").Length} in scene");
            yield return new WaitForSeconds(1f);
        }
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
        // Stop new spawns
        shouldSpawnWaves = false;
        enemiesLeftToSpawn = 0;
        StopAllCoroutines();

        // Destroy existing enemies and update count
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        enemiesAlive = 0;
    }
}