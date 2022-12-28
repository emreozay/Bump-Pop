using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform targetBall;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - targetBall.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = targetBall.position + offset;
        //newPosition.x = 0;

        transform.position = newPosition;
    }

    public void UpdateTargetObject(Transform target)
    {
        targetBall = target;
    }
}