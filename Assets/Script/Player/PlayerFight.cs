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
	bool canAttack = false;
	bool skipStep = false;
	private void Update()
	{
		if (canAttack)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				canAttack = false;
				//释放符文
				GridAttackEvent.RaiseEvent(null,this);
				StartCoroutine(EndAttack());
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.E) && !skipStep)
			{
				skipStep = true;
				GetComponent<PlayerMove_new>().PlayerMoveEvent.RaiseEvent(null,this);
				StartCoroutine(ResetSkipStep());
			}
		}
	}

	IEnumerator ResetSkipStep()
	{
		yield return new WaitForSeconds(1f);
		skipStep = false;
	}
	public void SetCanAttack()
	{
		canAttack = true;
		GetComponent<PlayerMove_new>().canMove = false;
	}
	public void SetCanNotAttack()
	{
		canAttack = false;
		GetComponent<PlayerMove_new>().canMove = true;
	}

	IEnumerator EndAttack()
	{
		GetComponent<PlayerMove_new>().canMove = true;
		yield return new WaitForSeconds(0f);
		
		// EndGridAttackEvent.RaiseEvent(null,this);
	}
	public ObjectEventSO GridAttackEvent;
	// public ObjectEventSO EndGridAttackEvent;
}
