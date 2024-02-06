using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialDrop
{
	public Material Material;
	public int Amount;
	public float Probability;
}

[CreateAssetMenu(fileName = "New Scrap", menuName = "Inventory System/Item/Scrap")]
[System.Serializable]
public class Scrap : Item
{
	[SerializeField] private MaterialDrop[] MaterialDrops;
	private MaterialDrop materialDrop;

	public struct RandomMaterialSelection
	{
		public float Probability;
		private readonly MaterialDrop materialDrop;

		public RandomMaterialSelection(MaterialDrop materialDrop, float probability)
		{
			this.materialDrop = materialDrop;
			this.Probability = probability;
		}

		public MaterialDrop GetMaterialDrop() { return materialDrop; }
	}

	private MaterialDrop GetRandomMaterial(params RandomMaterialSelection[] selections)
	{
		float currentProb = 0;
		foreach (var selection in selections)
		{
			currentProb += selection.Probability;
			if (UnityEngine.Random.value <= currentProb)
				return selection.GetMaterialDrop();
		}
		return null;
	}

	public void CalculateSalvageResult()
	{
		if (MaterialDrops.Length > 0)
		{
			RandomMaterialSelection[] materialSelections = new RandomMaterialSelection[MaterialDrops.Length];

			for (int i = 0; i < MaterialDrops.Length; i++)
			{
				if (MaterialDrops[i].Material == null)
				{
					materialSelections[i] =
					new RandomMaterialSelection(
					null,
					MaterialDrops[i].Probability
					);
				}
				else
				{
					materialSelections[i] =
					new RandomMaterialSelection(
					MaterialDrops[i],
					MaterialDrops[i].Probability
					);
				}
			}

			materialDrop = GetRandomMaterial(materialSelections);
		}
	}
}