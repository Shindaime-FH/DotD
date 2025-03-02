using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningScreenController : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}