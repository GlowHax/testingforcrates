using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ResultSection
{
	public GameObject Section;
	public Image Icon;
	public TMP_Text AmountText;
}

public class CrateOpeningTerminal : Terminal
{
	[HideInInspector] public List<CrateBehaviour> SelectedCrates = new List<CrateBehaviour>();

	[SerializeField] private CrateOpeningManager crateOpeningManager;
	[SerializeField] private GameObject openingResultsPanel;
	[SerializeField] private GameObject selectionScrollContent;
	[SerializeField] private CrateOpeningUITemplate selectionViewTemplate;
	[SerializeField] private GameObject multiSelectionSpawnButton;
	[SerializeField] private GameObject noCratesNotification;
	[SerializeField] private TMP_Text openingStateNotification;

	private int totalMoneyDrop = 0;
	private int totalXPDrop = 0;
	private List<int> totalMaterialDropAmounts = new List<int>();
	private List<string> totalMaterialDropNames = new List<string>();
	private bool resultsActive = false;
	private bool multiSelection = false;

	private void Update()
	{
		if (crateOpeningManager.OpeningFinished)
		{
			SelectedCrates.Clear();
			RefreshSelection();
			SetInteractable(true);
			crateOpeningManager.OpeningFinished = false;
		}
		else if(isOn)
        {
			if (SelectedCrates.Count != 0 && !multiSelectionSpawnButton.activeInHierarchy)
			{
				multiSelectionSpawnButton.SetActive(true);
			}
			else if (SelectedCrates.Count == 0 && multiSelectionSpawnButton.activeInHierarchy)
			{
				multiSelectionSpawnButton.SetActive(false);
			}
		}
	}

	public override void SwitchOn()
	{
		base.SwitchOn();
		RefreshSelection();
	}

	public void SwitchSelectionMode()
	{
		if (!multiSelection)
		{
			multiSelection = true;

			for (int i = 0; i < selectionScrollContent.transform.childCount; i++)
			{
				CrateOpeningUITemplate contentElement =
					selectionScrollContent.transform.GetChild(i).GetComponent<CrateOpeningUITemplate>();

				if (contentElement != null) contentElement.SwitchSelectionMode(true);
			}
		}
		else
		{
			multiSelection = false;
			SelectedCrates.Clear();

			for (int i = 0; i < selectionScrollContent.transform.childCount; i++)
			{
				CrateOpeningUITemplate contentElement =
					selectionScrollContent.transform.GetChild(i).GetComponent<CrateOpeningUITemplate>();

				if (contentElement != null) contentElement.SwitchSelectionMode(false);
			}
		}
	}

	public void RefreshSelection()
	{
		openingStateNotification.gameObject.SetActive(false);
        noCratesNotification.SetActive(false);
		mainPanel.SetActive(true);

		int contentChildCount = selectionScrollContent.transform.childCount;

		for (int i = 0; i < contentChildCount; i++)
		{
			Destroy(selectionScrollContent.transform.GetChild(i).gameObject);
		}

		if(FillSelectionScrollContent() == 0)
		{
			noCratesNotification.SetActive(true);
		}
	}

	private int FillSelectionScrollContent()
	{

		int cratesFound = 0;
		foreach (Crate crate in ResourceSystem.Instance.GetAllItemsOfType<Crate>())
		{
			int amountInInventory = Player.Instance.Inventory.GetAmountInInventory(crate.Name);
			if (amountInInventory > 0)
			{
				CrateOpeningUITemplate crateView = 
				Instantiate(selectionViewTemplate, selectionScrollContent.transform);
				crateView.Initialize(this, crate, amountInInventory);
				cratesFound++;
			}
		}

		return cratesFound;
	}

	public void StartCrateOpening()
	{
		Player.Instance.ComboBonus = 0f;

		SetInteractable(false);
		mainPanel.SetActive(false);
		multiSelection = false;
		openingStateNotification.gameObject.SetActive(true);
		openingStateNotification.text = "spawning...";

		for (int i = 0; i < SelectedCrates.Count; i++)
		{
			crateOpeningManager.UnopenedCrates.Add(SelectedCrates[i]);
		}
		StartCoroutine(crateOpeningManager.SpawnCrates());
	}

	private void SetInteractable(bool interactable)
	{
		if (interactable)
		{
			interactionPoint.gameObject.layer = LayerMask.NameToLayer("Interactable");
		}
		else
		{
			interactionPoint.gameObject.layer = LayerMask.NameToLayer("Default");
		}
	}

	public void GoToCrateSelectionMenu()
	{
		StopAllCoroutines();
		RefreshSelection();
		mainPanel.SetActive(true);
		openingResultsPanel.SetActive(false);
		resultsActive = false;
		totalMoneyDrop = 0;
		totalXPDrop = 0;
		totalMaterialDropNames.Clear();
		totalMaterialDropAmounts.Clear();
	}

	private void AddDropsToResults(GameObject crate)
	{
		//totalMoneyDrop += crate.GetComponent<CrateBehaviour>().CrateType.MoneyDrop;
		//totalXPDrop += crate.GetComponent<CrateBehaviour>().CrateType.XPDrop;

		if (crate.GetComponent<CrateBehaviour>().Crate.ScrapDropName != "NoMaterial")
		{
			if (totalMaterialDropNames.Contains(crate.GetComponent<CrateBehaviour>().Crate.ScrapDropName))
			{
				int materialIndex = totalMaterialDropNames.IndexOf(crate.GetComponent<CrateBehaviour>().Crate.ScrapDropName);
				totalMaterialDropAmounts[materialIndex] += crate.GetComponent<CrateBehaviour>().Crate.ScrapDropAmount;
			}
			else
			{
				totalMaterialDropNames.Add(crate.GetComponent<CrateBehaviour>().Crate.ScrapDropName);
				totalMaterialDropAmounts.Add(crate.GetComponent<CrateBehaviour>().Crate.ScrapDropAmount);
			}
		}
	}

}