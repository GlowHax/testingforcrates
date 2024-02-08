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
			ItemImage.sprite = ItemSlot.Item.InventorySprite;
			StacksText.text = ItemSlot.Stacks.ToString();

			if (ItemSlot.Durability > 0)
			{
				DurabilityBarImage.transform.parent.gameObject.SetActive(true);
				DurabilityBarImage.fillAmount = ItemSlot.Durability / ItemSlot.MaxDurability;
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

    public void ClearSlot()
    {
        ItemSlot = new ItemSlotInfo(null, 0, 0, 0);
    }
}