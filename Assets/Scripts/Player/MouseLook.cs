using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public bool LockMovement;

	[SerializeField] Transform playerBody;
	[SerializeField] float mouseSensitivity = 1f;
	float xRotation = 0f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnApplicationFocus(bool focused)
	{
		if (focused && !LockMovement)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void Update()
	{
		if(!LockMovement)
		{
			if(Cursor.lockState == CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}

			float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
			float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			playerBody.Rotate(Vector3.up * mouseX);
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
