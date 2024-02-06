using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum ItemPanelType
{
    Inventory,
    ToolEquip,
    PassiveEquip
}

public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public Inventory Inventory;
    public ItemSlotInfo ItemSlot;
    public Image ItemImage;
    public TextMeshProUGUI StacksText;
    public Image DurabilityBarImage;

	public ItemPanelType PanelType = ItemPanelType.Inventory;

	private Mouse mouse;
    private bool click;

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.pointerPress = this.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        click = true;
    }

	public void OnPointerUp(PointerEventData eventData)
	{
		if (click)
        {
            OnClick();
            click = false;
        }
	}

	public void PickupItem(bool halfSplit)
    {
		mouse.ItemSlot.Item = ItemSlot.Item;
		mouse.ItemImage.sprite = ItemSlot.Item.InventorySprite;
		mouse.ItemSlot.Durability = ItemSlot.Durability;
		mouse.ItemSlot.MaxDurability = ItemSlot.MaxDurability;

        if (halfSplit)
        {
            if (ItemSlot.Stacks % 2 == 0)
            {
                mouse.ItemSlot.Stacks = (ItemSlot.Stacks / 2);
            }
            else
            {
                mouse.ItemSlot.Stacks = (ItemSlot.Stacks / 2) + 1;
            }
            ItemSlot.Stacks -= mouse.ItemSlot.Stacks;
        }
        else
        {
            if(PanelType == ItemPanelType.ToolEquip)
            {
				Inventory.EquipTool(ItemSlot.Item as Tool, false);
			}
			mouse.ItemSlot.Stacks = ItemSlot.Stacks;
            Inventory.ClearSlot(ItemSlot);
        }
    }

    public void DropItem(bool singleItemDrop)
    {
		if (PanelType == ItemPanelType.Inventory)
        {
			ItemSlot.Item = mouse.ItemSlot.Item;
			ItemSlot.Durability = mouse.ItemSlot.Durability;
			ItemSlot.MaxDurability = mouse.ItemSlot.MaxDurability;

			if (singleItemDrop && mouse.ItemSlot.Stacks > 1)
			{
				ItemSlot.Stacks = 1;
				mouse.ItemSlot.Stacks--;
			}
			else
			{
				ItemSlot.Stacks = mouse.ItemSlot.Stacks;
				Inventory.ClearSlot(mouse.ItemSlot);
			}
		}
		else if(PanelType == ItemPanelType.ToolEquip && mouse.ItemSlot.Item is Tool)
		{
			ItemSlot.Item = mouse.ItemSlot.Item;
			ItemSlot.Stacks = mouse.ItemSlot.Stacks;
			ItemSlot.Durability = mouse.ItemSlot.Durability;
			ItemSlot.MaxDurability = mouse.ItemSlot.MaxDurability;
			Inventory.EquipTool(ItemSlot.Item as Tool, true);
			Inventory.ClearSlot(mouse.ItemSlot);
		}
	}

	public void SwapItem(ItemSlotInfo slotA, ItemSlotInfo slotB)
	{
		ItemSlotInfo tempItem = new ItemSlotInfo(
            slotA.Item, slotA.Stacks, slotA.Durability, slotA.MaxDurability);

		slotA.Item = slotB.Item;
        slotA.Stacks = slotB.Stacks;
        slotA.Durability = slotB.Durability;
        slotA.MaxDurability = slotB.MaxDurability;

        slotB.Item = tempItem.Item;
        slotB.Stacks = tempItem.Stacks;
        slotB.Durability = tempItem.Durability;
        slotB.MaxDurability = tempItem.MaxDurability;
	}

    public void StackItem(bool singleItemStack)
    {
        int slotsAvailable = ItemSlot.Item.MaxInventoryStacks - ItemSlot.Stacks;
        if (slotsAvailable <= 0)
            return;

        if (singleItemStack)
        {
			ItemSlot.Stacks++;
			if (mouse.ItemSlot.Stacks == 1)
			{
				Inventory.ClearSlot(mouse.ItemSlot);
			}
			else
			{
				mouse.ItemSlot.Stacks--;
			}
		}
		else
        {
			if (mouse.ItemSlot.Stacks > slotsAvailable)
			{
				mouse.ItemSlot.Stacks -= slotsAvailable;
				ItemSlot.Stacks = ItemSlot.Item.MaxInventoryStacks;
			}
			else
			{
				ItemSlot.Stacks += mouse.ItemSlot.Stacks;
				Inventory.ClearSlot(mouse.ItemSlot);
			}
		}
	}

    public void OnClick()
    {
        if(Inventory != null)
        {
            mouse = Inventory.Mouse;

            //grab item if mouse slot is empty
            if(mouse.ItemSlot.Item == null )
            {
                if(ItemSlot.Item != null)
                {
                    PickupItem(Input.GetMouseButtonUp(1));
                }
            }
            else
            {
				//clicked on empty slot
				if (ItemSlot.Item == null)
                {
					DropItem(Input.GetMouseButtonUp(1));
				}
				//clicked on occupied slot of same item type
                else if(ItemSlot.Item.Name == mouse.ItemSlot.Item.Name &&
                    ItemSlot.Stacks < ItemSlot.Item.MaxInventoryStacks)
                {
					StackItem(Input.GetMouseButtonUp(1));
				}
                //clicked on different item type or durability
                else if(ItemSlot.Item.Name != mouse.ItemSlot.Item.Name ||
                    ItemSlot.Durability != mouse.ItemSlot.Durability)
                {
					SwapItem(ItemSlot, mouse.ItemSlot);
				}
			}

			Inventory.RefreshInventory();
		}
	}
}
