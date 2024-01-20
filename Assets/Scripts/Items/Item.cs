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

	[HideInInspector] public Sprite Sprite;
    public virtual void LoadData(List<AssetBundle> assetBundles)
    {
		Sprite = LoadSprite(Name + "ItemSprite", assetBundles[0]);
    }

	public GameObject LoadObject(string objectNameToLoad, AssetBundle assetBundle)
	{
		//Load Asset
		AssetBundleRequest assetRequest = assetBundle.LoadAssetAsync<GameObject>(objectNameToLoad);
		GameObject loadedObject = assetRequest.asset as GameObject;
		Debug.Log(loadedObject.name + "Prefab loaded");
		return loadedObject;
	}

	public Sprite LoadSprite(string spriteNameToLoad, AssetBundle assetBundle)
	{
		//Load Asset
		AssetBundleRequest assetRequest = assetBundle.LoadAssetAsync<Sprite>(spriteNameToLoad);
		Sprite loadedSprite = assetRequest.asset as Sprite;
		Debug.Log(loadedSprite.name + " loaded");
		return loadedSprite;
	}
}