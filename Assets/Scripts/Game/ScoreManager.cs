using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public float BestScore { get; set; }
    public float CurrentScore { get; set; }
    [SerializeField]
    private float baseScore = 10f;
    [SerializeField]
    private float chainMergeScore = 20f;
    [SerializeField]
    private float comboScore = 30f;
    [SerializeField]
    private float compareScore = 40f;

    public float itemValue = 0;

    public bool IsScoreIncreaseByPattern { get; set; } = false;
    public bool IsScoreIncreaseByItem { get; set; } = false;

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

        Instance.Init();
    }

    public void Init()
    {
        UpdateBestScore();
        UIManager.Instance.UpdateBestScoreUI(BestScore);
        IsScoreIncreaseByPattern = false;
        IsScoreIncreaseByItem = false;
        CurrentScore = 0f;
    }

    public void UpdateBestScore()
    {
        BestScore = CurrentScore > BestScore ? CurrentScore : BestScore;
    }

    public void AddScoreBase()
    {
        ApplyScore(baseScore);
    }

    public void AddScoreByConnected(int count)
    {
        int factor = count - 2;
        ApplyScore(chainMergeScore * factor);
    }

    public void AddScoreByCombo()
    {
        Debug.Log("Combo");
        ApplyScore(comboScore);
    }

    public void AddScoreByComparePattern()
    {
        Debug.Log("Compare");
        ApplyScore(compareScore);
    }

    public void ApplyScore(float score)
    {
        if (IsScoreIncreaseByPattern)
            CurrentScore += score * 1.5f;

        if (IsScoreIncreaseByItem)
            CurrentScore += score * itemValue;

        if (!IsScoreIncreaseByPattern && !IsScoreIncreaseByItem)
            CurrentScore += score;


        UIManager.Instance.UpdateScoreUI(CurrentScore);
    }
}
