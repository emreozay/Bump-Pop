using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI ballText;

    public static Action<int> BallText;
    public static Action<int> MoneyText;

    private int ballCount;
    private int moneyCount;

    private void Awake()
    {
        BallText += UpdateBallText;
        MoneyText += UpdateMoneyText;
    }

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        levelText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UpdateBallText(int newBall)
    {
        ballCount += newBall;
        StartCoroutine(UpdateTextSlowly(ballCount, ballText));
    }

    private void UpdateMoneyText(int newMoney)
    {
        moneyCount += newMoney;
        StartCoroutine(UpdateTextSlowly(moneyCount, moneyText));
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BuyBall()
    {
        StartCoroutine(WaitForStop());
    }

    public void BuyIncome()
    {
        StartCoroutine(WaitForStop());
    }

    IEnumerator WaitForStop()
    {
        yield return new WaitForEndOfFrame();

        DragAndShoot.isStopNow = false;
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
    }
}
