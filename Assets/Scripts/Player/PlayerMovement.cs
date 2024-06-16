using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float crouchSpeed = 5f;  // E�ilme s�ras�nda hareket h�z�
    [SerializeField] float slideSpeed = 15f;  // Kayma s�ras�nda hareket h�z�
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float dashForce = 20f;   // At�lma s�ras�nda uygulanan kuvvet
    [SerializeField] float dashCooldown = 1f; // At�lma i�in bekleme s�resi
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float crouchHeight = 0.7f;
    [SerializeField] float dashCrouchHeight = 1.2f; // Dash sırasında crouch yüksekliği
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
    [HideInInspector] public bool canDash = true;  // At�lma eylemini yapabilme durumu
    private bool isSlamming = false;
    private bool hasAirDashed = false;  // Yeni değişken
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
        if (!isDashing)
        {
            Move();
            ApplyGravity();
        }
    }

    void Move()
    {
        if (isDashing) return;  // Stop moving if dashing

        float speed = isSliding ? slideSpeed : (isCrouching ? crouchSpeed : movementSpeed);
        Vector3 move;

        if (isSliding)
        {
            // Blend the slide direction with the player input direction
            Vector3 inputDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
            move = (slideDirection + inputDirection * 0.5f).normalized; // Decrease the power of movement during slide
            slideDirection = move; // Update slide direction for smoother transition
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
                rigidBody.drag = slopeDrag; // Increase drag when not moving
            }
            else
            {
                rigidBody.drag = 0f; // Reset drag when moving
            }
        }
        else
        {
            rigidBody.drag = 0f; // Reset drag on flat ground
        }

        Vector3 targetVelocity = new Vector3(move.x * speed, rigidBody.velocity.y, move.z * speed);
        rigidBody.velocity = Vector3.ClampMagnitude(targetVelocity, speed); // Cap the speed to prevent excessive acceleration
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
                // Perform fast fall in the air
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
        if (context.performed && canDash && !isDashing && (isGrounded || !hasAirDashed))
        {
            StartCoroutine(Dash());
            if (!isGrounded) hasAirDashed = true;  // Havada dash yapıldığında işaretle
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

        // Change collider height for crouch effect
        capsuleCollider.height = dashCrouchHeight;

        // Disable gravity for the duration of the dash
        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero; // Reset velocity to ensure clean dash
        rigidBody.AddForce(dashDirection.normalized * dashForce, ForceMode.VelocityChange);

        yield return new WaitForSeconds(0.1f);  // Atılma süresi

        // Reset collider height after dash
        capsuleCollider.height = standingHeight;

        // Enable gravity again
        rigidBody.useGravity = true;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);  // Atılma için bekleme süresi
        canDash = true; // Atılma tekrar yapılabilir hale gelir
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
            hasAirDashed = false;  // Yere değdiğinde havada dash durumu sıfırlanır
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
