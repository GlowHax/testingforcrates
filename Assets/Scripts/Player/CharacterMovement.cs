using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	public bool LockMovement = false;

	[SerializeField] private CharacterController controller;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private CharacterController characterController;

	[SerializeField] private float gravity = -9.81f;
	[SerializeField] private float speed = 12f;
	[SerializeField] private float jumpHeight = 3f;
	[SerializeField] private float groundDistance = 0.2f;

	private Vector3 velocity;
	private bool isGrounded;
	private float pushPower = 0.1f;

	void Update()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		if (!LockMovement)
		{
			ManageMovement();
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

	private void ManageMovement()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;

		controller.Move(move * speed * Time.deltaTime);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}

		velocity.y += gravity * Time.deltaTime;

		controller.Move(velocity * Time.deltaTime);
	}
}