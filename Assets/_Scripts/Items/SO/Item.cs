using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
	[Header("Stats")]
	public string Name;
	public int MaxInventoryStacks;
	public int Value;

	[Multiline(10)]
	public string Description;

	public Sprite InventorySprite;
}