using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryView : View
{
    public Inventory Inventory;

    public ItemPanel ItemPanelPrefab;
    public ItemPanel EquippedToolItemPanel;
    public GameObject ItemPanelGrid;
    public Mouse Mouse;

    private List<ItemPanel> existingPanels = new List<ItemPanel>();

    public override void Initialize()
    {
        Inventory = Player.Instance.Inventory;
        RefreshInventory();
    }

    public override void Show(Transform parent = null)
    {
        base.Show(parent);
        Initialize();
    }

    public override void Hide()
    {
        if (Mouse.ItemSlot.Item != null)
        {
            //empty mouse slot to inventory
            int overflowingItems = Player.Instance.Inventory.
                AddItem(Mouse.ItemSlot.Item.Name, Mouse.ItemSlot.Stacks);
            if (overflowingItems > 0)
            {
                //drop the rest if inventory is full
                Player.Instance.Inventory.
                    DropItem(Mouse.ItemSlot.Item.Name, overflowingItems);
            }
            Mouse.ClearSlot();
        }

        base.Hide();
    }

    public void RefreshInventory()
    {
        existingPanels = ItemPanelGrid.GetComponentsInChildren<ItemPanel>().ToList();

        //create panels if needed
        if (existingPanels.Count < Inventory.InventorySize)
        {
            int amountToCreate = Inventory.InventorySize - existingPanels.Count;
            for (int i = 0; i < amountToCreate; i++)
            {
                ItemPanel newPanel =
                    Instantiate(ItemPanelPrefab, ItemPanelGrid.transform);
                existingPanels.Add(newPanel);
            }
        }

        int index = 0;
        foreach (ItemSlotInfo slot in Inventory.Items)
        {
            //name list elements 
            slot.Name = (index + 1).ToString();
            if (slot.Item != null)
            {
                slot.Name += ": " + slot.Item.Name;
            }
            else
            {
                slot.Name += ": -";
            }

            //update panels
            ItemPanel panel = existingPanels[index];
            if (panel != null)
            {
                panel.name = slot.Name + " Panel";
                panel.InventoryView = this;
                panel.ItemSlot = slot;
                if (slot.Item != null)
                {
                    panel.ItemImage.gameObject.SetActive(true);
                    panel.ItemImage.sprite = slot.Item.InventorySprite;
                    panel.ItemImage.CrossFadeAlpha(1, 0.05f, true);
                    if (panel.ItemSlot.Stacks > 1)
                    {
                        panel.StacksText.gameObject.SetActive(true);
                        panel.StacksText.text = slot.Stacks.ToString();
                    }
                    if (slot.Durability > 0)
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
}
