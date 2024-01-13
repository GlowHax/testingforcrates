using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mouse : MonoBehaviour
{
    public GameObject MouseItemUI;
    public Image MouseCursor;
    public ItemSlotInfo ItemSlot;
    public Image ItemImage;
    public TextMeshProUGUI StacksText;
	public ItemPanel SourceItemPanel;
	public int SplitSize;

	void Update()
    {
        transform.position = Input.mousePosition;

        if(ItemSlot.Item != null)
        {
            MouseItemUI.SetActive(true);
        }
        else
        {
            MouseItemUI.SetActive(false);
        }       
	}

    public void SetUI()
    {
		StacksText.gameObject.SetActive(true);
		SourceItemPanel.StacksText.gameObject.SetActive(true);

		if (SplitSize == 1)
        {
			StacksText.gameObject.SetActive(false);
			SourceItemPanel.StacksText.gameObject.SetActive(false);
		}
        else if(SplitSize == 2 || SplitSize == SourceItemPanel.ItemSlot.Stacks)
        {
			SourceItemPanel.StacksText.gameObject.SetActive(false);
		}
        else
        {
			SourceItemPanel.StacksText.text = (ItemSlot.Stacks - SplitSize).ToString();
		}

		ItemImage.sprite = ItemSlot.Item.GetItemImage();
		StacksText.text = SplitSize.ToString();
	}

    public void EmptySlot()
    {
        ItemSlot = new ItemSlotInfo(null, 0);
    }
}
