using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Booster_Move : _BoosterItem_Logic 
{
	//每移动两次，对角色面前一个造成50%攻击力的伤害
	public int moveStep;
	public float hurtAmount;
	public GameObject effect;
	private int counter = 0;
	[HideInInspector]
	public Vector3 playerFacePos;
	public virtual void PlayerMove(object o)
	{
		if (o != null)
		{
			playerFacePos = (Vector3)o;
			
			if (counter >= moveStep)
			{
				counter = 0;
				BoosterEffect();
			}
			else
			{
				counter++;
			}
		}
	}

	public virtual void BoosterEffect()
	{
	}
	
	 public virtual IEnumerator SetEnemyHurt(Vector3 pos,float hurt)
	{
		yield return new WaitForSeconds(0.5f);
		EnemyGroup enemyGroup = GameObject.FindWithTag("EnemyGroup").GetComponent<EnemyGroup>();
		foreach (Transform enemy in enemyGroup.gameObject.transform)
		{
			EnemyCommon enemyBase = enemy.GetComponent<EnemyCommon>();
			if (enemy != null && Vector3.Distance(pos, enemy.position) < 0.1f) //判断怪物位置是否有符文
			{
				enemyBase.TakeDamage(hurt);
				if (enemyGroup.enemies.Count == 0) break;
			}
		}
	}
}
