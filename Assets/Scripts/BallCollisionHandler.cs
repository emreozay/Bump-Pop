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

    public void SpawnBalls()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, transform.position + Vector3.forward * 2.5f, Quaternion.identity, ballParent);

            newBall.GetComponent<Rigidbody>().AddForce(Vector3.forward * 3f, ForceMode.Impulse);
            newBall.GetComponent<Rigidbody>().velocity = Vector3.forward * 3f;

            dragAndShoot.AddBallToList(newBall.transform);
        }
    }
}