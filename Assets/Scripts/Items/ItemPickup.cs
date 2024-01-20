using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
	public string itemToDrop;
	public int amount = 1;
	public Image displayImage;
	public TextMeshProUGUI ItemStacksText;
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			PickUpItem();
		}
	}

	public void PickUpItem()
	{
		amount = Player.Instance.Inventory.AddItem(itemToDrop, amount);

		if(amount < 1)
		{
			Destroy(this.transform.root.gameObject);
		}
	}
}