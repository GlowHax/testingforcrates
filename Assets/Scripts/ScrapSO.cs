using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scrap", menuName = "Scrap")]
public class ScrapSO : ScriptableObject
{
    public string Name;
    public int AmountInInventory = 0;

	[Multiline(10)] public string Description;
    public Sprite Icon;

	[HideInInspector] public MaterialDrop MaterialDrop;

	[SerializeField] MaterialDrop[] materialDrops;

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

	public void DropSalvageResult()
	{
		CalculateSalvageResult();

		//add the calculated result to users inventory
		MaterialDrop.Material.AmountInInventory += MaterialDrop.Amount;
	}

	private void CalculateSalvageResult()
	{
		if (materialDrops.Length > 0)
		{
			RandomMaterialSelection[] materialSelections = new RandomMaterialSelection[materialDrops.Length];

			for (int i = 0; i < materialDrops.Length; i++)
			{
				if (materialDrops[i].Material == null)
				{
					materialSelections[i] =
					new RandomMaterialSelection(
					null,
					materialDrops[i].Probability
					);
				}
				else
				{
					materialSelections[i] =
					new RandomMaterialSelection(
					materialDrops[i],
					materialDrops[i].Probability
					);
				}
			}

			MaterialDrop = GetRandomMaterial(materialSelections);
		}
	}
}

[System.Serializable]
public class MaterialDrop
{
	public MaterialSO Material;
	public int Amount;
	public float Probability;
}