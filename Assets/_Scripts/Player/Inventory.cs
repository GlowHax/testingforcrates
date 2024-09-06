using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeReference] public List<ItemSlotInfo> Items = new List<ItemSlotInfo>();
    public Tool EquippedTool;

    [Space]
	public int InventorySize;

    [SerializeField] private InputReader input;
    [SerializeField] private InventoryView InventoryView;
    [SerializeField] private Transform toolHolderTransform;
	[SerializeField] private GameObject droppedItemPrefab;

	private void Awake()
	{
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
	}

    private void Start()
    {
        input.InventoryEvent += ToggleInventory;
    }

    private void OnDestroy()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	}

    private void Update()
	{
		//if(Input.GetMouseButtonDown(0) && 
		//	Mouse.ItemSlot.Item != null && 
		//	InventoryMenu.activeSelf &&
		//	!EventSystem.current.IsPointerOverGameObject())
		//{
		//	DropItem(Mouse.ItemSlot.Item.Name, Mouse.ItemSlot.Stacks);
		//}
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
        //EquippedToolItemPanel.Inventory = this;
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

		foreach (ItemSlotInfo slot in Items)
		{
			if(slot.Item != null) //check for open stack-capacities in existing slots
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
                        amount = 0;
                        if (InventoryView.objectInScene != null)
						{
                            InventoryView.RefreshInventory();
						}
						break;
					}
				}
			}
            else //fill empty slots with leftover items
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
                    if (InventoryView.objectInScene != null)
                    {
                        InventoryView.RefreshInventory();
                    }
                    amount = 0;
					break;
                }
            }
		}

        //not enough inventory space
        if (amount > 0) 
        {
            Debug.Log($"No space in inventory ({amount}x {item.Name})");
            if (InventoryView.objectInScene != null)
            {
                InventoryView.RefreshInventory();
            }
        }
		return amount;
	}

	public void RemoveItem(string itemName, int amount)
	{
		Item item = ResourceSystem.Instance.GetItem(itemName);

		foreach (ItemSlotInfo slot in Items)
        {
            if (slot.Item != null)
            {
                if (slot.Item.Name == item.Name)
                {
                    if (amount > slot.Stacks)
					{
						amount -= slot.Stacks;
                    }
					else
					{
						slot.Stacks -= amount;
					}
                    break;
                }
            }
        }

		if (InventoryView.objectInScene != null)
		{
			InventoryView.RefreshInventory();
		}
    }

	public void DropItem(string itemName, int amount)
	{
		Item item = ResourceSystem.Instance.GetItem(itemName);

		GameObject droppedItem = Instantiate(droppedItemPrefab,
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

		if(InventoryView.objectInScene != null)
		{
			InventoryView.RefreshInventory();
		}
	}

    private void ToggleInventory()
    {
		if(InventoryView.objectInScene == null)
		{
            UIManager.Instance.ShowScreen(InventoryView);
        }
		else
		{
			UIManager.Instance.ShowLast();
		}
    }
}