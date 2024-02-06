using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CraftingMaterial
{
	public Material Material;
	public int Amount;
}

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory System/Item/Tool")]
[System.Serializable]
public class Tool : Item
{
	public int Power;
	public float Durability;
	[HideInInspector] public float MaxDurability;
	public List<CraftingMaterial> CraftingRecipe;
	public int MaxUseDistance;

	public GameObject Prefab;
}