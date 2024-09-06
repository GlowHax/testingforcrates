using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{
	private List<Item> allItems;
	private Dictionary<string, Item> allItemsDictionary;

	protected override void Awake()
	{
		base.Awake();
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        LoadResources();
    }

	private void OnDestroy()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	}

	private void GameManager_OnGameStateChanged(GameState state)
	{
		if(state == GameState.Starting)
		{
			LoadResources();
		}
	}

	private void LoadResources()
	{
		allItems = Resources.LoadAll<Item>("Items").ToList();
		allItemsDictionary = allItems.ToDictionary(r =>  r.Name, r => r);
	}

	public Item GetItem(string itemName)
	{
		//find item
		Item item = allItemsDictionary[itemName];
		//exit if no item was found
		if (item == null)
		{
			Debug.Log($"Could not find item '{itemName}' in Dictionary");
			return null;
		}
		else
		{
			return item;
		}
	}

	public List<T> GetAllItemsOfType<T>() where T : Item
	{
		List<T> itemsFound = new List<T>();
		foreach (string name in allItemsDictionary.Keys)
		{
			if (allItemsDictionary[name] is T)
			{
				itemsFound.Add(allItemsDictionary[name] as T);
			}
		}
		return itemsFound;
	}
}
