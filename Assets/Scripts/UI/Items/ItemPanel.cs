using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    public Inventory Inventory;
    public ItemSlotInfo ItemSlot;
    public Image ItemImage;
    public TextMeshProUGUI StacksText;

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

	public void PickupItem()
    {
        mouse.ItemSlot = ItemSlot;
        mouse.SourceItemPanel = this;
        if(Input.GetMouseButtonUp(1) && ItemSlot.Stacks > 1) 
        {
            if(ItemSlot.Stacks % 2 == 0)
            {
				mouse.SplitSize = (ItemSlot.Stacks / 2);
			}
            else
            {
				mouse.SplitSize = (ItemSlot.Stacks / 2) + 1;
			}
		}
        else
        {
            mouse.SplitSize = ItemSlot.Stacks;
        }
        mouse.SetUI();
    }

    public void DropItem()
    {
        ItemSlot.Item = mouse.ItemSlot.Item;
        if(mouse.SplitSize < mouse.ItemSlot.Stacks)
        {
            ItemSlot.Stacks = mouse.SplitSize;
            mouse.ItemSlot.Stacks -= mouse.SplitSize;
            mouse.EmptySlot();
        }
        else
        {
			ItemSlot.Stacks = mouse.ItemSlot.Stacks;
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

    public void StackItem(ItemSlotInfo source, ItemSlotInfo destination, int amount)
    {
        int slotsAvailable = destination.Item.MaxStacks() - destination.Stacks;
        if (slotsAvailable == 0)
            return;

        if(amount > slotsAvailable)
        {
            source.Stacks -= slotsAvailable;
            destination.Stacks = destination.Item.MaxStacks();
        }
        if(amount <= slotsAvailable)
        {
            destination.Stacks += amount;
            if(source.Stacks == amount)
            {
                Inventory.ClearSlot(source);
            }
            else
            {
                source.Stacks -= amount;
            }
        }
    }

	public void FadeOut()
    {
        ItemImage.CrossFadeAlpha(0.3f, 0.05f, true);
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
                    PickupItem();
                    FadeOut();
                }
            }
            else
            {
                //clicked on original slot
                if(ItemSlot == mouse.ItemSlot)
                {
                    Inventory.RefreshInventory();
                }
				//clicked on empty slot
				else if (ItemSlot.Item == null)
                {
                    DropItem();
                    Inventory.RefreshInventory();
                }
                //clicked on occupied slot of different item type
                else if (ItemSlot.Item.GetName() != mouse.ItemSlot.Item.GetName())
                {
                    SwapItem(ItemSlot, mouse.ItemSlot);
					Inventory.RefreshInventory();
				}
				//clicked on occupied slot of same item type
                else if(ItemSlot.Stacks < ItemSlot.Item.MaxStacks())
                {
                    StackItem(mouse.ItemSlot, ItemSlot, mouse.SplitSize);
                    Inventory.RefreshInventory();
                }
			}
		}
    }
}
