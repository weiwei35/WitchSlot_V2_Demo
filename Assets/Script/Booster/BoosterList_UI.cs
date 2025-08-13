using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class BoosterList_UI : MonoBehaviour 
{
	public BoosterManager boosterManager;
	public BoosterItem_UI boosterItem_UI;
	public GameObject boosterParent;
	public void Init()
	{
		foreach (Transform obj in boosterParent.transform)
		{
			Destroy(obj.gameObject);
		}
		foreach (var booster in boosterManager.playerLibrary.boosters)
		{
			var boosterItem = Instantiate(boosterItem_UI,boosterParent.transform);
			boosterItem.booster = booster;
			boosterItem.Init();
		}
	}
}
