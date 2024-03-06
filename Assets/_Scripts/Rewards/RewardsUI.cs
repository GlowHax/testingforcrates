using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsUI : oldUIManager
{
	[SerializeField] Crate freeCrate;
	[SerializeField] Button freeCrateClaimButton;
	[SerializeField] Button dailyCrateClaimButton;
	[SerializeField] TMPro.TMP_Text freeCrateClaimText;
	[SerializeField] TMPro.TMP_Text dailyCrateClaimText;
	[SerializeField] RewardTimer freeCrateTimer;
	[SerializeField] RewardTimer dailyCrateTimer;

	private void Update()
	{
		//ManageLevelHUD();
		UpdateRewardButtons();
	}

	void UpdateRewardButtons() 
	{
		if (freeCrateTimer.TimerComplete)
		{
			freeCrateClaimButton.interactable = true;
			freeCrateClaimText.text = "Claim free crate!";
		}
		else
		{
			freeCrateClaimButton.interactable = false;
			freeCrateClaimText.text = "Not ready...";
			freeCrateTimer.TimerComplete = false;
		}

		if (dailyCrateTimer.TimerComplete)
		{
			dailyCrateClaimButton.interactable = true;
			dailyCrateClaimText.text = "Claim daily crate!";
		}
		else
		{
			dailyCrateClaimButton.interactable = false;
			dailyCrateClaimText.text = "Not ready...";
		}
	}

	public void ClaimFreeCrate()
	{
		freeCrateTimer.ResetTimer();
		Player.Instance.Inventory.AddItem("Free Crate", 1);
		freeCrateClaimButton.interactable = false;
		freeCrateClaimText.text = "Not ready...";
	}

	public void ClaimDailyCrate()
	{
		int i = Player.Instance.Inventory.AddItem("Daily Crate", 1);
		if (i > 0)
			return;

		dailyCrateTimer.ResetTimer();
		dailyCrateClaimButton.interactable = false;
		dailyCrateClaimText.text = "Not ready...";
		dailyCrateTimer.TimerComplete = false;
	}
}