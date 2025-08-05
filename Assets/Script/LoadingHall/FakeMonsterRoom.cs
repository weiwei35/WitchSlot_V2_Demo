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

public class FakeMonsterRoom : LoadingHallItems 
{
	[Header("Feedbacks")]
	public MMF_Player LoadSceneFeedback;

	public ObjectEventSO LeaveRoomEvent;
	public override void PlayerEnter()
	{
		if (getWeapon)
		{
			//进入地图
			LeaveRoomEvent.RaiseEvent(null,this);
			LoadSceneFeedback?.PlayFeedbacks();
		}
		else
		{
			Debug.LogError("先去拿装备吧贝贝");
		}
	}
}
