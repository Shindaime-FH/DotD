using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float levelDuration = 300f;
    private EnemySpawner spawner;

    [System.Obsolete]
    private void Start()
    {
        spawner = FindObjectOfType<EnemySpawner>();
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        float timeLeft = levelDuration;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateDisplay(timeLeft);
            yield return null;
        }

        spawner.StopAllSpawning();
        yield return new WaitUntil(() => spawner.EnemiesAlive <= 0);
        SceneManager.LoadScene("WinningScreenMenu");
    }

    private void UpdateDisplay(float time)
    {
        timerText.text = string.Format("{0:00}:{1:00}",
            Mathf.FloorToInt(time / 60),
            Mathf.FloorToInt(time % 60));
    }
}