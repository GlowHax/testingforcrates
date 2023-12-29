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

public class Item : MonoBehaviour
{
	public bool Equipped = false;

	[SerializeField] protected string m_name;
	[SerializeField] protected ItemTypes type;
	[SerializeField] protected int amountInInventory;
	[SerializeField] protected List<CraftingMaterial> CraftingRecipe;
	[SerializeField] protected Animator animator;

	protected Camera playerCam;

	private void Start()
	{
		playerCam = transform.parent.parent.GetComponent<Camera>();
	}

	protected virtual void UseItem()
	{

	}
}