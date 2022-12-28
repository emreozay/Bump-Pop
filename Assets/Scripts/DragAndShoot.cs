using System.Collections.Generic;
using System.Linq;
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

    List<Transform> fillings = new List<Transform>();
    Transform highestFilling;
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
        foreach (Transform child in transform)
        {
            if (child.CompareTag("ActiveBall"))
                continue;

            fillings.Add(child);
        }

        for (int i = 0; i < fillings.Count; i++)
        {
            if (fillings[i].position.z > highestZ)
            {
                highestFilling = fillings[i];
                highestZ = fillings[i].position.z;
            }
        }

        //fillings = fillings.OrderBy(filling => filling.transform.position.z).ToList();
        //highestFilling = fillings.Last();

        if (highestFilling == null)
            return;

        cameraMovement.UpdateTargetObject(highestFilling);
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