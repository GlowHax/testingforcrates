using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory System/Item/Tool")]
[System.Serializable]
public class Tool : Item
{
	public int Power;
	public float Durability;
	[HideInInspector] public float MaxDurability;
	public int MaxUseDistance;

	[HideInInspector] public GameObject Prefab;

	public override void LoadData(AssetBundle assetBundle)
	{
		base.LoadData(assetBundle);

		Prefab = LoadObject(Name, assetBundle);
		MaxDurability = Durability;
	}
}