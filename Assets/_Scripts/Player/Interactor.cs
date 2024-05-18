using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
	[SerializeField] private InputReader input;
	[SerializeField] private Transform interactableCheck;
	[SerializeField] private LayerMask interactableMask;
	[SerializeField] private GameObject hud;
	[SerializeField] private GameObject interactInfoText;

	[HideInInspector] public bool inReachOfTerminal = false;
	[HideInInspector] public Terminal TerminalInReach;

	private IInteractable interactable;
	private bool canInteract = false;
	private bool isInteracting = false;
	private readonly Collider[] colliders = new Collider[1];

    private void Start()
    {
		input.InteractEvent += HandleInteraction;
    }

    private void Update()
    {
		if (IsInteractableInReach())
		{
            if(IsPlayerFocusedOnInteractable())
            {
                canInteract = true;
                interactInfoText.SetActive(true);
            }
            else
            {
                canInteract = false;
                interactInfoText.SetActive(false);
            }
        }
        else
        {
            canInteract = false;
            interactInfoText.SetActive(false);
        }
    }

    private void HandleInteraction()
    {
        if (canInteract && !isInteracting)
        {
            isInteracting = true;
            hud.SetActive(false);
            interactable.Interact();
        }
        else if(isInteracting)
        {
            hud.SetActive(true);
            if (interactable != null)
            {
                interactable.EndInteraction();
                interactable = null;
            }
            isInteracting = false;
        }
    }

	private bool IsInteractableInReach()
	{
        bool isInReach = false;
        int numFound = Physics.OverlapSphereNonAlloc(transform.position, 0.25f, colliders, interactableMask);

        if (numFound > 0)
        {
            var iteractionPoint = colliders[0];
            if (iteractionPoint != null)
            {
                isInReach = true;
            }
        }

		return isInReach;
    }

	private bool IsPlayerFocusedOnInteractable()
	{
        bool isPlayerFocused = false;
        Ray ray = new Ray(Player.Instance.Cam.transform.position, Player.Instance.Cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1.5f, LayerMask.GetMask("Interactable")))
        {
            interactable = hitInfo.transform.parent.GetComponent<IInteractable>();

            if (interactable != null)
            {
                isPlayerFocused = true;
            }
        }
        
        return isPlayerFocused;
    }
}