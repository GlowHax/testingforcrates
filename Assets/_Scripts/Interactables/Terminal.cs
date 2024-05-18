using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Terminal : MonoBehaviour, IInteractable
{
	[SerializeField] protected GameObject mainPanel;
	[SerializeField] protected GameObject blackscreen;
	[SerializeField] protected Transform interactionPoint;

	protected Interactor interactor;
	protected bool isOn = false;

	public void Interact()
	{
		SwitchOn();
	}

    public void EndInteraction()
    {
        SwitchOff();
    }

    public virtual void SwitchOn()
	{
		mainPanel.SetActive(true);
		blackscreen.SetActive(false);
		isOn = true;
	}

	public virtual void SwitchOff()
	{
		blackscreen.SetActive(true);
		mainPanel.SetActive(false);
		isOn = false;
	}
}
