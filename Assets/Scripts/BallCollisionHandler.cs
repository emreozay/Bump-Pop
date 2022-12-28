using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    private Transform ballParent;

    private DragAndShoot dragAndShoot;

    private void Start()
    {
        ballParent = GameObject.Find("Balls").transform;
        dragAndShoot = ballParent.GetComponent<DragAndShoot>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ActiveBall"))
        {
            collision.transform.tag = "InactiveBall";

            collision.gameObject.GetComponent<BallCollisionHandler>().SpawnBalls();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Rigidbody ballRigidbody = GetComponent<Rigidbody>();

            ballRigidbody.velocity = Vector3.zero;
            //ballRigidbody.AddForce(new Vector3(0, -50, 0.5f) * 30f, ForceMode.Impulse);
            ballRigidbody.AddForce(0, -10, 10, ForceMode.Impulse);
            //ballRigidbody.AddTorque(new Vector3(0, -30, 1) * 20f, ForceMode.Impulse);
        }
    }

    public void SpawnBalls()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, transform.position + Vector3.forward * 2.5f, Quaternion.identity, ballParent);

            Rigidbody newBallRigidbody = newBall.GetComponent<Rigidbody>();
            newBallRigidbody.AddForce(Vector3.forward * 3f, ForceMode.Impulse);
            newBallRigidbody.velocity = Vector3.forward * 3f;

            dragAndShoot.AddBallToList(newBall.transform);
        }
    }
}