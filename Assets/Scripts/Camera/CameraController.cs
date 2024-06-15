using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;
    [SerializeField] float smoothTime = 0.1f; // Smooth hareketin s�resi

    private Vector2 lookInput;
    private float xRotation = 0f;
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
        RotateCamera();
    }

    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

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
}
