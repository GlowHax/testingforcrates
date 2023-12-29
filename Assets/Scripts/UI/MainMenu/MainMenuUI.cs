using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : UIManager
{
	[SerializeField] GameObject menu1;
	[SerializeField] GameObject menu2;

	private void Start()
	{
		RefreshCoinsHUD();
	}

	private void Update()
	{
		ManageLevelHUD();
	}

	public void SwitchToMenu(int menuID)
	{
		menu1.SetActive(false);
		menu2.SetActive(false);

		switch (menuID)
		{
			case 1:
				menu1.SetActive(true);
				break;
			case 2:
				menu2.SetActive(true);
				break;
		}
	}
}