using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1080, 1920, FullScreenMode.Windowed);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
