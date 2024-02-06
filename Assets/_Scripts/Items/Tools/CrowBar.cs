using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CrowBar : MonoBehaviour
{
	[SerializeField] private Animator animator;
	private ItemPanel inventoryItemPanel;
	private Tool inventoryItem;

	private void Start()
	{
		inventoryItemPanel = Player.Instance.Inventory.EquippedToolItemPanel;
		inventoryItem = inventoryItemPanel.ItemSlot.Item as Tool;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Use();
		}
	}

	public void Use()
	{
		if (animator.GetBool("UsingTool") || Player.Instance.Inventory.InventoryMenu.activeSelf)
			return;

		animator.SetBool("UsingTool", true);
		Ray ray = new Ray(Player.Instance.Cam.transform.position, 
			Player.Instance.Cam.transform.forward);
		Debug.DrawRay(ray.origin, ray.direction * inventoryItem.MaxUseDistance);

		if (Physics.Raycast(ray, out RaycastHit hitInfo, inventoryItem.MaxUseDistance,
			LayerMask.NameToLayer("Hittable")) &&
			hitInfo.transform.TryGetComponent(out IHittable hittableObject))
		{
			hittableObject.TakeHit(inventoryItem.Power);
			UpdateDurability();
		}
	}

	public void UpdateDurability()
	{
		inventoryItemPanel.ItemSlot.Durability--;
		if (inventoryItemPanel.ItemSlot.Durability == 0)
		{
			Player.Instance.Inventory.ClearSlot(inventoryItemPanel.ItemSlot);
			Destroy(gameObject);
		}
	}
}