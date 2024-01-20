using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory System/Item/Tool")]
[System.Serializable]
public class Tool : Item
{
	public int Durability;
	[HideInInspector] public int MaxDurability;
	public int MaxUseDistance;
	public int Power;

	[HideInInspector] public GameObject Prefab;

	public override void LoadData(List<AssetBundle> assetBundles)
	{
		base.LoadData(assetBundles);

		Prefab = LoadObject(Name, assetBundles[1]);
		MaxDurability = Durability;
	}
}