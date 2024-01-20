using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum ItemTypes
{
	Tool,
	Consumable,
	Passive
}

[System.Serializable]
public class CraftingMaterial
{
	public MaterialSO Material;
	public int Amount;
}

public interface ITool
{
	//public List<CraftingMaterial> GetCraftingRecipe();

	public abstract GameObject SpawnToolObject();

	public void Use(Animator animator = null, ItemSlotInfo itemSlot = null);
	public void UpdateDurability(ItemSlotInfo itemSlot);
}