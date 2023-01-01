using TMPro;
using UnityEngine;

public class FinalText : MonoBehaviour
{
    private TextMeshPro finalText;

    private int remainingBall;
    private float remainingTimeForLose = 2f;
    private bool canFinish;

    private void Awake()
    {
        finalText = GetComponentInChildren<TextMeshPro>();
        remainingBall = int.Parse(finalText.text);
    }

    private void Update()
    {
        LevelCompleted();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InactiveBall"))
        {
            //other.GetComponent<Rigidbody>().AddForce(Vector3.forward * 15f, ForceMode.Impulse);

            if (remainingBall <= 0)
                return;

            remainingTimeForLose = 2f;
            canFinish = true;

            remainingBall--;
            finalText.text = remainingBall.ToString();

            if (remainingBall == 0)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                canFinish = false;
            }
        }
    }

    private void LevelCompleted()
    {
        if (canFinish)
        {
            remainingTimeForLose -= Time.deltaTime;

            if (remainingTimeForLose <= 0f)
            {
                print("Finish");
                canFinish = false;

                GameManager.Instance.UpdateLevel();
                UIManager.LevelCompleted();
            }
        }
    }
}
