using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrateOpeningManager : MonoBehaviour
{
	[HideInInspector] public List<CrateBehaviour> CrateComboQueue = new List<CrateBehaviour>();
	[HideInInspector] public List<CrateBehaviour> OpenedCrates = new List<CrateBehaviour>();
	[HideInInspector] public List<GameObject> UnopenedCrates = new List<GameObject>();
	[HideInInspector] public bool OpeningFinished = false;

	[SerializeField] private ComboTimer[] comboTimers;

	[SerializeField] Transform openingArea;

	[SerializeField] ResultSection moneyResult;
	[SerializeField] ResultSection xpResult;
	[SerializeField] ResultSection scrapResult;
	[SerializeField] float crateSpawnheight = 30f;

	private void Update()
	{
		if (CrateComboQueue.Count > 1)
		{
			CheckSimpleCombo();
		}

		if (UnopenedCrates.Count > 0 && (OpenedCrates.Count + CrateComboQueue.Count) == UnopenedCrates.Count)
		{
			FinishCrateOpening();
		}
	}

	private void CheckSimpleCombo()
	{
		if (CrateComboQueue[0].CrateType == CrateComboQueue[1].CrateType)
		{
			Player.Instance.ComboBonus += 0.01f;
			//round to 0.00 to overwrite precision aberration
			Math.Round((decimal)Player.Instance.ComboBonus, 2, MidpointRounding.AwayFromZero);
			foreach(ComboTimer timer in comboTimers)
			{
				timer.StartTimer(new Color(1f, 0.64f, 0.2f), 5f);
				timer.transform.parent.gameObject.SetActive(true);
			}
		}
		OpenedCrates.Add(CrateComboQueue[0]);
		CrateComboQueue.Remove(CrateComboQueue[0]);
	}

	private void FinishCrateOpening()
	{
		OpeningFinished = true;
		CrateComboQueue.Clear();
		UnopenedCrates.Clear();
		OpenedCrates.Clear();
		StopAllCoroutines();
	}

	public IEnumerator SpawnCrates()
	{
		for (int i = 0; i < UnopenedCrates.Count; i++)
		{
			GameObject crate =
				Instantiate(UnopenedCrates[i].gameObject,
				openingArea.position + new Vector3(0f, crateSpawnheight, 0f),
				Quaternion.identity, openingArea);
			crate.GetComponent<CrateBehaviour>().CrateOpeningManager = this;
			yield return new WaitForSeconds(1.2f);
		}
		yield return null;
	}

	//IEnumerator ShowResults(float delay)
	//{
	//	// reset
	//	moneyResult.Section.SetActive(false);
	//	moneyResult.AmountText.text = "";
	//	xpResult.Section.SetActive(false);
	//	xpResult.AmountText.text = "";
	//	scrapResult.Section.SetActive(false);
	//	scrapResult.AmountText.text = "";

	//	yield return new WaitForSeconds(delay);
	//	openingResultsPanel.SetActive(true);
	//	yield return new WaitForSeconds(0.5f);

	//	moneyResult.Section.SetActive(true);
	//	CountToNumber moneyCounter = new CountToNumber(0f, 0.5f, moneyResult.AmountText);
	//	StartCoroutine(moneyCounter.CountTo(totalMoneyDrop));
	//	yield return new WaitForSeconds(0.8f);

	//	xpResult.Section.SetActive(true);
	//	CountToNumber xpCounter = new CountToNumber(0f, 0.5f, xpResult.AmountText);
	//	StartCoroutine(xpCounter.CountTo(totalXPDrop));
	//	yield return new WaitForSeconds(0.8f);

	//	for (int i = 0; i < totalMaterialDropNames.Count; i++)
	//	{
	//		if(totalMaterialDropNames[i] != "")
	//		{
	//			scrapResult.AmountText.text += $"{totalMaterialDropAmounts[i]}x {totalMaterialDropNames[i]}\n";
	//			scrapResult.Section.SetActive(true);
	//			yield return new WaitForSeconds(0.3f);
	//		}
	//	}

	//	yield return null;
	//}
}
