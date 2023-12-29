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
	[HideInInspector] public List<GameObject> SelectedCrates = new List<GameObject>();

	[SerializeField] private CrateOpeningManager crateOpeningManager;
	[SerializeField] private GameObject openingResultsPanel;
	[SerializeField] private GameObject selectionScrollContent;
	[SerializeField] private GameObject selectionViewTemplate;
	[SerializeField] private GameObject multiSelectionSpawnButton;
	[SerializeField] private GameObject noCratesNotification;
	[SerializeField] private TMP_Text openingStateNotification;

	private int totalMoneyDrop = 0;
	private int totalXPDrop = 0;
	private List<int> totalMaterialDropAmounts = new List<int>();
	private List<string> totalMaterialDropNames = new List<string>();
	private bool resultsActive = false;
	private bool multiSelection = false;

	private void Start()
	{
		RefreshSelection();
	}

	private void Update()
	{
		if (crateOpeningManager.OpeningFinished)
		{
			SelectedCrates.Clear();
			RefreshSelection();
			SetInteractable(true);
			crateOpeningManager.OpeningFinished = false;
		}
		else if (switchedOn)
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
		mainPanel.SetActive(true);

		int contentChildCount = selectionScrollContent.transform.childCount;

		for (int i = 0; i < contentChildCount; i++)
		{
			Destroy(selectionScrollContent.transform.GetChild(i).gameObject);
		}

		FillSelectionScrollContent();
	}

	private void FillSelectionScrollContent()
	{
		int cratesInTotal = 0;

		for (int i = 0; i < User.Instance.Inventory.Crates.Count; i++)
		{
			GameObject cratePrefab = User.Instance.CratePrefabs[i];

			if (User.Instance.Inventory.Crates[i].AmountInInventory > 0)
			{
				cratesInTotal++;

				CrateOpeningUITemplate crateView =
					Instantiate(selectionViewTemplate, selectionScrollContent.transform).GetComponent<CrateOpeningUITemplate>();
				crateView.transform.SetSiblingIndex(i);

				crateView.CrateOpeningUI = this;
				crateView.CratePrefab = cratePrefab;
				crateView.CrateType = User.Instance.Inventory.Crates[i];
				crateView.Counter.text = User.Instance.Inventory.Crates[i].AmountInInventory.ToString() + "x";

				crateView.Setup();
			}
		}

		if (cratesInTotal == 0)
			noCratesNotification.SetActive(true);
		else
			noCratesNotification.SetActive(false);
	}

	public void StartCrateOpening()
	{
		User.Instance.ComboBonus = 0f;

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
			interactionPoint.gameObject.layer = 11; // 14 = Interactable
		}
		else
		{
			interactionPoint.gameObject.layer = 0; // 0 = Default
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

		if (crate.GetComponent<CrateBehaviour>().CrateType.ScrapDropName != "NoMaterial")
		{
			if (totalMaterialDropNames.Contains(crate.GetComponent<CrateBehaviour>().CrateType.ScrapDropName))
			{
				int materialIndex = totalMaterialDropNames.IndexOf(crate.GetComponent<CrateBehaviour>().CrateType.ScrapDropName);
				totalMaterialDropAmounts[materialIndex] += crate.GetComponent<CrateBehaviour>().CrateType.ScrapDropAmount;
			}
			else
			{
				totalMaterialDropNames.Add(crate.GetComponent<CrateBehaviour>().CrateType.ScrapDropName);
				totalMaterialDropAmounts.Add(crate.GetComponent<CrateBehaviour>().CrateType.ScrapDropAmount);
			}
		}
	}

}
