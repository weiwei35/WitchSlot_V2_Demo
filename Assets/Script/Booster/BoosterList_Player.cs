using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class BoosterList_Player : MonoBehaviour 
{
	public void OnEnable()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		foreach (var booster in BoosterManager.instance.playerLibrary.boosters)
		{
			var boosterObj = Instantiate(booster.booster, transform);
		}
	}
}
