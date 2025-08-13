using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GetBooster : LoadingHallItems 
{
	private void OnEnable()
	{
		if (getWeapon)
		{
			gameObject.SetActive(false);
		}
	}

	public override void PlayerEnter()
	{
		BoosterManager.instance.GetBooster
			(BoosterManager.instance.boosterDataList[Random.Range(0, BoosterManager.instance.boosterDataList.Count)]);
		gameObject.SetActive(false);
	}
}
