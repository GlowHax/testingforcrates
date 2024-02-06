using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : Terminal
{
	[SerializeField] ShopUITemplate shopUITemplate;
	[SerializeField] GameObject lockedCrateElement;
	[SerializeField] GameObject shopScrollContent;
	[SerializeField] List<Button> switchOfferButtons = new List<Button>();

	private List<Crate> allCrates = new List<Crate>();
	private List<Material> allMaterials = new List<Material>();

	public override void SwitchOn()
	{
		base.SwitchOn();
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
				foreach(Crate crate in ResourceSystem.Instance.GetAllItemsOfType<Crate>())
				{
					if (crate.Type == CrateType.Main)
					{
						if (Player.Instance.Level >= crate.UnlockLvl)
						{
							ShopUITemplate crateOffer =
								Instantiate(shopUITemplate, shopScrollContent.transform);
							crateOffer.Initialize(crate);
						}
						else
						{
							Instantiate(lockedCrateElement, shopScrollContent.transform);
						}
					}
				}
				break;

			case "materials":
				foreach (Material material in ResourceSystem.Instance.GetAllItemsOfType<Material>())
				{
					ShopUITemplate materialOffer = 
						Instantiate(shopUITemplate, shopScrollContent.transform);
					materialOffer.Initialize(material);
				}
				break;
		}
	}
}