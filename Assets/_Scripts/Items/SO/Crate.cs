using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum CrateType
{
	Main, Material, Daily
}

[Serializable]
public struct ScrapDrop
{
	public string ScrapName;
	public int Amount;
	public float Probability;
}

[Serializable]
public struct MaterialCost
{
	public Material Material;
	public int Amount;
}

[CreateAssetMenu(fileName = "New Crate", menuName = "Inventory System/Item/Crate")]
[System.Serializable]
public class Crate : Item
{
	public CrateType Type;
	public int UnlockLvl;
	public MaterialCost[] MaterialCosts;
	public float Health;
    [Multiline(10)] public string Stats;

    public static bool FreeRedeemable;
	public static int MinsLeftForFreeCrate = 0;
	public static System.Timers.Timer FreeCrateTimer = new System.Timers.Timer(60000);

    public CrateBehaviour Prefab { get; private set; }

    //Drops
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

	public void OpenCrate()
	{
		CalcutlateDrops();
		Player.Instance.Inventory.RemoveItem(Name, 1);
		Player.Instance.Coins += MoneyDrop;
		Player.Instance.AddXPToPlayer(XPDrop);
	}

	private void CalcutlateDrops()
	{
		//calculate moneydrop
		if (moneyDrops != null)
		{
			RandomSelection[] moneySelections = new RandomSelection[moneyDrops.Length];

			for (int i = 0; i < moneyDrops.Length; i++)
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
                scrapSelections[i] =
                    new RandomScrapSelection(
                    scrapDrops[i].ScrapName,
                    scrapDrops[i].Amount,
                    scrapDrops[i].Probability
                    );
            }

			string[] scrapDrop = GetRandomScrap(scrapSelections);
			ScrapDropName = scrapDrop[0];
			ScrapDropAmount = int.Parse(scrapDrop[1]);
		}
	}
}