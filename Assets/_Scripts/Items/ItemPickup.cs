using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
	public Item ItemToDrop;
	public int Amount = 1;
	public Image DisplayImage;
	public TextMeshProUGUI ItemStacksText;
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			PickUpItem();
		}
	}

	private void Start()
	{
		DisplayImage.sprite = ItemToDrop.InventorySprite;
		if(Amount > 1 )
		{
			ItemStacksText.gameObject.SetActive(true);
			ItemStacksText.text = Amount.ToString();
		}
		else
		{
			ItemStacksText.gameObject.SetActive(false);
		}
	}

	public void PickUpItem()
	{
		Amount = Player.Instance.Inventory.AddItem(ItemToDrop.Name, Amount);

		if(Amount < 1)
		{
			Destroy(this.transform.root.gameObject);
		}
	}
}