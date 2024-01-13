using UnityEngine;

public class CrowBar : Tool
{
	public int durability = 12;
	[SerializeField] private int power = 4;

	public override string GetName()
	{
		return "Crowbar";
	}

	public override void Use()
	{
		if (animator.GetBool("UsingTool")) 
			return;

		Ray ray = new Ray(Player.Instance.Cam.transform.position, Player.Instance.Cam.transform.forward);
		Debug.DrawRay(ray.origin, ray.direction * MaxReachDistance());
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, MaxReachDistance(), LayerMask.GetMask("Hittable")))
		{
			animator.SetBool("UsingTool", true);
			IHittable hittableObject = hitInfo.transform.GetComponent<IHittable>();
			if (hittableObject != null)
			{
				hittableObject.TakeHit(power);
				durability--;
				if(durability == 0)
				{
					//<-- delete item in inventory
				}
			}
		}
	}
}