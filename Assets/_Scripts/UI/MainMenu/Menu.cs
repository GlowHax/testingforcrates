using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] private GameObject menu;
	private bool showingMenu = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !Player.Instance.Inventory.InventoryMenu.activeSelf)
		{
			showingMenu = !showingMenu;
			ShowMenu(showingMenu);
		}
	}

	public void ShowMenu(bool value)
	{
		Player.Instance.FPMovement(!value);
		menu.SetActive(value);
	}

	public void Settings()
	{

	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
