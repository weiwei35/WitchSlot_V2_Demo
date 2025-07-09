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
	private bool enemyAllDie = false;

	private void Start()
	{
		FightController.instance.startRound += StartRound;
	}

	private void StartRound()
	{
		enemyAllDie = false;
	}

	public void NextEnemyMove()
	{
		if (enemyAllDie) return;
		StartCoroutine(NextEnemy());
	}

	IEnumerator NextEnemy()
	{
		yield return new WaitForSeconds(0.5f);
		FightController.instance.NextStep();
	}

	public void EnemyDie(EnemyCommon enemy)
	{
		FightController.instance.ReCountFightWeight(enemy.fightweight.fightIndex);
		enemiesInFight.Remove(enemy);
		enemies.Remove(enemy);
		LogController.instance.logDelegate?.Invoke("击杀怪物："+enemy.name);
		if (enemiesInFight.Count == 0)
		{
			enemyAllDie = true;
			FightController.instance.endRound?.Invoke();
		}
	}
}
