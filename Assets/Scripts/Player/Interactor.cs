using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
	/*[HideInInspector]*/ public bool inReachOfTerminal = false;
	[HideInInspector] public Terminal TerminalInReach;

	[SerializeField] private Transform interactableCheck;
	[SerializeField] private LayerMask crateMask;
	[SerializeField] private LayerMask interactableMask;
	[SerializeField] private LayerMask terminalMask;
	[SerializeField] private CharacterMovement characterMovement;
	[SerializeField] private MouseLook mouseLook;
	[SerializeField] private GameObject hud;
	[SerializeField] private GameObject interactInfoText;

	private Camera playerCam;
	private Terminal terminal;
	private bool interacting = false;
	protected int numFound;
	protected readonly Collider[] playerColliders = new Collider[1];

	private void Start()
	{
		playerCam = Camera.main;
	}

	void Update()
    {
		if (Input.GetKeyUp(KeyCode.E) && interacting)
		{
			LockPlayerMovement(false);
			hud.SetActive(true);
			interacting = false;
			if(terminal != null)
			{
				terminal.SwitchOff();
				terminal = null;
			}
		}
		else
		{
			CheckForInteractables();
		}
	}

	protected void CheckForInteractables()
	{
		numFound = Physics.OverlapSphereNonAlloc(interactableCheck.position, 0.3f,
					playerColliders, interactableMask);

		if (numFound > 0 && !interacting)
		{
			Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
			Debug.DrawRay(ray.origin, ray.direction);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 1.5f, terminalMask))
			{
				terminal = hitInfo.transform.parent.GetComponent<Terminal>();

				if (terminal != null)
				{
					interactInfoText.SetActive(true);

					if (Input.GetKeyUp(KeyCode.E))
					{
						interacting = true;
						LockPlayerMovement(true);
						hud.SetActive(false);
						terminal.SwitchOn();
					}
				}
			}
			else
			{
				interactInfoText.SetActive(false);
			}
		}
		else
		{
			interactInfoText.SetActive(false);
		}
	}

	public void LockPlayerMovement(bool value)
	{
		characterMovement.LockMovement = value;
		mouseLook.LockMovement = value;
	}
}