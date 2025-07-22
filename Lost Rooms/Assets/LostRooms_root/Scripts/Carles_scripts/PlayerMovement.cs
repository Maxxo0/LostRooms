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

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    private float standHeight;
    private Vector3 standCenter;
    private Vector3 cameraStandLocalPos;
    private bool isCrouching = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        standHeight = controller.height;
        standCenter = controller.center;
        cameraStandLocalPos = playerCamera.localPosition;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleCrouch();
        HandleMovement();
        ApplyGravity();

        if (Input.GetButtonDown("Jump") && controller.isGrounded && !isCrouching)
        {
            Jump();
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && controller.isGrounded)
        {
            if (isCrouching)
                Stand();
            else
                Crouch();
        }
    }

    void Crouch()
    {
        controller.height = crouchHeight;
        controller.center = new Vector3(standCenter.x, crouchHeight / 2f, standCenter.z);
        playerCamera.localPosition = new Vector3(
            cameraStandLocalPos.x,
            cameraStandLocalPos.y - (standHeight - crouchHeight) / 2f,
            cameraStandLocalPos.z
        );
        isCrouching = true;
    }

    void Stand()
    {
        controller.height = standHeight;
        controller.center = standCenter;
        playerCamera.localPosition = cameraStandLocalPos;
        isCrouching = false;
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        float speed = moveSpeed * (isCrouching ? crouchSpeedFactor : 1f);
        controller.Move(move * speed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
