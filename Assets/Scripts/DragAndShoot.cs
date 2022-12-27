using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float forceMultiplier = 3f;

    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Vector3 first;

    private Rigidbody rb;

    private bool isShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePressDownPos = hit.point.normalized;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                first = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 newPos = hit.point;
                newPos.y = 0;

                RenderLine(newPos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                lineRenderer.positionCount = 0;

                mouseReleasePos = hit.point.normalized;
                mouseReleasePos.z = Mathf.Abs(mouseReleasePos.z);

                Vector3 shootDirection = (mouseReleasePos - mousePressDownPos).normalized;
                Shoot(hit.point);
            }
        }
    }

    private void RenderLine(Vector3 endPosition)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 0, transform.position.z));
        /*
        Vector3 firstPosition = transform.position;
        firstPosition.y = 0;
        firstPosition.z = Mathf.Abs(firstPosition.z);
        lineRenderer.SetPosition(0, firstPosition);

        Vector3 lastPosition = (endPosition - mousePressDownPos).normalized * 5f;
        lastPosition.y = 0;
        lastPosition.z = 10;
        lineRenderer.SetPosition(1, firstPosition + lastPosition);*/

        /*Vector3 finalPos = transform.position + (endPosition * 10f);
        finalPos.y = 0;
        finalPos.z = transform.position.z + 10;
        lineRenderer.SetPosition(1, finalPos);*/

        Vector3 finalPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        finalPos.x = Mathf.Lerp(-10f, 10f, finalPos.x);
        finalPos.y = 0;
        finalPos.z = transform.position.z + 10;
        
        first.x = finalPos.x - lineRenderer.GetPosition(0).x;
        lineRenderer.SetPosition(1, finalPos);
    }

    void Shoot(Vector3 Force)
    {
        /*if (isShoot)
            return;*/

        //rb.AddForce(new Vector3(Force.x * 10f, 0, 10) * forceMultiplier, ForceMode.Impulse);
        isShoot = true;

        Vector3 finalPos = mainCamera.ScreenToViewportPoint(Input.mousePosition) /*- first*/;
        
        finalPos.x = Mathf.Lerp(-10f, 10f, finalPos.x);
        finalPos.y = 0;
        finalPos.z = 10;

        first.z = 10;
        first.y = 0;
        rb.AddForce(first * forceMultiplier, ForceMode.Impulse);
    }
}