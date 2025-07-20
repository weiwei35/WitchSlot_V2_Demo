using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GetWeaponFree : LoadingHallItems 
{
	//获取随机装备
	public ObjectEventSO GetWeaponEvent;
	public override void PlayerEnter()
	{
		GetWeaponEvent.RaiseEvent(null,this);
		getWeapon = true;
		gameObject.SetActive(false);
	}
}
