using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction crouchAction;
    private InputAction jumpAction;


    public MovementType MovementType;

    [Header("Crouch Camera")]
    public float standingCamHeight = 1.6f;
    public float crouchingCamHeight = 1.0f;
    public float crouchLerpSpeed = 10f;

    public float normalSpeed = 5f;
    public float moveSpeed = 5f;

    [Range(0.01f, 0.14f)]
    public float mouseSensitivity = 0.012f;
    public Transform cameraHolder;

    private CharacterController controller;
    private float xRotation = 0f;

    public float gravity = -25f; // stronger gravity = less floaty
    public float verticalVelocity = 0f;
    public float terminalVelocity = -53f;

    [Header("Jump Settings")]
    public float jumpHeight = 0.5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchSpeed = 2f;

    private bool isCrouching = false;

    void OnEnable()
    {
        var actionMap = inputActions.FindActionMap("Player"); // your map name
        moveAction = actionMap.FindAction("Move"); // your action name
        lookAction = actionMap.FindAction("Look");
        crouchAction = actionMap.FindAction("crouch");
        jumpAction = actionMap.FindAction("Jump");
        moveAction.Enable();
        lookAction.Enable();
        crouchAction.Enable();
        jumpAction.Enable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleLook();
        HandleCrouch();
        HandleMovement();
    }

    void HandleMovement()
    {



        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        var move = transform.right * moveX + transform.forward * moveZ;


        MovementType = moveSpeed == crouchSpeed
         ? MovementType.Croaching
         : move == Vector3.zero 
         ? MovementType.StandingStill
         : MovementType.Running;

        Debug.Log("current movementype: " + MovementType);

        var isGrounded = controller.isGrounded;

        if (isGrounded && jumpAction.IsPressed())
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, terminalVelocity);
        }

        move.y = verticalVelocity;

        if (move == Vector3.zero)
            MovementType = MovementType.StandingStill;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // ⬇️ Prevent sticky ceiling effect
        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && verticalVelocity > 0)
        {
            verticalVelocity = 0;
        }
    }


    void HandleCrouch()
    {
        bool isCrouching = crouchAction.IsPressed();

        Debug.Log("iscrouch?" + isCrouching);

        controller.height = isCrouching ? crouchHeight : standingHeight;
        controller.center = new Vector3(0, controller.height / 2f - controller.skinWidth, 0);

        // Adjust movement speed
        moveSpeed = isCrouching ? crouchSpeed : normalSpeed;

        // Smooth camera position
        float targetHeight = isCrouching ? crouchingCamHeight : standingCamHeight;
        Vector3 camPos = cameraHolder.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetHeight, Time.deltaTime * crouchLerpSpeed);
        cameraHolder.localPosition = camPos;
    }



    void HandleLook()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
