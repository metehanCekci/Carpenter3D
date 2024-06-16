using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float crouchSpeed = 5f;  // E�ilme s�ras�nda hareket h�z�
    [SerializeField] float slideSpeed = 25f;  // Kayma s�ras�nda hareket h�z�
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float dashForce = 20f;   // At�lma s�ras�nda uygulanan kuvvet
    [SerializeField] float dashCooldown = 1f; // At�lma i�in bekleme s�resi
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float crouchHeight = 0.7f;
    [SerializeField] float standingHeight = 2f;
    [SerializeField] private float slopeForceRayLength = 1.5f;
    [SerializeField] private float slopeDrag = 5f;
    [SerializeField] private float fastFallMultiplier = 10f; // Havada h�zl� ini� i�in ek kuvvet �arpan�


    private Vector2 moveInput;
    private Vector3 slideDirection;
    private Rigidbody rigidBody;
    private PlayerInputActions inputActions;
    private bool isGrounded;
    private bool isJumping;
    private bool canDJump = true;
    private bool isCrouching;
    private bool isSliding;
    private bool isDashing;
    private bool canDash = true;  // At�lma eylemini yapabilme durumu
    private bool isSlamming = false;
    private CapsuleCollider capsuleCollider;
    private Camera mainCamera;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Crouch.performed += OnCrouch;
        inputActions.Player.Crouch.canceled += OnCrouchCanceled;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Crouch.performed -= OnCrouch;
        inputActions.Player.Crouch.canceled -= OnCrouchCanceled;
        inputActions.Player.Dash.performed -= OnDash;
        inputActions.Player.Disable();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;  // Rotation'u dondurarak stabiliteyi art�r
        rigidBody.useGravity = true; // Yer�ekimini kullan
        capsuleCollider = GetComponent<CapsuleCollider>();
    }



    void FixedUpdate()
    {
        Move();
        ApplyGravity();
        
    }

    void Move()
    {
        if (isDashing) return;  // E�er at�lma yap�l�yorsa hareket etmeyi durdur

        float speed = isSliding ? slideSpeed : (isCrouching ? crouchSpeed : movementSpeed);
        Vector3 move;

        if (isSliding)
        {
            move = slideDirection;
        }
        else
        {
            move = transform.right * moveInput.x + transform.forward * moveInput.y;
        }

        if (OnSlope())
        {
            move = Vector3.ProjectOnPlane(move, GetSlopeNormal());
            if (moveInput == Vector2.zero)
            {
                rigidBody.drag = slopeDrag; // Hareket yokken s�rt�nmeyi art�r
            }
            else
            {
                rigidBody.drag = 0f; // Hareket varken s�rt�nmeyi s�f�rla
            }
        }
        else
        {
            rigidBody.drag = 0f; // D�z zeminde s�rt�nmeyi s�f�rla
        }

        Vector3 targetVelocity = new Vector3(move.x * speed, rigidBody.velocity.y, move.z * speed);
        rigidBody.velocity = targetVelocity;
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rigidBody.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                SfxScript.Instance.playJump();
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z); // Y ekseni hızını sıfırla
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = true;
            }
            else if (ReadJson.Instance.config.hasDoubleJump) // buraya
            {
                if (canDJump)
                {
                    SfxScript.Instance.playJump();
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z); // Y ekseni hızını sıfırla
                    rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    isJumping = true;
                    canDJump = false;
                }
            }
        }

    }

    void SlamImpact()
    {
        // Implement effects here, like camera shake, damaging enemies, etc.
        Debug.Log("Slam Impact!");
    }

    void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded && moveInput != Vector2.zero)
            {
                isSliding = true;
                isCrouching = true;
                slideDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
                capsuleCollider.height = crouchHeight;
            }
            else if (!isGrounded)
            {
                // Havada h�zl� ini� yap
                isSlamming = true;
                rigidBody.AddForce(Vector3.down * fastFallMultiplier, ForceMode.Impulse);
            }
            else
            {
                isCrouching = true;
                capsuleCollider.height = crouchHeight;
            }
        }
    }

    void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        isCrouching = false;
        isSliding = false;
        capsuleCollider.height = standingHeight;
    }

    void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false; // Atılma yapılamaz hale getir

        SfxScript.Instance.playDash();
        Vector3 dashDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward; // Default direction if no input
        }

        StartCoroutine(TiltCamera(dashDirection)); // Kamera eğilme fonksiyonunu çağır

        rigidBody.AddForce(dashDirection.normalized * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f);  // Atılma süresi
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);  // Atılma için bekleme süresi
        canDash = true; // Atılma tekrar yapılabilir hale gelir
    }


    private IEnumerator TiltCamera(Vector3 dashDirection)
    {
        float tiltDuration = 0.15f; // Kamera eğilme süresi
        float tiltAngle = 10f; // Kamera eğilme açısı
        float originalFOV = mainCamera.fieldOfView;
        float targetFOV = originalFOV;

        Quaternion originalRotation = mainCamera.transform.localRotation;
        Quaternion targetRotation = originalRotation;

        // İleri veya geri dash
        if (dashDirection.z != 0 && dashDirection.x <=0)
        {
            targetFOV = dashDirection.z > 0 ? originalFOV + 10 : originalFOV - 10;
        }

        // Sola veya sağa dash
        if (dashDirection.x != 0 && dashDirection.z <=0)
        {
            targetRotation = Quaternion.Euler(mainCamera.transform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, -dashDirection.x * tiltAngle);
        }

        float time = 0f;
        while (time < tiltDuration)
        {
            if (dashDirection.z != 0)
            {
                mainCamera.fieldOfView = Mathf.Lerp(originalFOV, targetFOV, time / tiltDuration);
            }

            if (dashDirection.x != 0)
            {
                mainCamera.transform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, time / tiltDuration*1.8f);//metehan elleme animasyon daha seri olsun diye not : amo
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (dashDirection.z != 0)
        {
            mainCamera.fieldOfView = targetFOV;
        }

        if (dashDirection.x != 0)
        {
            mainCamera.transform.localRotation = targetRotation;
        }

        time = 0f;
        while (time < tiltDuration)
        {
            if (dashDirection.z != 0)
            {
                mainCamera.fieldOfView = Mathf.Lerp(targetFOV, originalFOV, time / tiltDuration);
            }

            if (dashDirection.x != 0)
            {
                mainCamera.transform.localRotation = Quaternion.Lerp(targetRotation, originalRotation, time / tiltDuration);
            }

            time += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = originalFOV;
        mainCamera.transform.localRotation = originalRotation;
    }





    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
        {
            return hit.normal != Vector3.up;
        }
        return false;
    }

    private Vector3 GetSlopeNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
        {
            return hit.normal;
        }
        return Vector3.up;
    }

    void OnCollisionEnter(Collision collision)
    {   
        /*
        Bitwise operation kullanımına bakılmalı, çok daha verimli
        ve hızlı !!!!!!!!!!!
        */
        if (collision.gameObject.CompareTag("Ground")) 
        {
            if (isSlamming)
            {
                SlamImpact();
                SfxScript.Instance.playSlam();
                isGrounded = true;
                isSlamming = false;
            }
            isGrounded = true;
            canDJump = true;
            isJumping = false;
            SfxScript.Instance.playFall();
            
            

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDJump = true;
            isJumping = false;
        }
    }
}
