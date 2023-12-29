using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowBar : Item
{
	[SerializeField] private int breakingPower = 4;
	[SerializeField] private float maxCrateBreakDistance = 3f;
	private IBreakable crate;

	private void Update()
	{
		if (!Equipped) return;

		if (Input.GetMouseButtonDown(0))
		{
			UseItem();
		}
	}

	protected override void UseItem()
	{
		base.UseItem();
		if (animator.GetBool("UsingTool")) return;

		animator.SetBool("UsingTool", true);
		Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
		Debug.DrawRay(ray.origin, ray.direction * maxCrateBreakDistance);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, maxCrateBreakDistance, LayerMask.GetMask("Crate")))
		{
			crate = hitInfo.transform.GetComponent<IBreakable>();

			if (crate != null) crate.TakeDamage(breakingPower);
		}
	}
}
