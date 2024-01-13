using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : Terminal
{
	[SerializeField] GameObject ShopViewTemplate;
	[SerializeField] GameObject lockedCrateElement;
	[SerializeField] GameObject shopScrollContent;
	[SerializeField] List<Button> switchOfferButtons = new List<Button>();

	private void Start()
	{
		RefreshShopScrollContent("crates");
	}

	public void SwitchOffer(string newOffer)
	{
		switch (newOffer)
		{
			case "crates":
				for (int i = 0; i < switchOfferButtons.Count; i++)
				{
					switchOfferButtons[i].interactable = true;
				}
				switchOfferButtons[0].interactable = false;

				RefreshShopScrollContent(newOffer);

				break;

			case "materials":
				for (int i = 0; i < switchOfferButtons.Count; i++)
				{
					switchOfferButtons[i].interactable = true;
				}
				switchOfferButtons[1].interactable = false;

				RefreshShopScrollContent(newOffer);
				break;
		}
	}

	private void RemoveShopScrollContent()
	{
		for (int i = 0; i < shopScrollContent.transform.childCount; i++)
		{
			Destroy(shopScrollContent.transform.GetChild(i).gameObject);
		}
	}

	private void RefreshShopScrollContent(string currentOffer)
	{
		RemoveShopScrollContent();
		switch (currentOffer)
		{
			case "crates":
				for (int i = 0; i < Player.Instance.OldInventory.Crates.Count; i++)
				{
					CrateSO crate = Player.Instance.OldInventory.Crates[i];
					if (crate.Type == CrateType.Main)
					{
						if (Player.Instance.Level >= crate.UnlockLvl)
						{
							CrateViewShopTemplate crateView =
								Instantiate(ShopViewTemplate, shopScrollContent.transform).GetComponent<CrateViewShopTemplate>();
							crateView.Crate = crate;
							crateView.Setup();
						}
						else
						{
							Instantiate(lockedCrateElement, shopScrollContent.transform);
						}
					}
				}
				break;

			case "materials":
				for (int i = 0; i < Player.Instance.OldInventory.Materials.Count; i++)
				{
					MaterialViewShopTemplate materialView =
						Instantiate(ShopViewTemplate, shopScrollContent.transform).GetComponent<MaterialViewShopTemplate>();
					materialView.Material = Player.Instance.OldInventory.Materials[i];
					materialView.Setup();
				}
				break;
		}
	}
}
