using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class FightWeight : MonoBehaviour 
{
	[Range(0, 100)] public float weight; // 先攻权重值 (越高优先级越高)
	public int fightIndex;//攻击顺序
	
	public PlayerFight playerFight;
	public EnemyController enemyController;

	public ObjectEventSO StartStepEvent_Player;
	//当前回合攻击方法
	public void myTurn()
	{
		if (playerFight != null)
		{
			StartStepEvent_Player.RaiseEvent(null,this);
			// FightController.instance.playerMove?.Invoke();
			// playerFight.PlayerMove();
		}

		if (enemyController != null)
		{
			enemyController.StartFight();
		}
	}
}
