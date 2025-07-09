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

public class SkillItem_UI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
	public SkillController skillController;
	public SkillItemSO skillData;
	public Image skillIcon;
	
	public TipsEventSO ShowTipsEvent;
	public TipsEventSO HideTipsEvent;

	public void Init(SkillItemSO skill)
	{
		skillData = skill;
		skillIcon.sprite = skillData.itemIcon;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		//显示tips
		ShowTipsEvent.RaiseEventWithGameObject(skillData,gameObject,this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//隐藏tips
		HideTipsEvent.RaiseEventWithGameObject(skillData,gameObject,this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (skillController.canSelectSkill)
		{
			skillController.ShowSkillInfo(skillData);
		}
	}
}
