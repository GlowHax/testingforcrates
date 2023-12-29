using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrapViewTemplate : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
	public bool Draggable = true;
	public TMPro.TMP_Text AmountTxt;

	/*[HideInInspector]*/ public ScrapSO Scrap;
	[HideInInspector] public RectTransform SalvageMachine;
	[HideInInspector] public Transform ActivePanel;
	[HideInInspector] public WorkshopUI UIManager;

	[SerializeField] Image icon;

	private Transform previousParent;
	private bool loaded = false;

	private void Update()
	{
		if (!loaded)
			return;

		AmountTxt.text = $"{Scrap.AmountInInventory}";
	}
	public void SetUp()
	{
		icon.sprite = Scrap.Icon;
		AmountTxt.text = $"{Scrap.AmountInInventory}";
		loaded = true;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!Draggable) return;
		previousParent = transform.parent;
		transform.SetParent(ActivePanel);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!Draggable) return;
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!Draggable) return;
		transform.SetParent(previousParent);
		if (!UIManager.Salvaging && RectTransformUtility.RectangleContainsScreenPoint(SalvageMachine, Input.mousePosition))
		{
			Scrap.AmountInInventory -= 1;
			if(Scrap.AmountInInventory <= 0)
				GameObject.Destroy(gameObject);
			UIManager.SalvagedScrap = Scrap;
			UIManager.StartSalvaging();
		}
	}
}
