using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItemPanel : ItemPanel
{
    public override void PickupItem(bool halfSplit)
    {
        mouse.ItemSlot.Item = ItemSlot.Item;
        mouse.ItemImage.sprite = ItemSlot.Item.InventorySprite;
        mouse.ItemSlot.Durability = ItemSlot.Durability;
        mouse.ItemSlot.MaxDurability = ItemSlot.MaxDurability;

        Inventory.EquipTool(ItemSlot.Item as Tool, false);
        ClearSlot();
    }

    public override void DropItem(bool singleItemDrop)
    {
        if (mouse.ItemSlot.Item is Tool)
        {
            ItemSlot.Item = mouse.ItemSlot.Item;
            ItemSlot.Stacks = mouse.ItemSlot.Stacks;
            ItemSlot.Durability = mouse.ItemSlot.Durability;
            ItemSlot.MaxDurability = mouse.ItemSlot.MaxDurability;
            Inventory.EquipTool(ItemSlot.Item as Tool, true);
            mouse.ClearSlot();
        }
    }

    public override void SwapItem()
    {
        if(mouse.ItemSlot.Item is Tool)
        {
            base.SwapItem();
        }
    }
}