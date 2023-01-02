using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;

    [SerializeField]
    private DragAndShoot dragAndShoot;
    [SerializeField]
    private Transform finishLine;

    [SerializeField]
    private Slider progressBar;

    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI ballText;

    [Header("More Ball")]
    [SerializeField]
    private Button ballButton;
    [SerializeField]
    private TextMeshProUGUI ballLevelText;
    [SerializeField]
    private TextMeshProUGUI ballPriceText;

    [Header("More Income")]
    [SerializeField]
    private Button incomeButton;
    [SerializeField]
    private TextMeshProUGUI incomeLevelText;
    [SerializeField]
    private TextMeshProUGUI incomePriceText;

    public static Action<int> BallText;
    public static Action<int> MoneyText;
    public static Action DisableStartPanel;
    public static Action LevelCompleted;
    public static Action LevelFailed;

    private Transform targetBall;
    private int ballCount;
    private int moneyCount;

    private void Awake()
    {
        BallText += UpdateBallText;
        MoneyText += UpdateMoneyText;
        DisableStartPanel += CloseStartPanel;
        LevelCompleted += LevelWin;
        LevelFailed += LevelLose;
    }

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        moneyCount = GameManager.Instance.TotalMoney;

        UpdateButtonsVisibility();
        UpdateAllTexts();
    }

    private void Update()
    {
        SetProgressBar();
    }

    private void SetProgressBar()
    {
        targetBall = dragAndShoot.GetFirstBall();

        progressBar.value = targetBall.position.z / finishLine.position.z;
    }

    private void CloseStartPanel()
    {
        startPanel.SetActive(false);
    }

    private void UpdateBallText(int newBall)
    {
        ballCount += newBall;
        StartCoroutine(UpdateTextSlowly(ballCount, ballText));
    }

    private void UpdateMoneyText(int newMoney)
    {
        moneyCount += newMoney;
        GameManager.Instance.SetTotalMoney(moneyCount);
        StartCoroutine(UpdateTextSlowly(moneyCount, moneyText));
    }

    private void LevelWin()
    {
        winPanel.SetActive(true);
    }

    private void LevelLose()
    {
        losePanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings >= GameManager.Instance.Level)
            SceneManager.LoadScene(GameManager.Instance.Level - 1);
    }

    public void BuyBall()
    {
        if (!GameManager.Instance.CanUpgradeBall())
            return;

        GameManager.Instance.IncreaseBallPerBall();
        UpdateButtonsVisibility();
        UpdateAllTexts();
    }

    public void BuyIncome()
    {
        if (!GameManager.Instance.CanUpgradeIncome())
            return;

        GameManager.Instance.IncreaseIncomePerBall();
        UpdateButtonsVisibility();
        UpdateAllTexts();
    }

    private void UpdateButtonsVisibility()
    {
        ballButton.interactable = GameManager.Instance.CanUpgradeBall();
        incomeButton.interactable = GameManager.Instance.CanUpgradeIncome();
    }

    private void UpdateAllTexts()
    {
        levelText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1);

        moneyText.text = GameManager.Instance.TotalMoney.ToString();

        ballLevelText.text = "LVL " + GameManager.Instance.BallLevel;
        ballPriceText.text = GameManager.Instance.BallPrice.ToString();

        incomeLevelText.text = "LVL " + GameManager.Instance.IncomeLevel;
        incomePriceText.text = GameManager.Instance.IncomePrice.ToString();
    }

    IEnumerator UpdateTextSlowly(int count, TextMeshProUGUI textForUpdate)
    {
        int currentNumber = int.Parse(textForUpdate.text);

        while (currentNumber < count)
        {
            currentNumber++;
            textForUpdate.text = currentNumber.ToString();

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDestroy()
    {
        BallText -= UpdateBallText;
        MoneyText -= UpdateMoneyText;
        DisableStartPanel -= CloseStartPanel;
        LevelCompleted -= LevelWin;
        LevelFailed -= LevelLose;
    }
}
