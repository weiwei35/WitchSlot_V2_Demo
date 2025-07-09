using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PlayerInfoList_UI : MonoBehaviour 
{
	public PlayerInfo_UI playerInfo_UI;
	private void Start()
	{
		FightController.instance.prepareFight += SetPlayer;
	}

	private void OnDisable()
	{
		FightController.instance.prepareFight -= SetPlayer;
	}

	private void SetPlayer()
	{
		playerInfo_UI.Init();
		playerInfo_UI.gameObject.SetActive(true);
	}
}
