using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IHittable
{
	public void TakeHit(int damage);
}

public class CrateBehaviour : MonoBehaviour, IHittable
{	
	public string InteractionPromt => throw null;

	public Crate Crate;
	public Sprite CrateSprite;

	[HideInInspector] public bool Opened = false;

	[SerializeField] Image healthBar1;
	[SerializeField] Image healthBar2;
	[SerializeField] TMP_Text healthText1;
	[SerializeField] TMP_Text healthText2;
	[SerializeField] GameObject wholeModel;
	[SerializeField] GameObject brokenModel;
	[SerializeField] GameObject lootText;

	[SerializeField] GameObject droppedItemObject;

	private CrateOpeningManager openingManager;

	private float maxHealth;
	private float health;

    private void Start()
    {
		maxHealth = Crate.Health;
		health = maxHealth;
		UpdateHealthBar();
	}

    private void OnDestroy()
	{
		StopAllCoroutines();
	}

    private void Update()
    {
		UpdateHealthBar();
    }

	public void Initialize(CrateOpeningManager openingManager)
	{
		this.openingManager = openingManager;
	}

    public void TakeHit(int amount)
    {
		if (Opened) 
			return;

		health -= amount;
		if (health <= 0)
		{
			health = 0;
			Break();
		}
    }

    private void UpdateHealthBar()
    {
		float remainingHealth = health / maxHealth;

        healthText1.text = $"{remainingHealth * 100}%";
		healthText2.text = $"{remainingHealth * 100}%";

		healthBar1.fillAmount = 
			Mathf.Lerp(healthBar1.fillAmount, remainingHealth, 
			4 * Time.deltaTime);
		healthBar2.fillAmount = 
			Mathf.Lerp(healthBar2.fillAmount, remainingHealth,
            4 * Time.deltaTime);

		Color healthColor = 
			Color.Lerp(Color.red, Color.green, remainingHealth);

		healthBar1.color = healthColor;
		healthBar2.color = healthColor;
    }

	public void Break()
	{
		openingManager.CrateComboQueue.Add(this);
		Destroy(transform.GetComponent<Collider>());
		Destroy(transform.GetComponent<Rigidbody>());

		Crate.OpenCrate();
		if (Crate.ScrapDropName != "None" && Crate.ScrapDropName != "")
		{
			DropScrap();
		}

		StartCoroutine(OpeningSequence());
	}

	private void DropScrap()
	{
		Item item = ResourceSystem.Instance.GetItem(Crate.ScrapDropName);

		ItemPickup itemPickup = droppedItemObject.GetComponentInChildren<ItemPickup>();

		if (itemPickup != null)
		{
			itemPickup.ItemToDrop = item;
			itemPickup.Amount = Crate.ScrapDropAmount;
			GameObject droppedScrap =
				Instantiate(droppedItemObject, transform.position, Quaternion.identity);
			droppedScrap.GetComponent<Rigidbody>().velocity = droppedScrap.transform.up * 4;
		}
	}

	private void ShowFloatingLootText(string text, Color color)
	{
		transform.rotation = Quaternion.identity;
		GameObject newLootText = Instantiate(lootText, transform.position, Quaternion.identity, transform);
		
		newLootText.GetComponent<TextMesh>().color = color;
		newLootText.GetComponent<TextMesh>().text = text;

		newLootText.SetActive(true);
	}

	IEnumerator OpeningSequence()
	{
		brokenModel.SetActive(true);
		wholeModel.SetActive(false);

		GameObject model = brokenModel.transform.GetChild(0).gameObject;
		brokenModel.transform.DetachChildren();

		Collider[] colliders = Physics.OverlapSphere(brokenModel.transform.position, 1);
		foreach (Collider collider in colliders)
		{
			if (collider.TryGetComponent(out Rigidbody rb))
			{
				rb.AddExplosionForce(80, brokenModel.transform.position, 1);
			}
		}

		yield return new WaitForSeconds(.5f);
		ShowFloatingLootText($"+{Crate.MoneyDrop} Coins", Color.yellow);
		yield return new WaitForSeconds(.2f);
		ShowFloatingLootText($"+{Crate.XPDrop} XP", Color.white);

		yield return new WaitForSeconds(2f);

		for (int i = 0; i < model.transform.childCount - 1;)
		{
			yield return new WaitForSeconds(0.2f);
			Destroy(model.transform.GetChild(0).gameObject);
		}
		Destroy(gameObject);
	}
}
