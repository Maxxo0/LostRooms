using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Cámara")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Agacharse")]
    public float crouchHeight = 1f;
    public float crouchSpeedFactor = 0.5f;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private float standHeight;
    private Vector3 cameraStandLocalPos;
    private bool isCrouching;

    private float xRotation;
    private bool isGrounded;
    public LayerMask groundMask;
    public float groundCheckDistance = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        standHeight = capsule.height;
        cameraStandLocalPos = playerCamera.localPosition;
        rb.freezeRotation = true;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleCrouch();
        HandleJump();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            if (isCrouching)
            {
                if (CanStandUp())
                    Stand();
                else
                    Debug.Log("¡No puedes levantarte, hay algo encima!");
            }
            else
            {
                Crouch();
            }
        }
    }

    bool CanStandUp()
    {
        float headCheckDistance = (standHeight - crouchHeight);
        Vector3 origin = transform.position + Vector3.up * (crouchHeight / 2f);
        return !Physics.Raycast(origin, Vector3.up, headCheckDistance);
    }

    void Crouch()
    {
        capsule.height = crouchHeight;
        capsule.center = new Vector3(0, crouchHeight / 2f, 0);
        playerCamera.localPosition = cameraStandLocalPos - Vector3.up * ((standHeight - crouchHeight) / 2f);
        isCrouching = true;
    }

    void Stand()
    {
        capsule.height = standHeight;
        capsule.center = new Vector3(0, standHeight / 2f, 0);
        playerCamera.localPosition = cameraStandLocalPos;
        isCrouching = false;
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        float speed = moveSpeed * (isCrouching ? crouchSpeedFactor : 1f);
        Vector3 moveVelocity = move * speed;

        Vector3 velocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
        rb.linearVelocity = velocity;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            Debug.Log("Saltando!");
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (capsule.height / 2f), groundCheckDistance, groundMask);
    }

    void OnDrawGizmosSelected()
    {
        if (capsule != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * (capsule.height / 2f), groundCheckDistance);
        }
    }

}
