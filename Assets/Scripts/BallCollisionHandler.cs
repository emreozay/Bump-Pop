using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    private Transform ballParent;

    private DragAndShoot dragAndShoot;

    private bool isCollide;

    private void Start()
    {
        ballParent = GameObject.Find("Balls").transform;
        dragAndShoot = ballParent.GetComponent<DragAndShoot>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ActiveBall"))
        {
            isCollide = true;
            collision.transform.tag = "InactiveBall";
            collision.gameObject.GetComponent<BallCollisionHandler>().SpawnBalls();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            FinishLineTriggered();
        }
    }

    private void FinishLineTriggered()
    {
        dragAndShoot.DisableShoot();

        Rigidbody ballRigidbody = GetComponent<Rigidbody>();

        ballRigidbody.constraints = RigidbodyConstraints.None;

        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.AddForce(Vector3.forward * 15f, ForceMode.Impulse);

        Camera.main.GetComponent<CameraMovement>().ChangeOffsetForFinish();
    }

    public bool IsCollideWithBall()
    {
        return isCollide;
    }

    public void SpawnBalls()
    {
        int ballPerBall = GameManager.Instance.BallPerBall;

        for (int i = 0; i < ballPerBall; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, transform.position + Vector3.forward * 2.5f, Quaternion.identity, ballParent);

            Rigidbody newBallRigidbody = newBall.GetComponent<Rigidbody>();
            newBallRigidbody.AddForce(Vector3.forward * 3f, ForceMode.Impulse);
            newBallRigidbody.velocity = Vector3.forward * 30f;

            dragAndShoot.AddBallToList(newBall.transform);
        }

        UIManager.MoneyText(GameManager.Instance.IncomePerBall);
        UIManager.BallText(ballPerBall);
    }
}