using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public abstract class UIManager : MonoBehaviour
{
	[SerializeField] protected TMP_Text coinsHUD;
	[SerializeField] protected GameObject levelHUD;
	[SerializeField] protected TMP_Text levelCounter;
	[SerializeField] protected Slider levelProgressBar;
	[SerializeField] protected TMP_Text xpDetailTxt;

	private float xpFillSpeed = 0.75f;

	public struct CountToNumber
	{
		float countDuration;
		TMP_Text numberText;
		float currentValue;

		public CountToNumber(float currentValue, float countDuration, TMP_Text numberText)
		{
			this.currentValue = currentValue;
			this.countDuration = countDuration;
			this.numberText = numberText;
		}

		public IEnumerator CountTo(float targetValue)
		{
			var rate = Mathf.Abs(targetValue - currentValue) / countDuration;
			while (currentValue != targetValue)
			{
				currentValue = Mathf.MoveTowards(currentValue, targetValue, rate * Time.deltaTime);
				numberText.text = ((int)currentValue).ToString();
				yield return null;
			}
		}
	}

	protected void ManageLevelHUD()
	{
		if (levelHUD == null || 
			levelProgressBar == null || 
			levelCounter == null ||
			xpDetailTxt == null)
			return;

		levelCounter.text = $"{Player.Instance.Level}";

		if (EventSystem.current.IsPointerOverGameObject())
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, raycastResults);
			for(int i = 0; i < raycastResults.Count; i++)
			{
				if(raycastResults[i].gameObject.layer == levelHUD.layer && 
					raycastResults[i].gameObject.name == levelHUD.name)
				{
					xpDetailTxt.text = $"{Player.Instance.XP}/{Player.Instance.XPToNextLvl[Player.Instance.Level - 1]}";
					levelProgressBar.gameObject.SetActive(true);
					FillLevelProgressToCurrentState();
				}
			}
		}
		else
		{
			levelProgressBar.value = 0;
			levelProgressBar.gameObject.SetActive(false);
		}
	}

	protected virtual void FillLevelProgressToCurrentState()
	{
		if (levelProgressBar == null)
			return;

		float targetProgress = Player.Instance.GetLevelProgressPercentage();
		if (levelProgressBar.value < targetProgress)
		{
			levelProgressBar.value += xpFillSpeed * Time.deltaTime;
		}
	}

	protected virtual void RefreshCoinsHUD()
	{
		if(coinsHUD != null)
		{
			coinsHUD.text = $"{Player.Instance.Coins}";
		}
	}

	public virtual void GoToScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
