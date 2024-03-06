using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : View
{
	[SerializeField] private GameObject menu;
	private bool showingMenu = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !Player.Instance.Inventory.InventoryMenu.activeSelf)
		{
			showingMenu = !showingMenu;
		}
	}

    public override void Initialize()
    {
        
    }

    public override void Show(Transform parent = null)
    {
        base.Show(parent);
        Player.Instance.FPMovement(false);
    }

    public override void Hide()
    {
        base.Hide();
        Player.Instance.FPMovement(true);
    }

	public void QuitGame()
	{
		Application.Quit();
	}
}
