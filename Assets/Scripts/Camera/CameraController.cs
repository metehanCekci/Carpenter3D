using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform arms;
    [SerializeField] float smoothTime = 0.1f; // Smooth hareketin süresi
    [SerializeField] float tiltAmount = 10f; // Tilt angle in degrees
    [SerializeField] float tiltSpeed = 5f; // Speed of tilting
    [SerializeField] float resetSpeed = 2f; // Speed of resetting the tilt
    [SerializeField] float dashFOV = 120f; // Dashing sırasında FOV
    [SerializeField] float normalFOV = 85f; // Normal FOV
    [SerializeField] float fovChangeSpeed = 1f; // FOV değişim hızı
    public PlayerMovement pm;
    private float zTilt = 0f;
    private float xRotation = 0f;
    private Vector2 lookInput;
    private Vector2 moveInput;

    private PlayerInputActions inputActions;
    private Vector2 currentMouseDelta;
    private Camera cam;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        cam = Camera.main;
    }

    void OnEnable()
    {
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Enable();

        // Mouse imlecini kilitle ve görünmez yap
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();

        // Mouse imlecini serbest bırak ve görünür yap
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandleRotationAndTilt();
        HandleFOVChange();
    }

    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void HandleRotationAndTilt()
    {
        // Mouse input for rotation
        Vector2 targetMouseDelta = lookInput * mouseSensitivity * Time.deltaTime;
        currentMouseDelta = Vector2.Lerp(currentMouseDelta, targetMouseDelta, smoothTime);
        float mouseX = currentMouseDelta.x;
        float mouseY = currentMouseDelta.y;

        // Calculate x rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Check for tilt input (A and D keys or gamepad)
        float tiltDirection = 0f;
        if (moveInput.x < -0.1f)
        {
            tiltDirection = 1f;
        }
        else if (moveInput.x > 0.1f)
        {
            tiltDirection = -1f;
        }

        // Apply tilt if dashing
        if (pm.isDashing && tiltDirection != 0f)
        {
            zTilt = Mathf.Lerp(zTilt, tiltAmount * tiltDirection, Time.deltaTime * tiltSpeed);
        }
        else
        {
            zTilt = Mathf.Lerp(zTilt, 0f, Time.deltaTime * resetSpeed);
        }

        // Combine rotation and tilt
        transform.localRotation = Quaternion.Euler(xRotation, 0f, zTilt);
        playerBody.Rotate(Vector3.up * mouseX);

        // Arms rotation on Y axis based on mouseX
        arms.localRotation = Quaternion.Euler(mouseY, mouseY, mouseY);
    }

    void HandleFOVChange()
    {
        if (pm.isDashing && (moveInput.y > 0.1f || moveInput.y < -0.1f))
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, dashFOV,fovChangeSpeed);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV,fovChangeSpeed);
        }
    }
}
