using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : View
{
	[SerializeField] private GameObject menu;

    public override void Initialize()
    {
        
    }

	public void QuitGame()
	{
		Application.Quit();
	}
}
