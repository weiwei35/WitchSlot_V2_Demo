using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class BoosterList : MonoBehaviour 
{
	public List<BoosterDataSO> boosters = new List<BoosterDataSO>();
	public BoosterItem_UI boosterItem_UI;
	public GameObject boosterParent;
	public void Init()
	{
		foreach (var booster in boosters)
		{
			var boosterItem = Instantiate(boosterItem_UI,boosterParent.transform);
			boosterItem.booster = booster;
			boosterItem.Init();
		}
	}
}
