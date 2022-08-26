#define USE_NEW_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    PlayerInputActions playerInputActions = null;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one Input Manager! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        Vector2 inputMoveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            inputMoveDir.y = 1f;
        else if (Input.GetKey(KeyCode.S))
            inputMoveDir.y = -1f;

        if (Input.GetKey(KeyCode.D))
            inputMoveDir.x = 1f;
        else if (Input.GetKey(KeyCode.A))
            inputMoveDir.x = -1f;

        return inputMoveDir;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        if (Input.GetKey(KeyCode.Q))
            return 1f;
        else if (Input.GetKey(KeyCode.E))
            return -1f;
        else
            return 0f;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        if (Input.mouseScrollDelta.y > 0)
            return -1f;
        else if (Input.mouseScrollDelta.y < 0)
            return 1f;
        else
            return 0f;
#endif
    }
}
