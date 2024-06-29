using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float crouchSpeed = 5f;  
    [SerializeField] float slideSpeed = 15f;  
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float slamForce = 30f;
    [SerializeField] float dashForce = 20f;  
    [SerializeField] float dashCooldown = 1f; 
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float crouchHeight = 0.7f;
    [SerializeField] float dashCrouchHeight = 1.2f; 
    [SerializeField] float standingHeight = 2f;
    [SerializeField] private float slopeForceRayLength = 1.5f;
    [SerializeField] private float slopeDrag = 5f;
    [SerializeField] private float fastFallMultiplier = 10f; 

    [SerializeField] private float duration;
    [SerializeField] private float magnitude;
    private bool iFrames = false;
    private bool parryCoolDown = false;

    [SerializeField] GameObject bloodEffectPrefab;
    [SerializeField] PauseMenuScript pauseScript;
    [HideInInspector] public bool parrySuccessful = false;
    [HideInInspector] public bool isDead = false;

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
    [HideInInspector] public bool canDash = true; 
    [HideInInspector] public bool isSlamming = false;
    private bool isWalking = false;
    public bool hasAirDashed = false;  
    private CapsuleCollider capsuleCollider;
    private Camera mainCamera;
    private CameraShake cameraShake; 

    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public float attackDamage = 10;
    public LayerMask attacklayer;

    bool attacking = false;
    bool readyToAttack = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: To keep the singleton across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        inputActions = new PlayerInputActions();
        mainCamera = Camera.main;
        cameraShake = mainCamera.GetComponent<CameraShake>(); 
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
        inputActions.Player.Parry.performed += OnParry;
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
        inputActions.Player.Parry.performed -= OnParry;
        inputActions.Player.Disable();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true; 
        rigidBody.useGravity = true; 
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        if(iFrames) Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("HitBox"),true);
        else 
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("HitBox"),false);                  
        }
        if (!isDashing)
        {
            Move();
            ApplyGravity();
        }
    }

    void Move()
    {
        if (isDashing && !isDead && !pauseScript.isPaused) return; 

        float speed = isSliding ? slideSpeed : (isCrouching ? crouchSpeed : movementSpeed);
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (OnSlope())
        {
            move = Vector3.ProjectOnPlane(move, GetSlopeNormal());
            if (moveInput == Vector2.zero)
            {
                rigidBody.drag = slopeDrag; 
            }
            else
            {
                rigidBody.drag = 0f; 
            }
        }

        if (isSliding)
        {
            targetVelocity = new Vector3(slideDirection.x * speed, rigidBody.velocity.y, slideDirection.z * speed);
            rigidBody.velocity = Vector3.ClampMagnitude(targetVelocity, speed); 
        }
        else
        {
            targetVelocity = new Vector3(move.x * speed, rigidBody.velocity.y, move.z * speed);
            rigidBody.velocity = targetVelocity;
        }
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        if(!parryCoolDown)
        StartCoroutine(parry());
    }

    IEnumerator parry()
    {
        transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Animator>().SetTrigger("Parry");
        parrySuccessful = true;
        yield return new WaitForSeconds(0.1f);
        if(!parrySuccessful);
        else
        {
        parrySuccessful = false;
        parryCoolDown = true;
        yield return new WaitForSeconds(1);
        parryCoolDown = false;
        }

    }

    void ApplyGravity()
    {
        rigidBody.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    public void startIFrames()
    {
        StartCoroutine(IFrames());
    }

    IEnumerator IFrames()
    {
        iFrames = true;
        yield return new WaitForSeconds(0.3f);
        iFrames = false;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            isWalking = true;
            if (isGrounded)
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            else
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }
        else if (context.canceled)
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
        if (context.performed && !isDead && !pauseScript.isPaused)
        {
            if (isGrounded)
            {
                transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                SfxScript.Instance.playJump();
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z); 
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = true;
            }
            else if (ReadJson.Instance.config.hasDoubleJump)
            {
                if (canDJump)
                {
                    SfxScript.Instance.playJump();
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z); 
                    rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    isJumping = true;
                    canDJump = false;
                }
            }
        }
    }

    public void SlamImpact()
    {
        SfxScript.Instance.playSlam();
        isSlamming = false;
        Debug.Log("Slam Impact!");
    }

    void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed && !isDead && !pauseScript.isPaused)
        {
            if (isGrounded && moveInput != Vector2.zero)
            {
                isSliding = true;
                slideDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized ; // Use the player's forward direction
                capsuleCollider.height = crouchHeight;
            }
            else if (!isGrounded)
            {
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
        if (context.performed && canDash && !isDashing && !isDead && !pauseScript.isPaused && (isGrounded || !hasAirDashed))
        {
            StartCoroutine(Dash());
            if (!isGrounded) hasAirDashed = true; 
        }
    }

    void OnFire(InputAction.CallbackContext context)
    {
        if (!readyToAttack || attacking || isDead || pauseScript.isPaused) return;

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
            SfxScript.Instance.gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            SfxScript.Instance.playHit();
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
        this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().speed = 1;
        SfxScript.Instance.gameObject.GetComponent<AudioSource>().pitch = 1;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        SfxScript.Instance.playDash();
        Vector3 dashDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward; 
        }


        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero; 
        rigidBody.AddForce(dashDirection.normalized * dashForce, ForceMode.VelocityChange);
        StartCoroutine(IFrames());
        yield return new WaitForSeconds(0.1f); 

        rigidBody.useGravity = true;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown); 
        canDash = true; 

         

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
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isSlamming)
            {
                SlamImpact();
            }
            if (isWalking) transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            isGrounded = true;
            hasAirDashed = false; 
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
