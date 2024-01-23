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
	[Multiline(10)]
	public string Description;

	[HideInInspector] public Sprite Sprite;
    public virtual void LoadData(AssetBundle assetBundle)
    {
		Sprite = LoadSprite(assetBundle);
    }

	public GameObject LoadObject(string objectNameToLoad, AssetBundle assetBundle)
	{
		//Load Asset
		AssetBundleRequest assetRequest = assetBundle.LoadAssetAsync<GameObject>(objectNameToLoad);
		GameObject loadedObject = assetRequest.asset as GameObject;
		return loadedObject;
	}

	public Sprite LoadSprite(AssetBundle assetBundle)
	{
		//Load Asset
		AssetBundleRequest assetRequest = assetBundle.
			LoadAssetAsync<Sprite>(Name + "ItemSprite");
		Sprite loadedSprite = assetRequest.asset as Sprite;
		return loadedSprite;
	}
}