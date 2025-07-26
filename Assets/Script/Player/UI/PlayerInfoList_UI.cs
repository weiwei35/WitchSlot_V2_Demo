using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PlayerInfoList_UI : MonoBehaviour 
{
	public PlayerInfo_UI playerInfo_UI;
	public MMF_Player feedBack;
	public void SetPlayer()
	{
		playerInfo_UI.Init();
		playerInfo_UI.gameObject.SetActive(true);
		feedBack.PlayFeedbacks();
	}
}
