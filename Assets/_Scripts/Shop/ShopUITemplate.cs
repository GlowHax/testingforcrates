using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUITemplate : MonoBehaviour
{
	private Item item;

	[SerializeField] private TMP_Text itemNameText;
	[SerializeField] private TMP_Text itemCostText;
	[SerializeField] private Button buyButton;

	public void Initialize(Item item)
	{
		this.item = item;
		RefreshCostTextColor();
		itemNameText.text = item.Name;
		itemCostText.text = $"{item.Value}C";
	}

	public void Purchase()
	{
		if (Player.Instance.Coins >= item.Value)
		{
			Player.Instance.Coins -= item.Value;
			Player.Instance.Inventory.AddItem(item.Name, 1);
		}
		RefreshCostTextColor();
	}

	public void RefreshCostTextColor()
	{
		if (Player.Instance.Coins < item.Value)
		{
			buyButton.interactable = false;
		}
		else
		{
			buyButton.interactable = true;
		}
	}
}