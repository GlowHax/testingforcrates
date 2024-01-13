using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkshopUI : UIManager
{
	//LockedPanel
	[SerializeField] GameObject lockedPanel;
	[SerializeField] Button recontsructButton;
	[SerializeField] TMP_Text coinCostText;
	[SerializeField] TMP_Text woodCostText;
	[SerializeField] TMP_Text stoneCostText;

	//SalvagePanel
	[HideInInspector] public bool Salvaging = false;
	[HideInInspector] public ScrapSO SalvagedScrap;
	[SerializeField] GameObject backButton;
	[SerializeField] GameObject salvagePanel;
	[SerializeField] RectTransform salvageMachine;
	[SerializeField] RewardTimer salvageTimer;
	[SerializeField] GameObject scrapScrollContent;
	[SerializeField] GameObject scrapViewTemplate;
	[SerializeField] GameObject salvageResultText;
	private List<ScrapViewTemplate> scrapScrollContentList = new List<ScrapViewTemplate>();

	private void Start()
	{
		woodCostText.text = $"{Player.Instance.OldInventory.Materials[0].AmountInInventory}/5";
		stoneCostText.text = $"{Player.Instance.OldInventory.Materials[0].AmountInInventory}/1";
		FillScrapScrollContent();
	}

	private void Update()
	{
		ManageSalvage();
	}

	private void ManageSalvage()
	{
		if (salvageTimer.TimerComplete && Salvaging)
		{
			EndSalvage();
		}
		else if(!salvageTimer.TimerComplete)
		{
			Salvaging = true;
			salvageResultText.SetActive(false);
		}
	}

	private void EndSalvage()
	{
		SalvagedScrap.DropSalvageResult();

		salvageResultText.GetComponent<TMP_Text>().text = 
			$"+{SalvagedScrap.MaterialDrop.Amount} {SalvagedScrap.MaterialDrop.Material.Name}";
		salvageResultText.SetActive(true);
		salvageMachine.GetComponent<Image>().color = Color.white;
		Salvaging = false;
	}

	void FillScrapScrollContent()
	{
		for (int i = 0; i < Player.Instance.OldInventory.Scraps.Count; i++)
		{
			ScrapSO scrap = Player.Instance.OldInventory.Scraps[i];
			if (scrap.AmountInInventory > 0)
			{
				ScrapViewTemplate scrapView = 
					Instantiate(scrapViewTemplate, scrapScrollContent.transform).GetComponent<ScrapViewTemplate>();
				scrapView.Scrap = scrap;
				scrapView.SalvageMachine = salvageMachine;
				scrapView.ActivePanel = salvagePanel.transform;
				scrapView.UIManager = this;
				scrapView.SetUp();
			}
		}
	}

	public void StartSalvaging()
	{
		if (Salvaging)
			return;

		salvageMachine.GetComponent<Image>().color = Color.gray;
		salvageTimer.ResetTimer();
	}

	void ShowFloatingLootText(string text)
	{
		salvageResultText.GetComponent<TMP_Text>().text = text;
	}
}