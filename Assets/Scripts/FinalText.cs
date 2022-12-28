using TMPro;
using UnityEngine;

public class FinalText : MonoBehaviour
{
    private TextMeshPro finalText;
    private int remainingBall;

    private void Awake()
    {
        finalText = GetComponentInChildren<TextMeshPro>();
        remainingBall = int.Parse(finalText.text);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InactiveBall"))
        {
            if (remainingBall == 0)
                return;

            remainingBall--;
            finalText.text = remainingBall.ToString();
        }
    }
}
