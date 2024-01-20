using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mouse : MonoBehaviour
{
    public ItemSlotInfo ItemSlot;
    public Image ItemImage;
    public TextMeshProUGUI StacksText;
    public Image DurabilityBarImage;

	void Update()
    {
        transform.position = Input.mousePosition;      
	}

    public void SetUI()
    {
        if (ItemSlot.Item == null)
		{
			ItemImage.gameObject.SetActive(false);
			StacksText.gameObject.SetActive(false);
			DurabilityBarImage.transform.parent.gameObject.SetActive(false);
		}
        else
        {
			ItemImage.gameObject.SetActive(true);
			if (ItemSlot.Stacks > 1)
			{
				StacksText.gameObject.SetActive(true);
			}
			ItemImage.sprite = ItemSlot.Item.Sprite;
			StacksText.text = ItemSlot.Stacks.ToString();

			if (ItemSlot.Item is Tool)
			{
				DurabilityBarImage.transform.parent.gameObject.SetActive(true);
				DurabilityBarImage.fillAmount = ((Tool)ItemSlot.Item).Durability;
			}
			else
			{
				DurabilityBarImage.transform.parent.gameObject.SetActive(false);
				if (ItemSlot.Stacks > 1)
				{
					StacksText.gameObject.SetActive(true);
				}
			}
		}
	}

    public void EmptySlot()
    {
		if (ItemSlot.Item != null)
		{
			int amountToDrop = Player.Instance.Inventory.AddItem(ItemSlot.Item.Name, ItemSlot.Stacks);
			if(amountToDrop > 0)
			{
				Player.Instance.Inventory.DropItem(ItemSlot.Item, amountToDrop);
			}
		}
		Player.Instance.Inventory.ClearSlot(ItemSlot);
	}
}
