using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
	[HideInInspector] public bool inReachOfTerminal = false;
	[HideInInspector] public Terminal TerminalInReach;

	[SerializeField] private Transform interactableCheck;
	[SerializeField] private LayerMask interactableMask;
	[SerializeField] private GameObject hud;
	[SerializeField] private GameObject interactInfoText;

	private IInteractable interactable;
	private bool interacting = false;
	private readonly Collider[] colliders = new Collider[1];

	void Update()
    {
		if (!interacting)
		{
			CheckForInteractables();
		}
		else if (Input.GetKeyUp(KeyCode.E))
		{
			Player.Instance.FPMovement(true);
			Player.Instance.FPMouse(true);
			hud.SetActive(true);
			interacting = false;
			if(interactable != null)
			{
				interactable.Interact();
				interactable = null;
			}
		}
	}

	private void CheckForInteractables()
	{
		int numFound = Physics.OverlapSphereNonAlloc(transform.position, 0.25f, colliders, interactableMask);
		
		if (numFound > 0)
		{
			var iteractionPoint = colliders[0];
			if(iteractionPoint != null)
			{
				Ray ray = new Ray(Player.Instance.Cam.transform.position, Player.Instance.Cam.transform.forward);
				Debug.DrawRay(ray.origin, ray.direction);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, 1.5f, LayerMask.GetMask("Interactable")))
				{
					if (hitInfo.collider.isTrigger)
						return;

					interactable = hitInfo.transform.parent.GetComponent<IInteractable>();

					if (interactable != null)
					{
						interactInfoText.SetActive(true);

						if (Input.GetKeyUp(KeyCode.E))
						{
							interacting = true;
							Player.Instance.FPMovement(false);
							Player.Instance.FPMouse(false);
							hud.SetActive(false);
							interactable.Interact();
						}
					}
				}
				else
				{
					interactInfoText.SetActive(false);
				}
			}
		}
		else
		{
			interactInfoText.SetActive(false);
		}
	}
}