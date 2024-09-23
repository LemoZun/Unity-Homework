using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameClear = false;
    public static GameManager Instance { get; private set; }
    [SerializeField] GameObject gameClearText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] TextMeshProUGUI scoreBoard;
    private int score = 0;
    public int Score { get => score; set => score = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //SceneManager.sceneLoaded += DisableClearText;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateClearText(false);
        UpdateGameOverText(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    // 스코어 추가
    public void AddScroe(int _point)
    {
        score += _point;
        UpdateScoreBoard();
    }

    private void UpdateScoreBoard()
    {
        scoreBoard.text = "Score : " + score;
    }



    public void ClearGame()
    {
        Debug.Log("게임 클리어");
        gameClear = true;
        UpdateClearText(true);


    }

    public void GameOverTrigger()
    {
        Debug.Log("게임오버");
        UpdateGameOverText(true);
        Time.timeScale = 0f;
    }


    public void Restart()
    {
        Debug.Log("재시작");
        gameClear = false;
        UpdateClearText(false);
        UpdateGameOverText(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;

    }

    //TextMeshPro들 활성화 및 비활성화
    private void UpdateClearText(bool isClear)
    {
        gameClearText.SetActive(isClear);
    }

    private void UpdateGameOverText(bool isDead)
    {
        gameOverText.SetActive(isDead);
    }


}
