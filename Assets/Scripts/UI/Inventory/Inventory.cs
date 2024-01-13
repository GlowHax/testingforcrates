using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeReference] public List<ItemSlotInfo> Items = new List<ItemSlotInfo>();

    [Space]
    [Header("Menu Components")]
    public GameObject InventoryMenu;
    public GameObject ItemPanel;
    public GameObject ItemPanelGrid;

	public Mouse Mouse;

	private List<ItemPanel> existingPanels = new List<ItemPanel>();

    [Space]
    public int InventorySize = 25;

	private void Start()
	{
		CreateEmptyInventory();

		//for testing
		AddItem(new CrowBar(), 7);
	}

	private void CreateEmptyInventory()
    {
        for(int i = 0; i < InventorySize; i++)
        {
            Items.Add(new ItemSlotInfo(null, 0));
        }
    }

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryMenu.activeSelf)
			{
				ShowInventoryMenu(false);
				Mouse.EmptySlot();
			}
			else
            {
				ShowInventoryMenu(true);
			}
        }

		if(Input.GetKeyDown(KeyCode.Escape) && InventoryMenu.activeSelf)
		{
			ShowInventoryMenu(false);
			Mouse.EmptySlot();
		}
	}

	/// <returns>Amount not possible to add</returns>
	public int AddItem(Item item, int amount)
	{
		//check for open stack-capacities in existing slots
		foreach(ItemSlotInfo slot in Items)
		{
			if(slot.Item != null)
			{
				if(slot.Item.GetName() == item.GetName())
				{
					if(amount > slot.Item.MaxStacks() - slot.Stacks)
					{
						amount -= slot.Item.MaxStacks() - slot.Stacks;
						slot.Stacks = slot.Item.MaxStacks();
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
				if(amount > item.MaxStacks())
				{
					slot.Item = item;
					slot.Stacks = item.MaxStacks();
					amount -= item.MaxStacks();
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
		Debug.Log($"No space in inventory ({item.GetName()})");
		if (InventoryMenu.activeSelf)
		{
			RefreshInventory();
		}
		return amount;
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
				slot.Name += ": " + slot.Item.GetName();
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
					panel.ItemImage.sprite = slot.Item.GetItemImage();
					panel.ItemImage.CrossFadeAlpha(1, 0.05f, true);
					if(panel.ItemSlot.Stacks > 1)
					{
						panel.StacksText.gameObject.SetActive(true);
						panel.StacksText.text = slot.Stacks.ToString();
					}
				}
				else
				{
					panel.ItemImage.gameObject.SetActive(false);
					panel.StacksText.gameObject.SetActive(false);
				}
			}
			index++;
		}
		Mouse.EmptySlot();
	}

	private void ShowInventoryMenu(bool value)
	{
		Player.Instance.FPMouse(!value);
		InventoryMenu.SetActive(value);

		if(value)
		{
			RefreshInventory();
		}
	}
}