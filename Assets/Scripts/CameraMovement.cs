using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform targetBall;

    [SerializeField]
    private float followSpeed = 4f;

    private Vector3 offset;

    private Vector3 targetLookAt;

    private bool shouldRotate;
    private Quaternion startRotation;

    public float rotationSpeed = 5f;

    void Start()
    {
        offset = transform.position - targetBall.position;
        startRotation = transform.rotation;
    }

    void LateUpdate()
    {
        SmoothFollow();
    }

    private void SmoothFollow()
    {
        Vector3 newPosition;

        if (!shouldRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * 2f);

            newPosition = targetBall.position + offset;
        }
        else
        {
            newPosition = targetBall.position + (targetBall.position - (targetLookAt.normalized * 0.05f));
            newPosition.y = 18.17f;
            newPosition.z = targetBall.position.z - 10f;

            Vector3 lookDirection = targetLookAt - transform.position;
            lookDirection.Normalize();

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
        }

        Vector3 smoothFollow = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        transform.position = smoothFollow;
    }

    public void UpdateTargetObject(Transform target)
    {
        targetBall = target;
    }

    public void LookAtObject(Vector3 target)
    {
        if (target == null)
            return;

        shouldRotate = true;
        targetLookAt = target;
    }

    public void CanRotate(bool canRotate)
    {
        shouldRotate = canRotate;
    }
}