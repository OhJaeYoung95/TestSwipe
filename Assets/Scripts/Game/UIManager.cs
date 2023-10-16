using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private Slider hpBar;
    private Image hpBarFadeFrame;
    private TextMeshProUGUI bestScore;
    private TextMeshProUGUI score;

    public TextMeshProUGUI gameOverBestScore;
    public TextMeshProUGUI gameOverScore;


    private GameObject foreCanvas;
    private GameObject gameOverPanel;
    private GameObject pausePanel;

    private Button puaseButton;

    private Button selectStage;
    private Button restart;

    private Button pauseSelectStage;
    private Button continueButton;
    private Button quitButton;

    private GameObject itemSlot1;
    private GameObject itemSlot2;
    private GameObject itemSlot3;

    public float gameTimer;
    public float gameDuration = 20f;

    public float stopTimer = 0f;
    public float stopDuration = 0f;

    public float fadeSpeed = 5f;
    private bool isFadeHpBar = false;

    public bool isStopTimer = false;

    public float scoreItemTimer = 0f;
    public float scoreItemDuration = 0f;

    public ItemID[] items = new ItemID[3];

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

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (Instance == null)
            return;

        if (isStopTimer)
            stopTimer += Time.deltaTime;
        else
            gameTimer -= Time.deltaTime;

        if(stopTimer > stopDuration)
        {
            stopTimer = 0f;
            isStopTimer = false;
        }

        float t = gameTimer / gameDuration;
        // 에러 발생
        if (BlockManager.Instance != null)
        {
            if (t <= 0.5f && !BlockManager.Instance.isSpawnObstacle)
                BlockManager.Instance.isSpawnObstacle = true;
        }

        if (t <= 0.1f && !isFadeHpBar)
        {
            isFadeHpBar = true;
            hpBarFadeFrame.gameObject.SetActive(true);
        }

        if (t > 0.1f && isFadeHpBar)
        {
            isFadeHpBar = false;
            hpBarFadeFrame.gameObject.SetActive(false);
        }

        if (isFadeHpBar)
            FadeHpBarFrame();

        if(ScoreManager.Instance.IsScoreIncreaseByItem)
        {
            scoreItemTimer += Time.deltaTime;
        }

        if(scoreItemTimer > scoreItemDuration)
        {
            scoreItemTimer = 0f;
            ScoreManager.Instance.IsScoreIncreaseByItem = false;
        }

        if (hpBar != null)
            UpdateTimerUI(t);
        if (gameTimer <= 0f && !GameManager.Instance.IsGameOver)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void Init()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = ItemID.None;
        }
        int stage = PlayerPrefs.GetInt("CurrentStage", 1);
        switch (stage)
        {
            case 1:
                gameDuration = 30;
                break;
            case 0:
                gameDuration = 240;
                break;
            case 2:
                gameDuration = 300;
                break;
            default:
                gameDuration = 30;
                break;
        }
        ItemID itemInfo1 = (ItemID)PlayerPrefs.GetInt("ItemSlot1", 0);
        ItemID itemInfo2 = (ItemID)PlayerPrefs.GetInt("ItemSlot2", 0);
        ItemID itemInfo3 = (ItemID)PlayerPrefs.GetInt("ItemSlot3", 0);

        if(itemInfo1 != ItemID.None)
        {
            items[0] = itemInfo1;
            itemSlot1 = GameObject.FindGameObjectWithTag("ItemSlot1");
            ApplyItemSlotImage(itemSlot1, items[0]);
        }

        if (itemInfo2 != ItemID.None)
        {
            items[1] = itemInfo2;
            itemSlot2 = GameObject.FindGameObjectWithTag("ItemSlot2");
            ApplyItemSlotImage(itemSlot2, items[1]);
        }

        if (itemInfo3 != ItemID.None)
        {
            items[2] = itemInfo3;
            itemSlot3 = GameObject.FindGameObjectWithTag("ItemSlot3");
            ApplyItemSlotImage(itemSlot3, items[2]);
        }

        isStopTimer = false;

        gameTimer = gameDuration;

        hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<Slider>();
        hpBar.onValueChanged.AddListener(UpdateTimerUI);
        hpBarFadeFrame = hpBar.transform.GetChild(2).GetComponent<Image>();
        bestScore = GameObject.FindGameObjectWithTag("BestScore").GetComponent<TextMeshProUGUI>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        foreCanvas = GameObject.FindGameObjectWithTag("ForeCanvas");
        puaseButton = GameObject.FindGameObjectWithTag("Pause").GetComponent<Button>();

        gameOverPanel = foreCanvas.transform.GetChild(0).gameObject;
        gameOverPanel.gameObject.SetActive(false);
        pausePanel = foreCanvas.transform.GetChild(1).gameObject;
        pausePanel.gameObject.SetActive(false);

        gameOverBestScore = gameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameOverScore = gameOverPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        if (GameManager.Instance != null)
        {
            puaseButton.onClick.AddListener(GameManager.Instance.Pause);

            selectStage = gameOverPanel.transform.GetChild(3).GetChild(0).GetComponent<Button>();
            selectStage.onClick.AddListener(GameManager.Instance.SelectStage);
            restart = gameOverPanel.transform.GetChild(4).GetChild(0).GetComponent<Button>();
            restart.onClick.AddListener(GameManager.Instance.Restart);

            pauseSelectStage = pausePanel.transform.GetChild(1).GetChild(0).GetComponent<Button>();
            pauseSelectStage.onClick.AddListener(GameManager.Instance.SelectStage);

            continueButton = pausePanel.transform.GetChild(2).GetChild(0).GetComponent<Button>(); ;
            continueButton.onClick.AddListener(GameManager.Instance.Continue);
            EventTrigger eventTrigger = continueButton.GetComponent<EventTrigger>();
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { InputManager.Instance.OnButtonHoverEnter(); });
            eventTrigger.triggers.Add(enterEntry);
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { InputManager.Instance.OnButtonHoverExit(); });
            eventTrigger.triggers.Add(exitEntry);


            quitButton = pausePanel.transform.GetChild(3).GetChild(0).GetComponent<Button>(); ;
            quitButton.onClick.AddListener(GameManager.Instance.Quit);

        }
    }

    public void GameOver()
    {
        isStopTimer = false;
        gameOverPanel.gameObject.SetActive(true);
        ScoreManager.Instance.UpdateBestScore();
        gameOverBestScore.text = $"{ScoreManager.Instance.BestScore}";
        gameOverScore.text = $"{ScoreManager.Instance.CurrentScore}";
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        StartCoroutine(ContinueDelay());
    }

    public IEnumerator ContinueDelay()
    {
        yield return new WaitForSeconds(0.5f);
        InputManager.Instance.isHover = false;
    }

    public void FadeHpBarFrame()
    {
        float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
        Color newColor = hpBarFadeFrame.color;
        newColor.a = alpha;
        hpBarFadeFrame.color = newColor;
    }

    public void IncreaseTimer(float value)
    {
        gameTimer += value;
        Mathf.Clamp(gameTimer, 0f, gameDuration);
    }

    public void UpdateTimerUI(float value)
    {
        if (hpBar != null)
            hpBar.value = value;
    }

    public void UpdateBestScoreUI(float value)
    {
        bestScore.text = $"BEST SCORE \n {Mathf.RoundToInt(value)}";
    }
    public void UpdateScoreUI(float value)
    {
        score.text = $"SCORE \n {Mathf.RoundToInt(value)}";
    }

    public void ApplyItemSlotImage(GameObject slot, ItemID itemID)
    {
        Image itemSlotImage = slot.transform.GetChild(1).GetComponent<Image>();
        string itemImagePath = DataTableManager.GetTable<ItemTable>().GetItemInfo(itemID).path;
        itemSlotImage.sprite = Resources.Load<Sprite>($"Arts/{itemImagePath}");
        Color iamgeColor = itemSlotImage.color;
        iamgeColor.a = 255;
        itemSlotImage.color = iamgeColor;
    }
}
