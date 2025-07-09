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

public class WeaponItem_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public WeaponSO weapon;
	public Image icon;

	public TipsEventSO ShowTipsEvent;
	public TipsEventSO HideTipsEvent;

	public void Init()
	{
		icon.sprite = weapon.itemIcon;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		//显示tips
		ShowTipsEvent.RaiseEventWithGameObject(weapon,gameObject,this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//隐藏tips
		HideTipsEvent.RaiseEventWithGameObject(weapon,gameObject,this);
	}
}