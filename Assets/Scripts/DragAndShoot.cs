using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Transform firstBall;

    [SerializeField]
    private float forceMultiplier = 3f;

    private Vector3 firstPosition;
    private Vector3 lastPosition;
    private Vector3 shootDirection;

    private Rigidbody rb;

    private float viewportXPosition;
    private bool isShoot;

    private void Awake()
    {
        rb = firstBall.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            lineRenderer.positionCount = 2;
            firstPosition = new Vector3(firstBall.position.x, 0, firstBall.position.z);
            lineRenderer.SetPosition(0, firstPosition);
        }

        if (Input.GetMouseButton(0))
        {
            RenderLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.positionCount = 0;

            Shoot();
        }
    }

    private void RenderLine()
    {
        viewportXPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition).x;
        lastPosition.x = Mathf.Lerp(-10f, 10f, viewportXPosition);
        lastPosition.y = 0;
        lastPosition.z = firstBall.position.z + 10;

        lineRenderer.SetPosition(1, lastPosition);
    }

    void Shoot()
    {
        isShoot = true;

        shootDirection.x = lastPosition.x - firstPosition.x;
        shootDirection.y = 0;
        shootDirection.z = 10;
        rb.AddForce(shootDirection * forceMultiplier, ForceMode.Impulse);
    }
}