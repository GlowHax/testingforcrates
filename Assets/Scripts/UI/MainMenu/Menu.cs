using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : UIManager
{
	public void Resume()
	{
		gameObject.SetActive(false);
	}

	public void Settings()
	{

	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
