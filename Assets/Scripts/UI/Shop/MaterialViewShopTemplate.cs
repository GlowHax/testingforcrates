using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialViewShopTemplate : ShopUITemplate
{
	public MaterialSO Material;

	public override void Setup()
	{
		base.Setup();
		nameTxt.text = Material.Name;
		costTxt.text = $"-{Material.MoneyCost}C";
	}

	public override void Purchase()
	{
		//Crate.Purchase();
		base.Purchase();
	}

	public override void RefreshCostTextColor()
	{
		if (User.Instance.Coins < Material.MoneyCost)
		{
			costTxt.color = Color.gray;
			buyButton.interactable = false;
		}
		else
		{
			costTxt.color = Color.black;
			buyButton.interactable = true;
		}
	}
}