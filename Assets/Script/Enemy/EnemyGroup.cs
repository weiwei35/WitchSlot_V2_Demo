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
/// 房间怪物组
/// 1.控制怪物按顺序行动
/// 2.怪物死亡后判断是否结束房间
/// </summary>
public class EnemyGroup : MonoBehaviour 
{
	public List<EnemyCommon> enemies;
	public List<EnemyCommon> enemiesInFight;
	public bool enemyAllDie = false;

	private void Start()
	{
		enemyAllDie = false;
	}
	public ObjectEventSO EndRoomEvent;
	public ObjectEventSO EndFightEnemyEvent;
	public void EnemyDie(EnemyCommon enemy)
	{
		if(enemiesInFight.Contains(enemy))
			enemiesInFight.Remove(enemy);
		if(enemiesInFight.Count == 0)
			EndFightEnemyEvent.RaiseEvent(null,this);
		enemies.Remove(enemy);
		if (enemies.Count == 0)
		{
			enemyAllDie = true;
		}
		if (enemyAllDie)
		{
			EndRoomEvent.RaiseEvent(null,this);
		}
	}
}
