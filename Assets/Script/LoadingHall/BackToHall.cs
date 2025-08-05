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

public class BackToHall : MonoBehaviour
{
	public MMF_Player loadingFeedBack;
	public ObjectEventSO LeaveRoomEvent;

	private void OnTriggerEnter2D(Collider2D other)
	{
		LeaveRoomEvent.RaiseEvent(null,this);
		loadingFeedBack.PlayFeedbacks();
	}
}
