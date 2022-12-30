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

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        levelText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1);
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
}
