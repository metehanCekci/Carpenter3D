using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;
    [SerializeField] float smoothTime = 0.1f; // Smooth hareketin s�resi
    [SerializeField] float tiltAmount = 10f; // Tilt angle in degrees
    [SerializeField] float tiltSpeed = 5f; // Speed of tilting
    [SerializeField] float resetSpeed = 2f; // Speed of resetting the tilt
    public PlayerMovement pm;
    private float zTilt = 0f;
    private float xRotation = 0f;
    private Vector2 lookInput;
    
    private PlayerInputActions inputActions;

    private Vector2 currentMouseDelta;
    void Awake()
    {
        inputActions = new PlayerInputActions();
        
    }

    void OnEnable()
    {
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
        inputActions.Player.Enable();

        // Mouse imlecini kilitle ve g�r�nmez yap
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
        inputActions.Player.Disable();

        // Mouse imlecini serbest b�rak ve g�r�n�r yap
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandleRotationAndTilt();
    }
    

    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
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
        if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < -0.1f)
        {
            tiltDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0.1f)
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
    }

/*
    void RotateCamera()
    {
        Vector2 targetMouseDelta = lookInput * mouseSensitivity * Time.deltaTime;

        // Ak�c� hareket i�in Lerp kullanarak fare hareketini yumu�at
        currentMouseDelta = Vector2.Lerp(currentMouseDelta, targetMouseDelta, smoothTime);

        float mouseX = currentMouseDelta.x;
        float mouseY = currentMouseDelta.y;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleTilt()
        {
            float tiltDirection = 0f;

            // Check for keyboard input (A and D keys)
            if (Input.GetKey(KeyCode.A))
            {
                tiltDirection = 1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                tiltDirection = -1f;
            }

            // Check for gamepad input (left stick horizontal)
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                tiltDirection = 1f;
            }
            else if (Input.GetAxis("Horizontal") > 0.1f)
            {
                tiltDirection = -1f;
            }

            // Apply tilt if dashing
            if (pm.isDashing && tiltDirection != 0f)
            {
                Quaternion targetRotation = originalRotation * Quaternion.Euler(0f, 0f, tiltAmount * tiltDirection);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
            }
            else
            {
                // Reset tilt to original rotation
                transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * resetSpeed);
            }
        }
        */
}
