using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrateType
{
	Main, Material, Daily
}

[System.Serializable]
public class ScrapDrop
{
	public string ScrapName;
	public int Amount;
	public float Probability;
}

[System.Serializable]
public class MaterialCost
{
	public MaterialSO Material;
	public int Amount;
}

[CreateAssetMenu(fileName = "New Crate", menuName = "Crate")]
public class CrateSO : ScriptableObject
{
	public string Name;
	public int UnlockLvl;
	public int MoneyCost;
	public CrateType Type;
	public MaterialCost[] MaterialCosts;
	public float Health;
	public int AmountInInventory = 0;

	public static bool FreeRedeemable;
	public static int MinsLeftForFreeCrate = 0;
	public static System.Timers.Timer FreeCrateTimer = new System.Timers.Timer(60000);

	//Drops
	[Multiline(10)] public string Stats;
	[HideInInspector] public int MoneyDrop;
	[HideInInspector] public int XPDrop;
	[HideInInspector] public string ScrapDropName;
	[HideInInspector] public int ScrapDropAmount;

	[SerializeField] int[] moneyDrops;
	[SerializeField] float[] moneyDropOdds;
	[SerializeField] int[] xpDrops;
	[SerializeField] float[] xpDropOdds;
	[SerializeField] ScrapDrop[] scrapDrops;

	public struct RandomSelection
	{
		private readonly int amount;
		public float probability;

		public RandomSelection(int amount, float probability)
		{
			this.amount = amount;
			this.probability = probability;
		}

		public int GetValue() { return amount; }
	}

	public struct RandomScrapSelection
	{
		private readonly int amount;
		private readonly string scrapName;
		public float probability;

		public RandomScrapSelection(string scrapName, int amount, float probability)
		{
			this.scrapName = scrapName;
			this.amount = amount;
			this.probability = probability;
		}

		public int GetAmount() { return amount; }
		public string GetScrapName() { return scrapName; }
	}

	private int GetRandomValue(params RandomSelection[] selections)
	{
		float currentProb = 0;
		foreach (var selection in selections)
		{
			currentProb += selection.probability;
			if (UnityEngine.Random.value <= currentProb)
				return selection.GetValue();
		}
		return -1;
	}

	private string[] GetRandomScrap(params RandomScrapSelection[] selections)
	{
		float currentProb = 0;
		foreach (var selection in selections)
		{
			currentProb += selection.probability;
			if (UnityEngine.Random.value <= currentProb)
				return new string[] { selection.GetScrapName(), selection.GetAmount().ToString() };
		}
		return null;
	}

	public void Purchase()
	{
		if (Player.Instance.Coins >= MoneyCost)
		{
			if (MaterialCosts.Length == 0)
			{
				Player.Instance.Coins -= MoneyCost;
				AddToInventory(1);
				return;
			}

			int affortableCosts = 0;
			for (int i = 0; i < MaterialCosts.Length; i++)
			{
				if (Player.Instance.OldInventory.Materials.Find(x => MaterialCosts[i].Material).AmountInInventory >= MaterialCosts[i].Amount)
				{
					affortableCosts++;
				}
			}

			if (affortableCosts == MaterialCosts.Length)
			{
				for (int i = 0; i < MaterialCosts.Length; i++)
				{
					Player.Instance.Coins -= MoneyCost;
					Player.Instance.OldInventory.Materials.Find(x => MaterialCosts[i].Material).AmountInInventory -= MaterialCosts[i].Amount;
					AddToInventory(1);
				}
			}
		}
	}

	public void AddToInventory(int amount)
	{
		AmountInInventory += amount;
	}

	public void OpenCrate()
	{
		CalcutlateDrops();
		AmountInInventory--;
		Player.Instance.Coins += MoneyDrop;
		Player.Instance.AddXPToUser(XPDrop);
	}

	private void CalcutlateDrops()
	{
		//calculate moneydrop
		if (moneyDrops != null)
		{
			RandomSelection[] moneySelections = new RandomSelection[moneyDrops.Length];

			for (int i = 0; i < moneySelections.Length; i++)
			{
				moneySelections[i] = new RandomSelection(moneyDrops[i], moneyDropOdds[i]);
			}
			MoneyDrop = GetRandomValue(moneySelections);
			MoneyDrop += (int)(MoneyDrop * Player.Instance.ComboBonus);
		}

		//calculate xpdrop
		if (xpDrops != null)
		{
			RandomSelection[] xpSelections = new RandomSelection[xpDrops.Length];

			for (int i = 0; i < xpSelections.Length; i++)
			{
				xpSelections[i] = new RandomSelection(xpDrops[i], xpDropOdds[i]);
			}
			XPDrop = GetRandomValue(xpSelections);
		}

		//calculate scrapdrop
		if (scrapDrops.Length > 0)
		{
			RandomScrapSelection[] scrapSelections = new RandomScrapSelection[scrapDrops.Length];

			for (int i = 0; i < scrapDrops.Length; i++)
			{
				if(scrapDrops[i].ScrapName == null)
				{
					scrapSelections[i] =
					new RandomScrapSelection(
					"",
					scrapDrops[i].Amount,
					scrapDrops[i].Probability
					);
				}
				else
				{
					scrapSelections[i] =
					new RandomScrapSelection(
					scrapDrops[i].ScrapName,
					scrapDrops[i].Amount,
					scrapDrops[i].Probability
					);
				}
			}

			string[] materialDrop = GetRandomScrap(scrapSelections);
			ScrapDropName = materialDrop[0];
			ScrapDropAmount = int.Parse(materialDrop[1]);
		}
	}
}