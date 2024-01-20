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

	[HideInInspector] public Dictionary<string, Item> AllItemsDictionary = new Dictionary<string, Item>();

	[SerializeField] private Transform toolHolderTransform;

	[SerializeField] private GameObject droppedItemObject;
	[SerializeField] string[] assetBundleNames;

	private List<ItemPanel> existingPanels = new List<ItemPanel>();
	List<AssetBundle> assetBundles = new List<AssetBundle>();

	private void Start()
	{
		LoadAssetBundles(assetBundleNames);
		CreateEmptyInventory();
		FillAllItemsDictionary();

		//for testing
		AddItem("Wood", 40);
		AddItem("Stone", 30);
		AddItem("Crowbar", 2);
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
			DropItem(Mouse.ItemSlot.Item, Mouse.ItemSlot.Stacks);
		}
	}

	private void FillAllItemsDictionary()
	{
		List<Item> allItems = LoadAllItems("item");
		string itemsInDictionary = "Items in Dictionary: ";
		foreach (Item item in allItems)
		{
			if (!AllItemsDictionary.ContainsKey(item.Name))
			{
				item.LoadData(assetBundles);
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
		Mouse.ItemSlot = new ItemSlotInfo(null, 0);
	}

	public void EquipTool(Tool tool)
	{
		tool.Prefab = Instantiate(tool.Prefab, toolHolderTransform);
		Player.Instance.equippedTool = tool;		
	}

	/// <returns>Amount not possible to add</returns>
	public int AddItem(string itemName, int amount)
	{
		//find Item to add
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

	public void DropItem(Item itemDrop, int amount)
	{
		//find Item to drop
		Item item = null;
		AllItemsDictionary.TryGetValue(itemDrop.Name, out item);
		//exit if no item was found
		if (item == null)
		{
			Debug.Log($"Could not find Item '{itemDrop.Name}' in Dictionary to drop");
			return;
		}

		GameObject droppedItem = Instantiate(droppedItemObject,
			Player.Instance.transform.position + new Vector3(0, 0.5f, 0) + Player.Instance.Cam.transform.forward,
			Quaternion.identity);

		ItemPickup itemPickup = droppedItem.GetComponentInChildren<ItemPickup>();
		if (itemPickup != null)
		{
			itemPickup.itemToDrop = item.Name;
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
							((Tool)slot.Item).Durability / ((Tool)slot.Item).MaxDurability;
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

		EquippedToolItemPanel.Inventory = this;
		if (EquippedToolItemPanel.ItemSlot.Item != null)
		{
			EquippedToolItemPanel.ItemImage.gameObject.SetActive(true);
			EquippedToolItemPanel.ItemImage.sprite = EquippedToolItemPanel.ItemSlot.Item.Sprite;
			EquippedToolItemPanel.ItemImage.CrossFadeAlpha(1, 0.05f, true);
			EquippedToolItemPanel.ItemImage.CrossFadeAlpha(1, 0.05f, true);
		}
		else
		{
			EquippedToolItemPanel.ItemImage.gameObject.SetActive(false);
			EquippedToolItemPanel.DurabilityBarImage.gameObject.SetActive(false);
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

	public List<Item> LoadAllItems(string assetBundleName)
	{
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");
		filePath = System.IO.Path.Combine(filePath, assetBundleName);

		//Load AssetBundle
		var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
		AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;

		//Load Items
		AssetBundleRequest assetRequest = assetBundle.LoadAllAssetsAsync<Item>();

		List<Item> loadedItems = new List<Item>();
		for (int i = 0; i < assetRequest.allAssets.Length; i++)
		{
			loadedItems.Add(assetRequest.allAssets[i] as Item);
			Debug.Log(assetRequest.allAssets[i].name + " loaded");
		}
		return loadedItems;
	}

	public void LoadAssetBundles(string[] assetBundleNames)
	{
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");

		for(int i = 0; i < assetBundleNames.Length; i++)
		{
			//Load AssetBundle
			assetBundles.Add(AssetBundle.
				LoadFromFileAsync(System.IO.Path.Combine(filePath, assetBundleNames[i])).
				assetBundle);
		}
	}
}