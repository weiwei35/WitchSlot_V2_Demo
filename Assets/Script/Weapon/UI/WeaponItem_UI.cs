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
		List<Vector2Int> gridPos = new List<Vector2Int>();
		foreach (var symbolList in weapon.symbolList)
		{
			foreach (var pos in symbolList.area)
			{
				if(!gridPos.Contains(pos)) gridPos.Add(pos);
			}
		}
		ShowHurtAreaEvent.RaiseEvent(gridPos,this);
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		//隐藏tips
		HideTipsEvent.RaiseEventWithGameObject(weapon,gameObject,this);
		List<Vector2Int> gridPos = new List<Vector2Int>();
		foreach (var symbolList in weapon.symbolList)
		{
			foreach (var pos in symbolList.area)
			{
				if(!gridPos.Contains(pos)) gridPos.Add(pos);
			}
		}
		HideHurtAreaEvent.RaiseEvent(gridPos,this);
	}
}