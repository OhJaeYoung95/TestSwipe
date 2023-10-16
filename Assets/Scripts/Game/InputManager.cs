using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private float maxSwipeDistance = 50f;
    private float minSwipeDistance = 0.1f;
    public bool isHover = false;

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
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.IsMove && !GameManager.Instance.IsGameOver && !GameManager.Instance.IsPause && !isHover)
        {
            mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (Input.GetMouseButtonUp(0) && !GameManager.Instance.IsMove && !GameManager.Instance.IsGameOver && !GameManager.Instance.IsPause && !isHover)
        {
            GameManager.Instance.IsMove = true;
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectSwipe();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //GameManager.Instance.GameOver();
            UIManager.Instance.gameTimer = UIManager.Instance.gameDuration / 2;
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeDir = mouseUpPos - mouseDownPos;
        float swipeDistance = swipeDir.magnitude;

        if (swipeDistance < minSwipeDistance)
        {
            GameManager.Instance.IsMove = false;
            return;
        }

        if (swipeDistance < maxSwipeDistance)
        {
            float swipeAngle = Mathf.Atan2(swipeDir.y, swipeDir.x) * Mathf.Rad2Deg;
            StartCoroutine(BlockManager.Instance.MoveBlocks(swipeAngle));
        }
    }

    public void OnButtonHoverEnter()
    {
        isHover = true;
    }

    public void OnButtonHoverExit()
    {
        isHover = false;
    }
}