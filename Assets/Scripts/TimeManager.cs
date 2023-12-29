using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance = null;
    public string currentTime;
    public string currentDate;

    private const string API_URL = "https://worldtimeapi.org/api/ip";
    private string _timeData;
    private DateTime _currentDateTime = DateTime.Now;

    struct TimeData
	{
        public string datetime;
	}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

	private void Start()
	{
        StartCoroutine(GetTime());
	}

	public DateTime GetCurrentDateTime()
	{
        return _currentDateTime.AddSeconds(Time.realtimeSinceStartup);
	}

    DateTime ParseDateTime(string datetime)
	{
        string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }

    public IEnumerator GetTime()
    {
		yield return null;
		UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
		yield return webRequest.SendWebRequest();

		if (webRequest.result == UnityWebRequest.Result.ConnectionError)
		{
			Debug.Log("Not Connected:" + webRequest.error);
		}
		else
		{
			TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);

			_currentDateTime = ParseDateTime(timeData.datetime);
		}

		_timeData = GetCurrentDateTime().ToString();

		string[] words = _timeData.Split(' ');

		Debug.Log("The date is : " + words[0]);
		Debug.Log("The time is : " + words[1]);

		//setting current time
		currentDate = words[0];
		currentTime = words[1];
	}
}
