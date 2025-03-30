using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            SoundFXManager.Instance.PlayButtonClick();
        });
    }
}