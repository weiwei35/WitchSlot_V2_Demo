using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Entrance : LoadingHallItems 
{
	//已经领取武器可进入地图，否则提示领取武器
	[Header("Feedbacks")]
	public MMF_Player LoadSceneFeedback;
	public override void PlayerEnter()
	{
		if (getWeapon)
		{
			//进入地图
			LoadSceneFeedback?.PlayFeedbacks();
			// SceneManager.LoadSceneAsync("FightScene",LoadSceneMode.Additive);
			// SceneManager.UnloadSceneAsync("GameHallScene");
		}
		else
		{
			Debug.LogError("先去拿装备吧贝贝");
		}
	}
}
