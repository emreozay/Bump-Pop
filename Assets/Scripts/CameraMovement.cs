using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform targetBall;

    [SerializeField]
    private float followSpeed = 4f;

    private Vector3 newPosition;
    private Vector3 offset;
    private Quaternion newRotation;

    private Vector3 targetLookAt;

    private bool shouldRotate;
    private bool isFinish;
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
        if (!shouldRotate)
        {
            Quaternion newRot = startRotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * 2f);

            newPosition = targetBall.position + offset;

            if (isFinish)
            {
                newPosition.x /= 2f;
                newPosition.z -= 5f;
            }
        }
        else
        {
            newPosition -= new Vector3(0, -15, 5);

            Vector3 lookDirection = targetLookAt - transform.position;
            lookDirection.Normalize();
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
            
        }

        Vector3 smoothFollow = Vector3.Lerp(transform.localPosition, newPosition, followSpeed * Time.deltaTime);
        transform.localPosition = smoothFollow;
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

    public void ChangeOffsetForFinish()
    {
        isFinish = true;
    }

    public void SetPositionAndRotationOfCamera(Vector3 cameraPosition, Quaternion cameraRotation)
    {
        shouldRotate = true;
        newPosition = cameraPosition;
        newRotation = cameraRotation;

        Vector3 v = newRotation.eulerAngles;
        newRotation = Quaternion.Euler(v.x + 45f, v.y, v.z);
    }
}