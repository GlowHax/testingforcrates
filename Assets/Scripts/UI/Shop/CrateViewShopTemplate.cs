using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateViewShopTemplate : ShopUITemplate
{
    public CrateSO Crate;

	public override void Setup()
	{
		base.Setup();
		nameTxt.text = Crate.Name;
		costTxt.text = $"-{Crate.MoneyCost}C";
	}

	public override void Purchase()
	{
		Crate.Purchase();
		base.Purchase();
	}

	public override void RefreshCostTextColor()
	{
		if (Player.Instance.Coins < Crate.MoneyCost)
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
