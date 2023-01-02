using TMPro;
using UnityEngine;

public class FinalText : MonoBehaviour
{
    [SerializeField]
    private Transform chainParent;

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
            SetFinalText();
        }
    }

    private void SetFinalText()
    {
        if (remainingBall <= 0)
            return;

        remainingTimeForLose = 2f;
        canFinish = true;

        remainingBall--;
        finalText.text = remainingBall.ToString();

        if (remainingBall == 0)
        {
            DestroyChain();
            canFinish = false;
        }
    }

    private void LevelCompleted()
    {
        if (canFinish)
        {
            remainingTimeForLose -= Time.deltaTime;

            if (remainingTimeForLose <= 0f)
            {
                canFinish = false;

                GameManager.Instance.UpdateLevel();
                UIManager.LevelCompleted();
            }
        }
    }

    private void DestroyChain()
    {
        foreach (Transform childTransform in chainParent)
        {
            FixedJoint childFixedJoint = childTransform.GetComponent<FixedJoint>();

            if (childFixedJoint != null)
                Destroy(childFixedJoint);
        }

        foreach (Transform childTransform in chainParent)
        {
            Rigidbody childRigidbody = childTransform.GetComponent<Rigidbody>();
            childRigidbody.AddForce(new Vector3(Random.Range(-20f, 20f), 10f, 2f), ForceMode.Impulse);

            Destroy(childTransform.gameObject, 1f);
        }
    }
}
