using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class CrateOpeningUITemplate : MonoBehaviour
{
	public GameObject CratePrefab;
	public CrateSO CrateType;
	public CrateOpeningTerminal CrateOpeningUI;
	public TMP_Text Counter;

	[SerializeField] TMP_Text crateViewName;
	[SerializeField] Image crateViewImage;

	[SerializeField] Button spawnButton;
	[SerializeField] Button showStatsButton;

	[SerializeField] GameObject statsPanel;
	[SerializeField] TMP_Text statsText;

	[SerializeField] GameObject multiselectPanel;
	[SerializeField] GameObject selectedOverlay;

	private bool showingStats = false;

	public void Setup()
	{
		gameObject.name = CrateType.Name;
		statsPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = CrateType.Stats;

		crateViewImage.sprite = CratePrefab.GetComponent<CrateBehaviour>().CrateSprite;
		crateViewName.text = CrateType.Name;
		spawnButton.onClick.RemoveAllListeners();
		spawnButton.onClick.AddListener(() => SingleSelect());
		spawnButton.onClick.AddListener(() => CrateOpeningUI.StartCrateOpening());
		showStatsButton.onClick.AddListener(() => ShowStats());
	}

	public void SingleSelect()
	{
		CrateOpeningUI.SelectedCrates.Add(CratePrefab);
	}

	public void ChangeSelectionAmount(int amountToChange)
	{
		List<char> count = Counter.text.ToCharArray().ToList();
		count.Remove('x');

		int selectedAmount = int.Parse(new string(count.ToArray()));
		if ((selectedAmount + amountToChange) > CrateType.AmountInInventory)
		{
			return;
		}
		else if ((selectedAmount + amountToChange) <= 0)
		{
			CrateOpeningUI.SelectedCrates.Remove(CratePrefab);
			Counter.text = "0x";
			selectedOverlay.SetActive(true);
			return;
		}
		else
		{
			Counter.text = (selectedAmount + amountToChange).ToString() + "x";
		}

		if (amountToChange > 0)
		{
			if(selectedOverlay.activeInHierarchy) selectedOverlay.SetActive(false);

			for (int i = 0; i < amountToChange; i++)
			{
				CrateOpeningUI.SelectedCrates.Add(CratePrefab);
			}
		}
		else if(amountToChange < 0)
		{
			for (int i = 0; i < Mathf.Abs(amountToChange); i++)
			{
				CrateOpeningUI.SelectedCrates.Remove(CratePrefab);
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
			Counter.text = "0x";
		} 
		else
		{ 
			selectedOverlay.SetActive(true);
			multiselectPanel.SetActive(false);

			spawnButton.gameObject.SetActive(true);
			showStatsButton.gameObject.SetActive(true);
			Counter.text = CrateType.AmountInInventory.ToString() + "x";
		}	
	}

	private void ShowStats()
	{
		if (!showingStats)
		{
			spawnButton.gameObject.SetActive(false);
			crateViewImage.gameObject.SetActive(false);
			Counter.gameObject.SetActive(false);

			showStatsButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "back";
			statsPanel.SetActive(true);
			showingStats = true;
		}
		else
		{
			spawnButton.gameObject.SetActive(true);
			crateViewImage.gameObject.SetActive(true);
			Counter.gameObject.SetActive(true);

			showStatsButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "stats";
			statsPanel.SetActive(false);
			showingStats = false;
		}
	}
}
