using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public InventoryView InventoryView;
    public ItemSlotInfo ItemSlot;
    public Image ItemImage;
    public TextMeshProUGUI StacksText;
    public Image DurabilityBarImage;


	protected Mouse mouse;
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

	public virtual void PickupItem(bool halfSplit)
    {
		mouse.ItemSlot.Item = ItemSlot.Item;
		mouse.ItemImage.sprite = ItemSlot.Item.InventorySprite;
		mouse.ItemSlot.Durability = ItemSlot.Durability;
		mouse.ItemSlot.MaxDurability = ItemSlot.MaxDurability;

        if (halfSplit)
        {
            if (ItemSlot.Stacks == 1)
            {
                mouse.ItemSlot.Stacks = ItemSlot.Stacks;
                ClearSlot();
            }
            else if (ItemSlot.Stacks % 2 == 0)
            {
                mouse.ItemSlot.Stacks = (ItemSlot.Stacks / 2);
                ItemSlot.Stacks -= mouse.ItemSlot.Stacks;
            }
            else
            {
                mouse.ItemSlot.Stacks = (ItemSlot.Stacks / 2) + 1;
                ItemSlot.Stacks -= mouse.ItemSlot.Stacks;
            }
        }
        else
        {
            mouse.ItemSlot.Stacks = ItemSlot.Stacks;
            ClearSlot();
        }
    }

    public virtual void DropItem(bool singleItemDrop)
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
            mouse.ClearSlot();
        }
	}

	public virtual void SwapItem()
	{
		ItemSlotInfo tempItem = new ItemSlotInfo(
            ItemSlot.Item, ItemSlot.Stacks, ItemSlot.Durability, 
            ItemSlot.MaxDurability);

        ItemSlot.Item = mouse.ItemSlot.Item;
        ItemSlot.Stacks = mouse.ItemSlot.Stacks;
        ItemSlot.Durability = mouse.ItemSlot.Durability;
        ItemSlot.MaxDurability = mouse.ItemSlot.MaxDurability;

        mouse.ItemSlot.Item = tempItem.Item;
        mouse.ItemSlot.Stacks = tempItem.Stacks;
        mouse.ItemSlot.Durability = tempItem.Durability;
        mouse.ItemSlot.MaxDurability = tempItem.MaxDurability;
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
				mouse.ClearSlot();
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
				mouse.ClearSlot();
			}
		}
	}

    public void ClearSlot()
    {
        ItemSlot.Item = null;
        ItemSlot.Stacks = 0;
        ItemSlot.Durability = 0;
        ItemSlot.MaxDurability = 0;
    }

    public void OnClick()
    {
        if(InventoryView != null)
        {
            mouse = InventoryView.Mouse;

            //grab item if mouse slot is empty
            if(mouse.ItemSlot.Item == null)
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
					SwapItem();
				}
			}

			InventoryView.RefreshInventory();
		}
	}
}