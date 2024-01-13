using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TimerFormats
{
    Seconds,
	HundrethsMilliseconds,
    TenthMilliseconds,
    Milliseconds
}

public class ComboTimer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timerText;
	[SerializeField] private TextMeshProUGUI comboText;
	[SerializeField] private Image background;
	[SerializeField] private TimerFormats format;

	private Dictionary<TimerFormats, string> timerFormats = new Dictionary<TimerFormats, string>();
    private float currrentTime;
	private bool timerReady = false;

	private void Start()
	{
        timerFormats.Add(TimerFormats.Seconds, "00"); 
        timerFormats.Add(TimerFormats.HundrethsMilliseconds, "00.0"); 
        timerFormats.Add(TimerFormats.TenthMilliseconds, "00.00"); 
        timerFormats.Add(TimerFormats.Milliseconds, "00.000"); 
	}

	void Update()
    {
        if(currrentTime <= 0f && timerReady)
        {
            currrentTime = 0f;
            timerReady = false;
			Player.Instance.ComboBonus = 0;
			transform.parent.gameObject.SetActive(false);
        }
        else if(currrentTime > 0f && timerReady)
        {
			currrentTime -= Time.deltaTime;
		}

        SetTimerText();
	}

	public void StartTimer(Color comboColor, float timer)
	{
		currrentTime = timer;
		comboText.text = $"+{Convert.ToInt32(Player.Instance.ComboBonus * 100)}% Combo!";
		comboText.color = comboColor;
		background.color = comboColor;
		timerReady = true;
	}

	private void SetTimerText()
    {
        timerText.text = currrentTime.ToString(timerFormats[format]);
    }
}