using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PickNextRoom : MonoBehaviour 
{
	public ObjectEventSO PickNextRoomEvent;
	public ObjectEventSO LeaveRoomEvent;
	private void OnTriggerEnter2D(Collider2D other)
	{
		//弹出选房间面板
		if (other.gameObject.tag == "Player")
		{
			LeaveRoomEvent.RaiseEvent(null,this);
			PickNextRoomEvent.RaiseEvent(null,this);
		}
	}
}
