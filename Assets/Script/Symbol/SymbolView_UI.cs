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

public class SymbolView_UI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
	public SymbolSO symbolData;
	public Image symbolIcon;
	public TMP_Text symbolAttack;
	public void SetSymbol()
	{
		if (symbolData != null)
        {
        	symbolIcon.sprite = symbolData.symbolIcon;
        	symbolAttack.text = symbolData.symbolAttack.ToString();
        }
	}

	public GameObject hurtArea;
	public GameObject hurtAreaParent;
	public void OnPointerEnter(PointerEventData eventData)
	{
		//展示攻击范围和符文说明
		if (symbolData.symbolAttacks.Count > 0)
		{
			foreach (var grid in symbolData.symbolAttacks)
			{
				var hurt = Instantiate(hurtArea,transform.position,Quaternion.identity,hurtAreaParent.transform);
				hurt.transform.localPosition = grid.position*100;
			}
		}
		else
		{
			var hurt = Instantiate(hurtArea,transform.position,Quaternion.identity,hurtAreaParent.transform);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//隐藏攻击范围和符文说明
		foreach (Transform grid in hurtAreaParent.transform)
		{
			Destroy(grid.gameObject);
		}
	}
}
