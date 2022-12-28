using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    private List<Transform> ballList = new List<Transform>();
    private Transform targetBall;
    private float highestZ = 0f;

    private Rigidbody rb;
    private CameraMovement cameraMovement;

    private float viewportXPosition;
    private bool isShoot;

    private void Awake()
    {
        rb = firstBall.GetComponent<Rigidbody>();
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
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

        GetTargetBall();
    }

    private void GetTargetBall()
    {
        for (int i = 0; i < ballList.Count; i++)
        {
            if (ballList[i].position.z > highestZ)
            {
                targetBall = ballList[i];
                highestZ = ballList[i].position.z;

                firstBall = targetBall;
                rb = firstBall.GetComponent<Rigidbody>();
            }
        }

        if (targetBall == null)
            return;

        cameraMovement.UpdateTargetObject(targetBall);
    }

    public void AddBallToList(Transform ball)
    {
        ballList.Add(ball);
    }

    private void RenderLine()
    {
        viewportXPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition).x;
        lastPosition.x = Mathf.Lerp(-10f, 10f, viewportXPosition);
        lastPosition.y = 0;
        lastPosition.z = firstBall.position.z + 10;

        lineRenderer.SetPosition(1, lastPosition);
    }

    private void Shoot()
    {
        isShoot = true;

        shootDirection.x = lastPosition.x - firstPosition.x;
        shootDirection.y = 0;
        shootDirection.z = 10;
        rb.AddForce(shootDirection * forceMultiplier, ForceMode.Impulse);
    }
}