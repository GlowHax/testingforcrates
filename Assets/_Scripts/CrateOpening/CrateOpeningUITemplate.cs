using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class CrateOpeningUITemplate : MonoBehaviour
{
	[SerializeField] TMP_Text crateViewName;
	[SerializeField] Image crateViewImage;
	[SerializeField] TMP_Text counter;

	[SerializeField] Button spawnButton;
	[SerializeField] Button showStatsButton;

	[SerializeField] GameObject statsPanel;
	[SerializeField] TMP_Text statsText;

	[SerializeField] GameObject multiselectPanel;
	[SerializeField] GameObject selectedOverlay;

	private CrateOpeningTerminal crateOpeningUI;
	private Crate crate;

	private bool showingStats = false;

	public void Initialize(CrateOpeningTerminal crateOpeningUI, Crate crate, int amount)
	{
		this.crateOpeningUI = crateOpeningUI;
		this.crate = crate;

		gameObject.name = crate.Name;
		statsPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = crate.Stats;

		crateViewImage.sprite = crate.InventorySprite;
		crateViewName.text = crate.Name;
		counter.text = amount + "x";
		spawnButton.onClick.RemoveAllListeners();
		spawnButton.onClick.AddListener(() => SingleSelect());
		spawnButton.onClick.AddListener(() => crateOpeningUI.StartCrateOpening());
		showStatsButton.onClick.AddListener(() => ShowStats());
	}

	public void SingleSelect()
	{
		crateOpeningUI.SelectedCrates.Add(crate.Prefab);
	}

	public void ChangeSelectionAmount(int amountToChange)
	{
		List<char> count = counter.text.ToCharArray().ToList();
		count.Remove('x');

		int selectedAmount = int.Parse(new string(count.ToArray()));
		if ((selectedAmount + amountToChange) > Player.Instance.Inventory.
			GetAmountInInventory(crate.Name))
		{
			return;
		}
		else if ((selectedAmount + amountToChange) <= 0)
		{
			crateOpeningUI.SelectedCrates.Remove(crate.Prefab);
			counter.text = "0x";
			selectedOverlay.SetActive(true);
			return;
		}
		else
		{
			counter.text = (selectedAmount + amountToChange).ToString() + "x";
		}

		if (amountToChange > 0)
		{
			if(selectedOverlay.activeInHierarchy) selectedOverlay.SetActive(false);

			for (int i = 0; i < amountToChange; i++)
			{
				crateOpeningUI.SelectedCrates.Add(crate.Prefab);
			}
		}
		else if(amountToChange < 0)
		{
			for (int i = 0; i < Mathf.Abs(amountToChange); i++)
			{
				crateOpeningUI.SelectedCrates.Remove(crate.Prefab);
			}
		}
	}
	
	public void SwitchSelectionMode(bool toMultiSelection)
	{
		if (toMultiSelection)
		{
			if (showingStats) ShowStats();

			multiselectPanel.SetActive(true);

			spawnButton.gameObject.SetActive(false);
			showStatsButton.gameObject.SetActive(false);
			counter.text = "0x";
		}
		else
		{
			selectedOverlay.SetActive(true);
			multiselectPanel.SetActive(false);

			spawnButton.gameObject.SetActive(true);
			showStatsButton.gameObject.SetActive(true);
			counter.text = Player.Instance.Inventory.
				GetAmountInInventory(crate.Name) + "x";
		}
	}

	private void ShowStats()
	{
		if (!showingStats)
		{
			spawnButton.gameObject.SetActive(false);
			crateViewImage.gameObject.SetActive(false);
			counter.gameObject.SetActive(false);

			showStatsButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "back";
			statsPanel.SetActive(true);
			showingStats = true;
		}
		else
		{
			spawnButton.gameObject.SetActive(true);
			crateViewImage.gameObject.SetActive(true);
			counter.gameObject.SetActive(true);

			showStatsButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "stats";
			statsPanel.SetActive(false);
			showingStats = false;
		}
	}
}
