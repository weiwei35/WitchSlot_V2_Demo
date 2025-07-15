using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;
/// <summary>
/// 进战-自动召唤符文-方向键设置方向/WASD移动-空格释放符文-释放法术/使用道具-空格结束回合
/// </summary>
public class PlayerFight : MonoBehaviour
{
	public Sprite icon;
	public FightWeight fightWeight;
	public GridController gridController;
	
	bool canAttack = false;
	bool canSkip = false;

	private void Start()
	{
		FightController.instance.endRound += StopFight;
	}


	private void Update()
	{
		if (canSkip)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				canSkip = false;
				// FightController.instance.NextStep();
			}
		}
		if (canAttack)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				canAttack = false;
				StartCoroutine(SetCanSkipDelay());
				//释放符文
				GridAttackEvent.RaiseEvent(null,this);
				LogController.instance.logDelegate?.Invoke("Player: 释放符文");
			}
		}
	}

	IEnumerator SetCanSkipDelay()
	{
		yield return new WaitForSeconds(0.2f);
		canSkip = true;
	}
	IEnumerator SetCannotSkipDelay()
	{
		yield return new WaitForSeconds(0.2f);
		canSkip = false;
	}

	//进战
	public void StartFight()
	{
		// canAttack = true;
		// canSkip = false;
		// CallSymbols();
	}

	private void StopFight()
	{
		canAttack = false;
		StartCoroutine(SetCannotSkipDelay());
	}
	public ObjectEventSO GridMoveEvent;
	public ObjectEventSO GridCallEvent;
	public ObjectEventSO GridAttackEvent;
	public void CallSymbols()
	{
		canAttack = true;
		GridCallEvent.RaiseEvent(null,this);
		GridMoveEvent.RaiseEvent(null,this);
		gridController.SetSymbol(gridController.defaultGrid.Count);
		LogController.instance.logDelegate?.Invoke("Player: 召唤符文");
	}
}
