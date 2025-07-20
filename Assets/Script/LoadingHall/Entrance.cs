using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Entrance : LoadingHallItems 
{
	//已经领取武器可进入地图，否则提示领取武器
	public ObjectEventSO EnterUndergroundEvent;
	public override void PlayerEnter()
	{
		if (getWeapon)
		{
			//进入地图
			EnterUndergroundEvent.RaiseEvent(null,this);
			SceneManager.LoadSceneAsync("FightScene",LoadSceneMode.Additive);
			SceneManager.UnloadSceneAsync("GameHallScene");
		}
		else
		{
			Debug.LogError("先去拿装备吧贝贝");
		}
	}
}
