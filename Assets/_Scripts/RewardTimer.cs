using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardTimer : MonoBehaviour
{
	public bool TimerComplete = true;

	[SerializeField] int hours;
	[SerializeField] int minutes;
	[SerializeField] int seconds;
	[SerializeField] string timerName;
	[HideInInspector] string timerNameStart;
	[HideInInspector] string timerNameEnd;
	[SerializeField] Image progressImg;

	private DateTime startDate;
	private DateTime endDate;
	private TimeSpan timerDuration;
	private TimeSpan remainingTime;
	
	private bool _timerIsReady;
	private float progressFiller = 1f;

	void Start()
	{
		timerDuration = TimeSpan.Parse(hours + ":" + minutes + ":" + seconds);
		timerNameStart = $"{timerName}_start_time";
		timerNameEnd = $"{timerName}_end_time";

		CheckTime();

		if (remainingTime > TimeSpan.Zero)
		{
			TimerComplete = false;
			RefreshProgress();
		}
	}

	private void Update()
	{
		if (_timerIsReady)
		{
			if (!TimerComplete && PlayerPrefs.GetString(timerName) == "running")
			{
				progressFiller -= Time.deltaTime * (1f / (float)timerDuration.TotalSeconds);
				progressImg.fillAmount = progressFiller;

				if (progressFiller <= 0 && !TimerComplete)
				{
					CheckTime();
				}
			}
			else
			{
				progressFiller = 1f;
			}
		}
	}

	private void RefreshProgress()
	{
		float ah = 1f / (float)timerDuration.TotalSeconds;
		float bh = 1f / (float)remainingTime.TotalSeconds;
		progressFiller = ah / bh;
		progressImg.fillAmount = progressFiller;
	}

	private void CheckTime()
	{
		if (PlayerPrefs.GetString(timerName) == "running")
		{
			startDate = DateTime.Parse(PlayerPrefs.GetString(timerNameStart));
			endDate = DateTime.Parse(PlayerPrefs.GetString(timerNameEnd));
			DateTime current = TimeManager.Instance.GetCurrentDateTime();
			remainingTime = endDate - current;

			if (current >= endDate)
			{
				TimerComplete = true;
				remainingTime = TimeSpan.Zero;
				PlayerPrefs.SetString(timerName, "");
				progressImg.gameObject.SetActive(false);
			}
			else
			{
				progressImg.gameObject.SetActive(true);
				TimerComplete = false;
				_timerIsReady = true;
			}
		}
	}

	public void ResetTimer()
	{
		startDate = TimeManager.Instance.GetCurrentDateTime();
		endDate = startDate.Add(TimeSpan.Parse(hours + ":" + minutes + ":" + seconds));
		PlayerPrefs.SetString($"{timerNameStart}", startDate.ToString());
		PlayerPrefs.SetString($"{timerNameEnd}", endDate.ToString());
		PlayerPrefs.SetString(timerName, "running");

		remainingTime = TimeSpan.Zero;
		TimerComplete = false;
		_timerIsReady = true;
		progressImg.gameObject.SetActive(true);
		CheckTime();
	}
}