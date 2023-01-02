using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private bool canGameStart = true;

    private int level = 1;

    private int totalBall;
    private int ballLevel = 1;
    private int ballPrice = 10;
    private int ballPerBall = 20;

    private int totalMoney;
    private int incomeLevel = 1;
    private int incomePrice = 10;
    private int incomePerBall = 1;

    public int Level { get { return level; } }

    public int TotalBall { get { return totalBall; } }
    public int BallLevel { get { return ballLevel; } }
    public int BallPrice { get { return ballPrice; } }
    public int BallPerBall { get { return ballPerBall; } }

    public int TotalMoney { get { return totalMoney; } }
    public int IncomeLevel { get { return incomeLevel; } }
    public int IncomePrice { get { return incomePrice; } }
    public int IncomePerBall { get { return incomePerBall; } }

    public bool CanGameStart { get { return canGameStart; } set { canGameStart = value; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        LoadData();

        if (SceneManager.sceneCountInBuildSettings >= level)
            SceneManager.LoadScene(level - 1);
    }

    public void UpdateLevel()
    {
        level++;
    }

    public void SetTotalMoney(int money)
    {
        totalMoney = money;
    }

    public void SetTotalBall(int ball)
    {
        totalBall = ball;
    }

    public void IncreaseIncomePerBall()
    {
        totalMoney -= incomePrice;

        incomePerBall++;
        incomePrice *= 2;
        incomeLevel++;
    }

    public void IncreaseBallPerBall()
    {
        totalMoney -= ballPrice;

        ballPerBall++;
        ballPrice *= 2;
        ballLevel++;
    }

    public bool CanUpgradeBall()
    {
        return totalMoney >= ballPrice;
    }

    public bool CanUpgradeIncome()
    {
        return totalMoney >= incomePrice;
    }

    private void LoadData()
    {
        level = PlayerPrefs.GetInt("LEVEL", 1);

        totalBall = PlayerPrefs.GetInt("TOTAL_BALL", 1);
        ballLevel = PlayerPrefs.GetInt("BALL_LEVEL", 1);
        ballPrice = PlayerPrefs.GetInt("BALL_PRICE", 10);
        ballPerBall = PlayerPrefs.GetInt("BALL_PER_BALL", 20);

        totalMoney = PlayerPrefs.GetInt("TOTAL_MONEY", 0);
        incomeLevel = PlayerPrefs.GetInt("INCOME_LEVEL", 1);
        incomePrice = PlayerPrefs.GetInt("INCOME_PRICE", 10);
        incomePerBall = PlayerPrefs.GetInt("INCOME_PER_BALL", 1);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("LEVEL", level);

        PlayerPrefs.SetInt("TOTAL_BALL", totalBall);
        PlayerPrefs.SetInt("BALL_LEVEL", ballLevel);
        PlayerPrefs.SetInt("BALL_PRICE", ballPrice);
        PlayerPrefs.SetInt("BALL_PER_BALL", ballPerBall);

        PlayerPrefs.SetInt("TOTAL_MONEY", totalMoney);
        PlayerPrefs.SetInt("INCOME_LEVEL", incomeLevel);
        PlayerPrefs.SetInt("INCOME_PRICE", incomePrice);
        PlayerPrefs.SetInt("INCOME_PER_BALL", incomePerBall);
    }

    private void OnDestroy()
    {
        SaveData();
       // PlayerPrefs.DeleteAll();
    }
}
