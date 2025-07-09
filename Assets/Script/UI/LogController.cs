using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class LogController : MonoBehaviour 
{
	public static LogController instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public delegate void LogDelegate(string log);
	public LogDelegate logDelegate;

	private void Start()
	{
		logDelegate += LOGDelegate;
	}

	public GameObject logPanel;
	public GameObject logObj;
	private void LOGDelegate(string log)
	{
		var textObj = Instantiate(logObj, logPanel.transform);
		textObj.GetComponent<TMP_Text>().text = log;
	}
}
