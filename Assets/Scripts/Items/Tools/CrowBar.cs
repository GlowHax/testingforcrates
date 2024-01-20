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
		if (animator == null)
			return;
		if (animator.GetBool("UsingTool"))
			return;

		animator.SetBool("UsingTool", true);
		Ray ray = new Ray(Player.Instance.Cam.transform.position, Player.Instance.Cam.transform.forward);
		Debug.DrawRay(ray.origin, ray.direction * inventoryItem.MaxUseDistance);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, inventoryItem.MaxUseDistance, LayerMask.GetMask("Hittable")))
		{
			IHittable hittableObject = hitInfo.transform.GetComponent<IHittable>();
			hittableObject.TakeHit(inventoryItem.Power);
			UpdateDurability();
		}
	}

	public void UpdateDurability()
	{
		inventoryItem.Durability--;
		if (inventoryItem.Durability == 0)
		{
			Player.Instance.Inventory.ClearSlot(inventoryItemPanel.ItemSlot);
			Destroy(gameObject);
		}
	}
}