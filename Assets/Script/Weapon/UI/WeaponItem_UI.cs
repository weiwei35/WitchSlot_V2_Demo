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

	public ObjectEventSO ShowHurtAreaEvent;
	public ObjectEventSO HideHurtAreaEvent;

	public void Init()
	{
		icon.sprite = weapon.itemIcon;
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		//显示tips
		ShowTipsEvent.RaiseEventWithGameObject(weapon,gameObject,this);
		ShowHurtAreaEvent.RaiseEvent(weapon.hurtArea,this);
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		//隐藏tips
		HideTipsEvent.RaiseEventWithGameObject(weapon,gameObject,this);
		HideHurtAreaEvent.RaiseEvent(weapon.hurtArea,this);
	}
}