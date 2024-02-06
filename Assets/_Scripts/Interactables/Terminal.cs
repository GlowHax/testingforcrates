using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Terminal : MonoBehaviour, IInteractable
{
	[HideInInspector] public bool switchedOn = false;

	[SerializeField] protected GameObject mainPanel;
	[SerializeField] protected GameObject blackscreen;
	[SerializeField] protected Transform interactionPoint;

	protected Interactor interactor;

	public void Interact()
	{
		if(!switchedOn)
		{
			SwitchOn();
		}
		else
		{
			SwitchOff();
		}
	}

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
