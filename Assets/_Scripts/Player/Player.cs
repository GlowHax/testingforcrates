using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	private static Player instance;
	public static Player Instance { get { return instance; } }

	public Camera Cam;
	public Interactor interactor;
	public CharacterMovement characterMovement;
	public MouseLook mouseLook;
	public float Coins = 500;
	public int Level = 1;
	public int XP = 0;
	public Inventory Inventory;
	public List<GameObject> CratePrefabs = new List<GameObject>();

	public float ComboBonus = 0f;

	[HideInInspector] public Tool equippedTool;

	#region level reward vars
	public int[] XPToNextLvl = { 50, 100, 200, 500, 750, 1000, 1500, 2000, 3000 };
	public int[] LevelRewardsMoney = { 250, 500, 1000, 2000, 2500, 2000, 1000 };
	public string[] LevelRewardsCrates =
	{
			"NoCrate", "NoCrate", "Uncommon Crate", "NoCrate", "Common Crate",
			"Uncommon Crate", "NoCrate", "NoCrate", "Rare Crate"
	};
	#endregion

	private void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			SelectEquippedTool(!equippedTool.Prefab.activeSelf);
		}
	}

	public void FPMovement(bool value)
	{
		Player.Instance.characterMovement.Active = value;
	}

	public void FPMouse(bool value)
	{
		Player.Instance.mouseLook.Active = value;
	}

	private void SelectEquippedTool(bool select)
	{
		equippedTool.Prefab.SetActive(select);
	}

	public void AddXPToPlayer(int amount)
	{
		XP += amount;
		//Quests.XPQuestCounter += amount;

		while (XP >= XPToNextLvl[Level - 1])
		{
			XP -= XPToNextLvl[Level - 1];
			Level++;
			//ShowLevelRewardScreen(Level);
		}
	}

	public float GetLevelProgressPercentage()
	{
		return (float)XP / (float)XPToNextLvl[Level - 1];
	}
}
