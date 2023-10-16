using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsMove { get; set; } = false;
    public bool IsGameOver { get; private set; } = false;
    public bool IsPause { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Instance.IsGameOver = false;
        Instance.IsPause = false;
        Instance.IsMove = false;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBestScoreUI(ScoreManager.Instance.BestScore);
            UIManager.Instance.UpdateScoreUI(ScoreManager.Instance.CurrentScore);
        }
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("ItemSlot1", 0);
        PlayerPrefs.SetInt("ItemSlot2", 0);
        PlayerPrefs.SetInt("ItemSlot3", 0);

        IsGameOver = true;
        IsMove = false;
        Time.timeScale = 0f;
        StopAllCoroutinesOfSingleTon();
        BlockManager.Instance.ClearBoard();
        UIManager.Instance.GameOver();
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        UIManager.Instance.gameTimer = UIManager.Instance.gameDuration;
        StopAllCoroutinesOfSingleTon();
        BlockManager.Instance.ClearBoard();
        ScoreManager.Instance.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SelectStage()
    {
        PlayerPrefs.SetInt("ItemSlot1", 0);
        PlayerPrefs.SetInt("ItemSlot2", 0);
        PlayerPrefs.SetInt("ItemSlot3", 0);

        Time.timeScale = 0f;
        StopAllCoroutinesOfSingleTon();
        ScoreManager.Instance.CurrentScore = 0;
        BlockManager.Instance.ClearBoard();
        SceneManager.LoadScene(2);
    }

    public void Pause()
    {
        IsPause = true;
        UIManager.Instance.Pause();
    }

    public void Continue()
    {
        IsPause = false;
        UIManager.Instance.Continue();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        PlayerPrefs.SetInt("ItemSlot1", 0);
        PlayerPrefs.SetInt("ItemSlot2", 0);
        PlayerPrefs.SetInt("ItemSlot3", 0);

        EditorApplication.isPlaying = false;
#else
        PlayerPrefs.SetInt("ItemSlot1", 0);
        PlayerPrefs.SetInt("ItemSlot2", 0);
        PlayerPrefs.SetInt("ItemSlot3", 0);
        Application.Quit();
#endif    
    }

    public void StopAllCoroutinesOfSingleTon()
    {
        BlockManager.Instance.StopAllCoroutines();
        InputManager.Instance.StopAllCoroutines();
    }
}
