using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemyListItem_UI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
	public EnemyCommon enemyCommon;
	public Image enemyIcon;
	public TMP_Text enemyName;
	public TMP_Text enemyDesc;
	public IntVariable enemyHP;
	public TMP_Text HP;

	private void Start()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
	}

	public void InitEnemy(EnemyCommon enemy)
	{
		enemyCommon = enemy;
		enemyIcon.sprite = enemy.enemyIcon;
		enemyName.text = enemy.enemyName;
		enemyHP = enemy.hp;
		enemyDesc.text = "行动力：" + enemy.GetComponent<EnemyController>().stepStatic+"\n"+"攻击力："+enemy.attack;
	}

	private void Update()
	{
		if (enemyCommon != null)
		{
			HP.text = enemyHP.currentValue.ToString();
			SetHP();
		}
	}

	public Slider HPSlider;
	private void SetHP()
	{
		if (enemyHP != null) HPSlider.value = (float)enemyHP.currentValue / enemyHP.maxValue;
	}

	//弹出详情
	public EnemySkillList SkillList;
	private GameObject skill;
	public void OnPointerEnter(PointerEventData eventData)
	{
		var skillList = Instantiate(SkillList,transform.parent);
		skillList.transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
		skillList.InitSkillList(enemyCommon.skill);
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
		skill = skillList.gameObject;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (skill != null)
		{
			Destroy(skill);
		}
	}
}
