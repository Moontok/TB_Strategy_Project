using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    const float MIN_FOLLOW_Y_OFFSET = 2f;
    const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] CinemachineVirtualCamera virtualCamera = null;

    CinemachineTransposer transposer = null;
    Vector3 targetFollowOffset = Vector3.zero;

    void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = transposer.m_FollowOffset;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomSpeed;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomSpeed;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

    void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    void HandleMovement()
    {
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}
