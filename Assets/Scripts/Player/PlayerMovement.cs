using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float crouchSpeed = 5f;  // E�ilme s�ras�nda hareket h�z�
    [SerializeField] float slideSpeed = 15f;  // Kayma s�ras�nda hareket h�z�
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float slamForce = 30f;
    [SerializeField] float dashForce = 20f;   // At�lma s�ras�nda uygulanan kuvvet
    [SerializeField] float dashCooldown = 1f; // At�lma i�in bekleme s�resi
    [SerializeField]  float gravity = -9.81f;
    [SerializeField] float crouchHeight = 0.7f;
    [SerializeField] float dashCrouchHeight = 1.2f; // Dash sırasında crouch yüksekliği
    [SerializeField] float standingHeight = 2f;
    [SerializeField] private float slopeForceRayLength = 1.5f;
    [SerializeField] private float slopeDrag = 5f;
    [SerializeField] private float fastFallMultiplier = 10f; // Havada h�zl� ini� i�in ek kuvvet �arpan�

    [SerializeField] private float duration;
    [SerializeField] private float magnitude;

    [SerializeField] GameObject bloodEffectPrefab;

    private Vector2 moveInput;
    private Vector3 slideDirection;
    private Vector3 targetVelocity;
    private Rigidbody rigidBody;
    private PlayerInputActions inputActions;
    [HideInInspector] public bool isGrounded;
    private bool isJumping;
    private bool canDJump = true;
    private bool isCrouching;
    private bool isSliding;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool canDash = true;  // At�lma eylemini yapabilme durumu
    [HideInInspector] public bool isSlamming = false;
    private bool isWalking = false;
    public bool hasAirDashed = false;  // Yeni değişken
    private CapsuleCollider capsuleCollider;
    private Camera mainCamera;
    private CameraShake cameraShake; // Reference to the CameraShake script

    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public float attackDamage = 10;
    public LayerMask attacklayer;

    bool attacking = false;
    bool readyToAttack = true;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        mainCamera = Camera.main;
        cameraShake = mainCamera.GetComponent<CameraShake>(); // Initialize the camera shake reference
    }

    void OnEnable()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Crouch.performed += OnCrouch;
        inputActions.Player.Crouch.canceled += OnCrouchCanceled;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Fire.performed += OnFire;
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
        inputActions.Player.Fire.performed -= OnFire;
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

        move = transform.right * moveInput.x + transform.forward * moveInput.y;

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
        if (!isSliding)
        {
            targetVelocity = new Vector3(move.x * speed, rigidBody.velocity.y, move.z * speed);

            //Sınırlamayı (clamp) kaldırarak daha hızlı düşüş sağlanıyor
            rigidBody.velocity = targetVelocity; 
            
            //rigidBody.velocity = Vector3.ClampMagnitude(targetVelocity, speed);  Cap the speed to prevent excessive acceleration
        }
        else
        {
            rigidBody.velocity = Vector3.ClampMagnitude(slideDirection, speed); // Cap the speed to prevent excessive acceleration
        }
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



            if (context.performed)
            {
                isWalking = true;
                if(isGrounded)
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", true);
                else transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            }
            else if(context.canceled)
            {
            transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            isWalking = false;
            }
            else if (isGrounded)
            {
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                isWalking = false;
            }
            else
            {
                isWalking = false;
            }


    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", false);
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

    public void SlamImpact()
    {
        // Implement effects here, like camera shake, damaging enemies, etc.
        SfxScript.Instance.playSlam();
        isSlamming = false;
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
                slideDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized * slideSpeed;
                capsuleCollider.height = crouchHeight;
            }
            else if (!isGrounded)
            {
                // Perform fast fall in the air
                isSlamming = true;
                rigidBody.AddForce(Vector3.down * fastFallMultiplier * slamForce, ForceMode.Impulse);
                Debug.Log(rigidBody.velocity.y);
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

    void OnFire(InputAction.CallbackContext context)
    {
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Attack");

        SfxScript.Instance.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        SfxScript.Instance.playAttack();
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, attackDistance, attacklayer))
        {
            HitTarget(hit, hit.point);
        }
        else if (Physics.Raycast(mainCamera.transform.position + new Vector3(0.5f, 0, 0), mainCamera.transform.forward, out RaycastHit hit2, attackDistance, attacklayer))
        {
            HitTarget(hit2, hit2.point);
        }
        else if (Physics.Raycast(mainCamera.transform.position + new Vector3(0.7f, 0, 0), mainCamera.transform.forward, out RaycastHit hit3, attackDistance, attacklayer))
        {
            HitTarget(hit3, hit3.point);
        }
        else if (Physics.Raycast(mainCamera.transform.position + new Vector3(-0.5f, 0, 0), mainCamera.transform.forward, out RaycastHit hit4, attackDistance, attacklayer))
        {
            HitTarget(hit4, hit4.point);
        }
        else if (Physics.Raycast(mainCamera.transform.position + new Vector3(-0.7f, 0, 0), mainCamera.transform.forward, out RaycastHit hit5, attackDistance, attacklayer))
        {
            HitTarget(hit5, hit5.point);
        }
    }

    void HitTarget(RaycastHit hit, Vector3 pos)
    {
        if (hit.collider.gameObject.CompareTag("Hitable"))
        {
            hit.transform.gameObject.GetComponent<EnemyHealthScript>().takeDamage(attackDamage);
            SfxScript.Instance.gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.9f,1.1f);
            SfxScript.Instance.playHit();
            //Time.timeScale = 0.05f;
            this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().speed = 0.01f;
            GameObject clone = Instantiate(bloodEffectPrefab);
            clone.SetActive(true);
            clone.transform.position = hit.transform.gameObject.transform.position;
            Destroy(clone, 2);
            Invoke("recoverTime", 0.20f);
        }
    }

    private void recoverTime()
    {
        //Time.timeScale = 1;
        this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().speed = 1;
        SfxScript.Instance.gameObject.GetComponent<AudioSource>().pitch = 1;
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
            }
            if (isWalking) transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            isGrounded = true;
            hasAirDashed = false;  // Yere değdiğinde havada dash durumu sıfırlanır
            canDJump = true;
            isJumping = false;
            SfxScript.Instance.playFall();
            rigidBody.velocity = Vector3.up * 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("StackingTrampoline"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("StackingTrampoline"))
        {
            isGrounded = true;
            canDJump = true;
            isJumping = false;
        }
    }
}
