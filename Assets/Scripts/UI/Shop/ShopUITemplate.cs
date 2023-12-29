using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShopUITemplate : MonoBehaviour
{
	[SerializeField] protected TMP_Text nameTxt;
	[SerializeField] protected TMP_Text costTxt;
	[SerializeField] protected Button buyButton;

	public virtual void Setup()
	{
		RefreshCostTextColor();
	}

    public virtual void Purchase()
	{
		RefreshCostTextColor();
	}

	public virtual void RefreshCostTextColor()
	{

	}
}
