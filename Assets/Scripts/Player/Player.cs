using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class OldInventory
{
	public List<CrateSO> Crates = new List<CrateSO>();
	public List<MaterialSO> Materials = new List<MaterialSO>();
	public List<ScrapSO> Scraps = new List<ScrapSO>();
	public List<Tool> Items = new List<Tool>();
}

public class Player : MonoBehaviour
{
	private static Player instance;
	public static Player Instance { get { return instance; } }

	public Camera Cam;
	public Interactor interactor;
	public CharacterMovement characterMovement;
	public MouseLook mouseLook;
	public float Coins = 500;
	public int Level = 1;
	public int XP = 0;
	public OldInventory OldInventory = new OldInventory();
	public Inventory Inventory;
	public List<GameObject> CratePrefabs = new List<GameObject>();

	public float ComboBonus = 0f;

	[SerializeField] private Item equippedItem;

	#region level reward vars
	public int[] XPToNextLvl = { 50, 100, 200, 500, 750, 1000, 1500, 2000, 3000 };
	public int[] LevelRewardsMoney = { 250, 500, 1000, 2000, 2500, 2000, 1000 };
	public string[] LevelRewardsCrates =
	{
			"NoCrate", "NoCrate", "Uncommon Crate", "NoCrate", "Common Crate",
			"Uncommon Crate", "NoCrate", "NoCrate", "Rare Crate"
	};
	#endregion

	private void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		ManageEquippedItem();

		if (equippedItem != null && Input.GetMouseButtonDown(0))
		{
			if (equippedItem.GetType() == typeof(Tool))
			{
				Tool equippedTool = equippedItem as Tool;
				equippedTool.Use();
			}
		}
	}

	public void FPMovement(bool value)
	{
		Player.Instance.characterMovement.Active = value;
	}

	public void FPMouse(bool value)
	{
		Player.Instance.mouseLook.Active = value;
	}

	private void ManageEquippedItem()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (equippedItem == null)
			{
				//equip the crowbar
				equippedItem = OldInventory.Items[0];
				//equippedItem.Equipped = true;
				//equippedItem.gameObject.SetActive(true);
			}
			else
			{
				//unequip crowbar
				//equippedItem.Equipped = false;
				//equippedItem.gameObject.SetActive(false);
				equippedItem = null;
			}
		}
	}

	public void AddXPToUser(int amount)
	{
		XP += amount;
		//Quests.XPQuestCounter += amount;

		while (XP >= XPToNextLvl[Level - 1])
		{
			XP -= XPToNextLvl[Level - 1];
			Level++;
			//ShowLevelRewardScreen(Level);
		}
	}

	public float GetLevelProgressPercentage()
	{
		return (float)XP / (float)XPToNextLvl[Level - 1];
	}
}
