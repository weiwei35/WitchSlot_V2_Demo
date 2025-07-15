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

public class WeaponInBag_UI : WeaponItem_UI, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
	public WeaponGroup group;
	public void OnPointerClick(PointerEventData eventData)
	{
		//替换装备
		group.ChangeWeapon(weapon);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		return;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		return;
	}
}