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

public abstract class Tool : Item
{
	public bool Equipped = false;

	[SerializeField] protected string m_name;
	[SerializeField] protected ItemTypes type;
	[SerializeField] protected int amountInInventory;
	[SerializeField] protected List<CraftingMaterial> CraftingRecipe;
	[SerializeField] protected Animator animator;

	protected Camera playerCam;

	public override int MaxStacks()
	{
		return 5;
	}

	public virtual float MaxReachDistance()
	{
		return 3f;
	}

	public virtual void Use()
	{

	}
}