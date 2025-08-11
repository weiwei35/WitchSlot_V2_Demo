using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemyList : MonoBehaviour
{
	public EnemyListItem_UI enemyObj;
	public GameObject enemyListParent;
	private List<EnemyListItem_UI> enemyList = new List<EnemyListItem_UI>();

	public void DeleteEnemy(object obj)
	{
		EnemyCommon enemy = obj as EnemyCommon;
		foreach (var child in enemyList)
		{
			if (child != null && child.enemyCommon.GetInstanceID() == enemy?.GetInstanceID())
			{
				Destroy(child.gameObject);
			}
		}
	}
	IEnumerator SetEnemyListOne()
	{
		EnemyGroup enemyGroup = GameObject.FindGameObjectWithTag("EnemyGroup").GetComponent<EnemyGroup>();
		foreach (var enemy in enemyGroup.enemiesInFight.ToList())
		{
			if (enemy.newAdd)
			{
                var enemyItem = Instantiate(enemyObj, enemyListParent.transform);
                enemyItem.InitEnemy(enemy);
                enemyList.Add(enemyItem);
                enemy.newAdd = false;
				yield return new WaitForSeconds(0.2f);
			}
		}
	}

	public void SetEnemyListAdd()
	{
		StartCoroutine(SetEnemyListOne());
	}
}
