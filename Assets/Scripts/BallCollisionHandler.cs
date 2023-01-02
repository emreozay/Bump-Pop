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
            dragAndShoot.DisableShoot();

            Rigidbody ballRigidbody = GetComponent<Rigidbody>();

            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.AddForce(Vector3.forward * 15f, ForceMode.Impulse);
            //ballRigidbody.AddTorque(Vector3.right*100f, ForceMode.Impulse);
            //ballRigidbody.velocity = Vector3.forward * 15f;

            //ballRigidbody.AddForce(new Vector3(0, -1f, 6f) * 1f, ForceMode.Impulse);
            //ballRigidbody.AddForce(0, -10, 10, ForceMode.Impulse);
            //ballRigidbody.velocity = Vector3.forward * 5f;
        }

        if (other.CompareTag("Corner"))
        {
            Camera.main.GetComponent<CameraMovement>().RotateForCorner(other.transform.rotation);
        }
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