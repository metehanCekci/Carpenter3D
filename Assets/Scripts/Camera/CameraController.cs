using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] 
    public Transform player;
    public float mouseSensivity = 3f;

    float cameraVerticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSensivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensivity;

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90, 90);
        transform.localEulerAngles=Vector3.right * cameraVerticalRotation;

        player.Rotate(Vector3.up * inputX);
    }

}