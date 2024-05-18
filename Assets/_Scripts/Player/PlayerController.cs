using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private InputReader input;

    [SerializeField] Player player;
    [SerializeField] private CharacterController controller;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private CharacterController characterController;

    private Inventory inventory;

	//Movement
	[SerializeField] private float gravity = -9.81f;
	[SerializeField] private float speed = 6f;
	[SerializeField] private float jumpHeight = 3f;
	[SerializeField] private float groundDistance = 0.2f;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private bool isMovementEnabled = true;
    private bool isGrounded;
    private float pushPower = 0.1f;

    //Mouse Look
    [SerializeField] Transform cam;
    [SerializeField] float mouseSensitivity = 1f;
    private float xRotation = 0f;
    private bool isLookingEnabled = true;

    protected void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        if (state == GameState.Pause)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (state == GameState.Running)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Start()
    {
        inventory = transform.GetComponent<Inventory>();

		input.MoveEvent += HandleMove;
        input.LookEvent += Look;

		input.JumpEvent += Jump;

        input.InventoryEvent += ToggleInventoryControls;
        input.InteractEvent += ToggleInteractionControls;
    }

    void Update()
    {
        GroundCheck();
        Move();
        ApplyGravity();
    }

    private void OnApplicationFocus(bool focused)
    {
        if (focused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		Vector3 colloisionPoint = hit.point;
		body.AddForceAtPosition(pushDir * (pushPower / body.mass), colloisionPoint, ForceMode.Impulse);
	}

    private void ToggleInventoryControls()
    {
        ToggleFPControls(true, !isLookingEnabled);
    }

    private void ToggleInteractionControls()
    {
        ToggleFPControls(!isMovementEnabled, !isLookingEnabled);
    }

    private void ToggleFPControls(bool movement, bool mouseLook)
    {
        isMovementEnabled = movement;
        isLookingEnabled = mouseLook;
    }

    private void HandleMove(Vector3 dir)
	{
		moveDirection = dir;
	}

    private void Look(Vector2 dir)
    {
        if (isLookingEnabled)
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            xRotation -= dir.y * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * dir.x * mouseSensitivity);
        }
        else
        {
            if(Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void Move()
	{
		if(isMovementEnabled)
        {
            Vector3 move = transform.right * moveDirection.x + transform.forward * moveDirection.z;

            controller.Move(move * speed * Time.deltaTime);

            controller.Move(velocity * Time.deltaTime);
        }
	}

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void Jump()
    {
        if (isMovementEnabled && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
	
	private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
}