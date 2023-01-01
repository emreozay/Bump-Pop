using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private Rigidbody firstBallRigidbody;
    private CameraMovement cameraMovement;
    private BallCollisionHandler ballCollisionHandler;

    private Material lineMaterial;

    private float viewportXPosition;
    private bool canShoot = true;
    private bool isShot;

    private void Awake()
    {
        firstBallRigidbody = firstBall.GetComponent<Rigidbody>();
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
        ballCollisionHandler = firstBall.GetComponent<BallCollisionHandler>();
        lineMaterial = lineRenderer.material;
    }

    private void Update()
    {
        if (!GameManager.Instance.CanGameStart || !canShoot)
            return;

        GetTargetBall();
        CheckLose();

        if (isShot)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            UIManager.DisableStartPanel();

            firstBallRigidbody.velocity = Vector3.zero;
            firstBallRigidbody.angularVelocity = Vector3.zero;

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
            cameraMovement.CanRotate(false);

            Shoot();
        }
    }

    private void GetTargetBall()
    {
        for (int i = 0; i < ballList.Count; i++)
        {
            if (ballList[i].position.z > highestZ)
            {
                targetBall = ballList[i];
                highestZ = ballList[i].position.z;

                if (firstBall != targetBall)
                {
                    isShot = false;
                }

                firstBall = targetBall;
                firstBallRigidbody = firstBall.GetComponent<Rigidbody>();
                ballCollisionHandler = firstBall.GetComponent<BallCollisionHandler>();
            }
        }

        if (targetBall == null)
            return;

        cameraMovement.UpdateTargetObject(targetBall);
    }

    private void CheckLose()
    {
        if (isShot)
            StartCoroutine(WaitForCheck());
    }

    private IEnumerator WaitForCheck()
    {
        yield return new WaitForSeconds(1f);

        if (firstBallRigidbody.velocity.z <= 3.5f)
        {
            if (!ballCollisionHandler.IsCollideWithBall())
            {
                UIManager.LevelFailed();
            }
        }
    }

    public void AddBallToList(Transform ball)
    {
        ballList.Add(ball);
    }

    public Transform GetFirstBall()
    {
        return firstBall;
    }

    private void RenderLine()
    {
        viewportXPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition).x;
        lastPosition.x = Mathf.Lerp(-10f, 10f, viewportXPosition);
        lastPosition.y = 0;
        lastPosition.z = firstBall.position.z + 10;

        lineRenderer.SetPosition(1, lastPosition);

        cameraMovement.LookAtObject(lastPosition);

        lineMaterial.mainTextureOffset += Vector2.right * Time.deltaTime;
    }

    private void Shoot()
    {
        canShoot = true;
        isShot = true;

        shootDirection.x = lastPosition.x - firstPosition.x;
        shootDirection.y = 0;
        shootDirection.z = 10;
        firstBallRigidbody.AddForce(shootDirection * forceMultiplier, ForceMode.Impulse);
    }

    public void DisableShoot()
    {
        canShoot = false;
    }
}