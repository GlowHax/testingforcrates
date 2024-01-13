using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsUI : UIManager
{
	[SerializeField] CrateSO freeCrate;
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
		Player.Instance.OldInventory.Crates.Find(x => freeCrate).AddToInventory(1);
		freeCrateClaimButton.interactable = false;
		freeCrateClaimText.text = "Not ready...";
	}

	public void ClaimDailyCrate()
	{
		dailyCrateTimer.ResetTimer();
		for(int i = 0; i < Player.Instance.OldInventory.Crates.Count; i++)
		{
			if (Player.Instance.OldInventory.Crates[i].Type == CrateType.Daily)
			{
				Player.Instance.OldInventory.Crates[i].AddToInventory(1);
			}
		}
		dailyCrateClaimButton.interactable = false;
		dailyCrateClaimText.text = "Not ready...";
		dailyCrateTimer.TimerComplete = false;
	}
}