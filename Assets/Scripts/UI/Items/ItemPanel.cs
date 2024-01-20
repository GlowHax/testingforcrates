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
		mouse.ItemImage.sprite = ItemSlot.Item.Sprite;

		if(PanelType == ItemPanelType.Inventory)
		{
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
				mouse.ItemSlot.Stacks = ItemSlot.Stacks;
				Inventory.ClearSlot(ItemSlot);
			}
		}
    }

    public void DropItem(bool singleItemDrop)
    {
		ItemSlot.Item = mouse.ItemSlot.Item;

		if (PanelType == ItemPanelType.Inventory)
        {
			ItemSlot.Item = mouse.ItemSlot.Item;
			ItemImage.sprite = ItemSlot.Item.Sprite;

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
			Inventory.EquipTool(ItemSlot.Item as Tool);
			Inventory.ClearSlot(mouse.ItemSlot);
		}
	}

	public void SwapItem(ItemSlotInfo slotA, ItemSlotInfo slotB)
	{
		ItemSlotInfo tempItem = new ItemSlotInfo(slotA.Item, slotA.Stacks);

		slotA.Item = slotB.Item;
        slotA.Stacks = slotB.Stacks;

        slotB.Item = tempItem.Item;
        slotB.Stacks = tempItem.Stacks;
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
				//clicked on original slot or empty slot
				if (ItemSlot == mouse.ItemSlot || ItemSlot.Item == null)
                {
					DropItem(Input.GetMouseButtonUp(1));
				}
                //clicked on occupied slot of different item type
                else if (ItemSlot.Item.Name != mouse.ItemSlot.Item.Name)
                {
                    SwapItem(ItemSlot, mouse.ItemSlot);
				}
				//clicked on occupied slot of same item type
                else if(ItemSlot.Stacks < ItemSlot.Item.MaxInventoryStacks)
                {
					StackItem(Input.GetMouseButtonUp(1));
				}
			}

			Inventory.RefreshInventory();
		}
	}
}
