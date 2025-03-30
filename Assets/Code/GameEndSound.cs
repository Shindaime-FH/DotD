using UnityEngine;

public class GameEndSound : MonoBehaviour
{
    [SerializeField] private bool isGameOver;

    private void Start()
    {
        if (isGameOver)
            SoundFXManager.Instance.PlayGameOver();
        else
            SoundFXManager.Instance.PlayGameWon();
    }
}
