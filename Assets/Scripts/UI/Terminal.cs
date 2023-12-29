using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Terminal : MonoBehaviour
{
	[HideInInspector] public bool switchedOn = false;

	[SerializeField] protected GameObject mainPanel;
	[SerializeField] protected GameObject blackscreen;
	[SerializeField] protected Transform interactionPoint;

	protected Interactor interactor;
	protected int numFound;
	protected readonly Collider[] playerColliders = new Collider[1];
	protected bool playerIsLeaving = false;

	public virtual void SwitchOn()
	{
		mainPanel.SetActive(true);
		blackscreen.SetActive(false);
		switchedOn = true;
	}

	public virtual void SwitchOff()
	{
		blackscreen.SetActive(true);
		mainPanel.SetActive(false);
		switchedOn = false;
	}
}
