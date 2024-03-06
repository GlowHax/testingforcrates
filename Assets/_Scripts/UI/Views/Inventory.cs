using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

public class Inventory : View
{
    [SerializeReference] public List<ItemSlotInfo> Items = new List<ItemSlotInfo>();
	public ItemPanel EquippedToolItemPanel;
	public Tool EquippedTool;

	[Space]
	[Header("Menu Components")]
    public GameObject InventoryMenu;
    public ItemPanel ItemPanelPrefab;
    public GameObject ItemPanelGrid;
	public TMP_Text TitleText;

	public Mouse Mouse;

	[Space]
	public int InventorySize;

	[SerializeField] private Transform toolHolderTransform;

	[SerializeField] private GameObject droppedItemObject;

	private List<ItemPanel> existingPanels = new List<ItemPanel>();

	private void Awake()
	{
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
	}

	private void OnDestroy()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	}

    public override void Initialize()
    {

    }

    private void Update()
	{
		if(Input.GetMouseButtonDown(0) && 
			Mouse.ItemSlot.Item != null && 
			InventoryMenu.activeSelf &&
			!EventSystem.current.IsPointerOverGameObject())
		{
			DropItem(Mouse.ItemSlot.Item.Name, Mouse.ItemSlot.Stacks);
		}
	}

	private void GameManager_OnGameStateChanged(GameState state)
	{
		if (state == GameState.Starting)
		{
			CreateEmptyInventory();

			//for testing
			AddItem("Wood", 40);
			AddItem("Stone", 30);
			AddItem("Crowbar", 2);
			AddItem("Iron", 20);
			AddItem("Common Crate", 30);
		}
	}

	private void CreateEmptyInventory()
	{
		for (int i = 0; i < InventorySize; i++)
		{
			Items.Add(new ItemSlotInfo(null, 0, 0, 0));
		}
        EquippedToolItemPanel.Inventory = this;
    }

	public int GetAmountInInventory(string itemName)
	{
		int amount = 0;
        
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

	public void EquipTool(Tool tool, bool equip)
	{
		if (equip)
		{
			Instantiate(tool.Prefab, toolHolderTransform);
			Player.Instance.equippedTool = tool;
		}
		else
		{
			Destroy(toolHolderTransform.GetChild(0).gameObject);
			Player.Instance.equippedTool = null;
		}
	}

	/// <returns>Amount not possible to add</returns>
	public int AddItem(string itemName, int amount)
	{
		Item item = ResourceSystem.Instance.GetItem(itemName);

		//check for open stack-capacities in existing slots
		foreach (ItemSlotInfo slot in Items)
		{
			if(slot.Item != null)
			{
				if(slot.Item.Name == item.Name && slot.Stacks != slot.Item.MaxInventoryStacks)
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
				if (item is Tool)
				{
					slot.Durability = ((Tool)item).Durability;
					slot.MaxDurability = ((Tool)item).MaxDurability;
				}

				if (amount > item.MaxInventoryStacks)
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
		Item item = ResourceSystem.Instance.GetItem(itemName);

		foreach (ItemPanel panel in existingPanels)
        {
            if (panel.ItemSlot.Item != null)
            {
                if (panel.ItemSlot.Item.Name == item.Name)
                {
                    if (amount > panel.ItemSlot.Stacks)
					{
						amount -= panel.ItemSlot.Stacks;
						panel.ClearSlot();
						return;
                    }
					else
					{
						panel.ItemSlot.Stacks -= amount;
						return;
					}
                }
            }
        }
    }

	public void DropItem(string itemName, int amount)
	{
		Item item = ResourceSystem.Instance.GetItem(itemName);

		GameObject droppedItem = Instantiate(droppedItemObject,
			Player.Instance.transform.position + new Vector3(0, 0.5f, 0) + Player.Instance.Cam.transform.forward,
			Quaternion.identity);

		ItemPickup itemPickup = droppedItem.GetComponentInChildren<ItemPickup>();
		if (itemPickup != null)
		{
			itemPickup.ItemToDrop = item;
			itemPickup.Amount = amount;
		}

		Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.velocity = Player.Instance.Cam.transform.forward * 3f;
		}

		Mouse.ClearSlot();
		RefreshInventory();
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
				ItemPanel newPanel = 
					Instantiate(ItemPanelPrefab, ItemPanelGrid.transform);
				existingPanels.Add(newPanel);
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
					panel.ItemImage.sprite = slot.Item.InventorySprite;
					panel.ItemImage.CrossFadeAlpha(1, 0.05f, true);
					if(panel.ItemSlot.Stacks > 1)
					{
						panel.StacksText.gameObject.SetActive(true);
						panel.StacksText.text = slot.Stacks.ToString();
					}
					if(slot.Durability > 0)
					{
						panel.DurabilityBarImage.transform.parent.gameObject.SetActive(true);
						panel.DurabilityBarImage.fillAmount = slot.Durability / slot.MaxDurability;
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
			EquippedToolItemPanel.ItemImage.sprite = EquippedToolItemPanel.ItemSlot.Item.InventorySprite;
            EquippedToolItemPanel.DurabilityBarImage.transform.parent.
				gameObject.SetActive(true);
            EquippedToolItemPanel.DurabilityBarImage.fillAmount =
                EquippedToolItemPanel.ItemSlot.Durability / 
				EquippedToolItemPanel.ItemSlot.MaxDurability;
        }
		else
		{
			EquippedToolItemPanel.ItemImage.gameObject.SetActive(false);
            EquippedToolItemPanel.DurabilityBarImage.transform.parent.
                gameObject.SetActive(false);
        }

		Mouse.SetUI();
	}

    public override void Show(Transform parent = null)
    {
        Player.Instance.FPMouse(false);
        RefreshInventory();
        base.Show(parent);
    }

    public override void Hide()
    {
        if (Mouse.ItemSlot.Item != null)
        {
            //empty mouse slot to inventory
            int amountToDrop = Player.Instance.Inventory.
                AddItem(Mouse.ItemSlot.Item.Name, Mouse.ItemSlot.Stacks);
            if (amountToDrop > 0)
            {
                //drop the rest if inventory is full
                Player.Instance.Inventory.
                    DropItem(Mouse.ItemSlot.Item.Name, amountToDrop);
            }
        }
        Mouse.ClearSlot();
        Player.Instance.FPMouse(true);
        base.Hide();
    }
}