using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Inventory
{
	public List<CrateSO> Crates = new List<CrateSO>();
	public List<MaterialSO> Materials = new List<MaterialSO>();
	public List<ScrapSO> Scraps = new List<ScrapSO>();
	public List<Item> Items = new List<Item>();
}

public class User : MonoBehaviour
{
	private static User instance;
	public static User Instance { get { return instance; } }

	public float Coins = 500;
	public int Level = 1;
	public int XP = 0;
	public Inventory Inventory = new Inventory();
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
	}

	private void ManageEquippedItem()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (equippedItem == null)
			{
				//equip the crowbar
				equippedItem = Inventory.Items[0];
				equippedItem.Equipped = true;
				equippedItem.gameObject.SetActive(true);
			}
			else
			{
				//unequip crowbar
				equippedItem.Equipped = false;
				equippedItem.gameObject.SetActive(false);
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
