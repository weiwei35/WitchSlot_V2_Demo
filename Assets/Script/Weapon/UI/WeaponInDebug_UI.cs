using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class WeaponInDebug_UI : WeaponItem_UI
{
	public WeaponGroup group;
	public override void OnPointerClick(PointerEventData eventData)
	{
		//替换装备
		group.manager.playerWeaponLibrary.weapons.Add(weapon);
		group.PutWeaponInBag(weapon);
		group.debugObj.SetActive(false);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		//显示tips
		ShowTipsEvent.RaiseEventWithGameObject(weapon,gameObject,this);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		//隐藏tips
		HideTipsEvent.RaiseEventWithGameObject(weapon,gameObject,this);
	}
}