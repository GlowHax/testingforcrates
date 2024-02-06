using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	[HideInInspector] public bool Active = true;

	[SerializeField] Player player;
	[SerializeField] Transform cam;
	[SerializeField] float mouseSensitivity = 1f;
	float xRotation = 0f;

	private void OnApplicationFocus(bool focused)
	{
		if (focused && Active)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void Update()
	{
		if(Active)
		{
			if(Cursor.lockState != CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}

			float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
			float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			transform.Rotate(Vector3.up * mouseX);
		}
		else if(Cursor.lockState != CursorLockMode.None)
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
