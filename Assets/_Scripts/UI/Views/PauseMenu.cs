using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : View
{
	[SerializeField] private GameObject menu;
	[SerializeField] private InputReader input;

    public override void Initialize()
    {
        
    }

	public void Resume()
	{
		GameManager.Instance.ResumeGame();
		input.SetGameplay();
	}

	public void Quit()
	{
		Application.Quit();
	}
}
