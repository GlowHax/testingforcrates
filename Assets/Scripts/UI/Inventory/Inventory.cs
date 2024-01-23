using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [SerializeReference] public List<ItemSlotInfo> Items = new List<ItemSlotInfo>();
	public ItemPanel EquippedToolItemPanel;
	public Tool EquippedTool;

    [Space]
    [Header("Menu Components")]
    public GameObject InventoryMenu;
    public GameObject ItemPanel;
    public GameObject ItemPanelGrid;

	public Mouse Mouse;

	[Space]
	public int InventorySize = 25;

	[HideInInspector] public Dictionary<string, Item> AllItemsDictionary = 
		new Dictionary<string, Item>();

	[SerializeField] private Transform toolHolderTransform;

	[SerializeField] private GameObject droppedItemObject;
	[SerializeField] string[] assetBundleNames;

	private List<ItemPanel> existingPanels = new List<ItemPanel>();

	private void Start()
	{
		CreateEmptyInventory();
		FillAllItemsDictionary();

		//for testing
		AddItem("Wood", 40);
		AddItem("Stone", 30);
		AddItem("Crowbar", 2);
		AddItem("Iron", 20);
		AddItem("Common Crate", 2);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
        {
			ShowInventoryMenu(!InventoryMenu.activeSelf);
        }

		if(Input.GetKeyDown(KeyCode.Escape) && InventoryMenu.activeSelf)
		{
			ShowInventoryMenu(false);
		}

		if(Input.GetMouseButtonDown(0) && 
			Mouse.ItemSlot.Item != null && 
			InventoryMenu.activeSelf &&
			!EventSystem.current.IsPointerOverGameObject())
		{
			DropItem(Mouse.ItemSlot.Item.Name, Mouse.ItemSlot.Stacks);
		}
	}

	private void FillAllItemsDictionary()
	{
		//load items with their data from assetbundle
		List<Item> allItems = LoadAllItems();
		string itemsInDictionary = "Items in Dictionary: ";
		foreach (Item item in allItems)
		{
			if (!AllItemsDictionary.ContainsKey(item.Name))
			{
				AllItemsDictionary.Add(item.Name, item);
				itemsInDictionary += ", " + item.Name;
			}
			else
			{
				Debug.Log($"{item} already exists in Dictionary - " +
					$"shares name with {AllItemsDictionary[item.Name]}");
			}
		}
		itemsInDictionary += ".";
		Debug.Log(itemsInDictionary);
	}

	private void CreateEmptyInventory()
	{
		for (int i = 0; i < InventorySize; i++)
		{
			Items.Add(new ItemSlotInfo(null, 0));
		}
		//Mouse.ItemSlot = new ItemSlotInfo(null, 0);
        EquippedToolItemPanel.Inventory = this;
    }

    public void EquipTool(Tool tool)
	{
		tool.Prefab = Instantiate(tool.Prefab, toolHolderTransform);
		Player.Instance.equippedTool = tool;		
	}

	public ItemSlotInfo GetItemSlot(string itemName)
	{
        //find item to add
        Item item = null;
        AllItemsDictionary.TryGetValue(itemName, out item);
        //exit if no item was found
        if (item == null)
        {
            Debug.Log($"Could not find item '{itemName}' in Dictionary to get slot from Inventory");
            return null;
        }

        foreach (ItemSlotInfo slot in Items)
        {
            if (slot.Item != null)
            {
                if (slot.Item.Name == itemName)
                {
					return slot;
                }
            }
        }
		return null;
    }

	public int GetAmountInInventory(string itemName)
	{
		int amount = 0;

        //find item to add
        Item item = null;
        AllItemsDictionary.TryGetValue(itemName, out item);
        //exit if no item was found
        if (item == null)
        {
            Debug.Log($"Could not find Item '{itemName}' in Dictionary to get amount from Inventory");
            return 0;
        }

		foreach(ItemSlotInfo slot in Items)
		{
			if (slot.Item != null)
			{
				if(slot.Item.Name == itemName)
				{
					amount += slot.Stacks;
				}
			}
		}
		return amount;
    }

	/// <returns>Amount not possible to add</returns>
	public int AddItem(string itemName, int amount)
	{
		//find item to add
		Item item = null;
		AllItemsDictionary.TryGetValue(itemName, out item);
		//exit if no item was found
		if (item == null)
		{
			Debug.Log($"Could not find Item '{itemName}' in Dictionary to add to Inventory");
			return amount;
		}

		//check for open stack-capacities in existing slots
		foreach(ItemSlotInfo slot in Items)
		{
			if(slot.Item != null)
			{
				if(slot.Item.Name == item.Name)
				{
					if(amount > slot.Item.MaxInventoryStacks - slot.Stacks)
					{
						amount -= slot.Item.MaxInventoryStacks - slot.Stacks;
						slot.Stacks = slot.Item.MaxInventoryStacks;
					}
					else
					{
						slot.Stacks += amount;
						if (InventoryMenu.activeSelf)
						{
							RefreshInventory();
							return 0;
						}
					}
				}
			}
		}

		//fill empty slots with leftover items
		foreach(ItemSlotInfo slot in Items)
		{
			if(slot.Item == null)
			{
				if(amount > item.MaxInventoryStacks)
				{
					slot.Item = item;
					slot.Stacks = item.MaxInventoryStacks;
					amount -= item.MaxInventoryStacks;
				}
				else
				{
					slot.Item = item;
					slot.Stacks = amount;
					if (InventoryMenu.activeSelf)
					{
						RefreshInventory();
					}
					return 0;
				}
			}
		}

		//no inventory space at all
		Debug.Log($"No space in inventory ({item.Name})");
		if (InventoryMenu.activeSelf)
		{
			RefreshInventory();
		}
		return amount;
	}

	public void RemoveItem(string itemName, int amount)
	{
        //find item to add
        Item item = null;
        AllItemsDictionary.TryGetValue(itemName, out item);
        //exit if no item was found
        if (item == null)
        {
            Debug.Log($"Could not find Item '{itemName}' in Dictionary to remove from Inventory");
        }

        foreach (ItemSlotInfo slot in Items)
        {
            if (slot.Item != null)
            {
                if (slot.Item.Name == item.Name)
                {
                    if (amount > slot.Stacks)
					{
						amount -= slot.Stacks;
						ClearSlot(slot);
                    }
					else
					{
						slot.Stacks -= amount;
					}
                }
            }
        }
    }

	public void DropItem(string itemName, int amount)
	{
		Item item = null;
		//find Item to drop
		AllItemsDictionary.TryGetValue(itemName, out item);

		//exit if no item was found
		if (item == null)
		{
			Debug.Log($"Could not find Item '{itemName}' in Dictionary to drop");
			return;
		}

		GameObject droppedItem = Instantiate(droppedItemObject,
			Player.Instance.transform.position + new Vector3(0, 0.5f, 0) + Player.Instance.Cam.transform.forward,
			Quaternion.identity);

		ItemPickup itemPickup = droppedItem.GetComponentInChildren<ItemPickup>();
		if (itemPickup != null)
		{
			itemPickup.itemToDrop = itemName;
			itemPickup.amount = amount;
			itemPickup.displayImage.sprite = item.Sprite;
			if(amount > 1)
			{
				itemPickup.ItemStacksText.text = amount.ToString();
			}
			else
			{
				itemPickup.ItemStacksText.gameObject.SetActive(false);
			}
		}

		Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.velocity = Player.Instance.Cam.transform.forward * 3f;
		}

		ClearSlot(Mouse.ItemSlot);
		RefreshInventory();
	}

	public void ClearSlot(ItemSlotInfo slot)
	{
		slot.Item = null;
		slot.Stacks = 0;
	}

	public void RefreshInventory()
	{
		existingPanels = ItemPanelGrid.GetComponentsInChildren<ItemPanel>().ToList();

		//create panels if needed
		if(existingPanels.Count < InventorySize)
		{
			int amountToCreate = InventorySize - existingPanels.Count;
			for (int i = 0; i < amountToCreate; i++)
			{
				GameObject newPanel = Instantiate(ItemPanel, ItemPanelGrid.transform);
				existingPanels.Add(newPanel.GetComponent<ItemPanel>());
			}
		}

		int index = 0;
		foreach(ItemSlotInfo slot in Items)
		{
			//name list elements 
			slot.Name = (index + 1).ToString();
			if(slot.Item != null)
			{
				slot.Name += ": " + slot.Item.Name;
			}
			else
			{
				slot.Name += ": -";
			}

			//update panels
			ItemPanel panel = existingPanels[index];
			if(panel != null)
			{
				panel.name = slot.Name + " Panel";
				panel.Inventory = this;
				panel.ItemSlot = slot;
				if (slot.Item != null)
				{
					panel.ItemImage.gameObject.SetActive(true);
					panel.ItemImage.sprite = slot.Item.Sprite;
					panel.ItemImage.CrossFadeAlpha(1, 0.05f, true);
					if(panel.ItemSlot.Stacks > 1)
					{
						panel.StacksText.gameObject.SetActive(true);
						panel.StacksText.text = slot.Stacks.ToString();
					}
					if(slot.Item is Tool)
					{
						panel.DurabilityBarImage.transform.parent.gameObject.SetActive(true);
						panel.DurabilityBarImage.fillAmount = 
							((slot.Item as Tool).Durability / (slot.Item as Tool).MaxDurability);
					}
				}
				else
				{
					panel.ItemImage.gameObject.SetActive(false);
					panel.StacksText.gameObject.SetActive(false);
					panel.DurabilityBarImage.transform.parent.gameObject.SetActive(false);
				}
			}
			index++;
		}

		if (EquippedToolItemPanel.ItemSlot.Item != null)
		{
			EquippedToolItemPanel.ItemImage.gameObject.SetActive(true);
			EquippedToolItemPanel.ItemImage.sprite = EquippedToolItemPanel.ItemSlot.Item.Sprite;
            EquippedToolItemPanel.DurabilityBarImage.transform.parent.
				gameObject.SetActive(true);
            EquippedToolItemPanel.DurabilityBarImage.fillAmount =
                (EquippedToolItemPanel.ItemSlot.Item as Tool).Durability;
        }
		else
		{
			EquippedToolItemPanel.ItemImage.gameObject.SetActive(false);
            EquippedToolItemPanel.DurabilityBarImage.transform.parent.
                gameObject.SetActive(false);
        }

		Mouse.SetUI();
	}

	private void ShowInventoryMenu(bool show)
	{
		Player.Instance.FPMouse(!show);
		InventoryMenu.SetActive(show);
		if(!show)
		{
			Mouse.EmptySlot();
		}
		RefreshInventory();
	}

	public List<Item> LoadAllItems()
	{
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");
		filePath = System.IO.Path.Combine(filePath, "item");

		//load assetBundle
		var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
		AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;

		//load items
		AssetBundleRequest assetRequest = assetBundle.LoadAllAssetsAsync<Item>();

		List<Item> loadedItems = new List<Item>();
		for (int i = 0; i < assetRequest.allAssets.Length; i++)
		{
			Item item = assetRequest.allAssets[i] as Item;

			//load item data
            item.LoadData(assetBundle);
            loadedItems.Add(item);
		}
		return loadedItems;
	}
}