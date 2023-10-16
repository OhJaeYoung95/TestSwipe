using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button play;
    public Button option;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickOptionButton()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnClikQuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif    
    }
}
