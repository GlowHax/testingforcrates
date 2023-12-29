using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MaterialSO", menuName = "MaterialSO")]
public class MaterialSO : ScriptableObject
{
	public string Name;
	public int MoneyCost;
	public int AmountInInventory = 0;

    [Multiline(10)]
    public string Description;
    [SerializeField] Sprite Icon;
}
