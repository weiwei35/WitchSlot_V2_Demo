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

	private void Start()
	{
		FightController.instance.prepareFight += SetEnemyList;
		FightController.instance.meetEnemy += SetEnemyListAdd;
	}

	private void OnDisable()
	{
		FightController.instance.prepareFight -= SetEnemyList;
		FightController.instance.meetEnemy -= SetEnemyListAdd;
	}

	public void DeleteEnemy(object obj)
	{
		EnemyCommon enemy = obj as EnemyCommon;
		foreach (var child in enemyList)
		{
			if (child != null && child.enemyCommon == enemy)
			{
				Destroy(child.gameObject);
			}
		}
	}

	IEnumerator SetEnemyListOneByOne()
	{
		EnemyGroup enemyGroup = GameObject.FindGameObjectWithTag("EnemyGroup").GetComponent<EnemyGroup>();
		foreach (var enemy in enemyGroup.enemiesInFight)
		{
			yield return new WaitForSeconds(0.2f);
			var enemyItem = Instantiate(enemyObj, enemyListParent.transform);
			enemyItem.InitEnemy(enemy);
			enemyList.Add(enemyItem);
			enemy.newAdd = false;
		}
	}
	IEnumerator SetEnemyListOne()
	{
		EnemyGroup enemyGroup = GameObject.FindGameObjectWithTag("EnemyGroup").GetComponent<EnemyGroup>();
		foreach (var enemy in enemyGroup.enemiesInFight)
		{
			if (enemy.newAdd)
			{
				yield return new WaitForSeconds(0.2f);
                var enemyItem = Instantiate(enemyObj, enemyListParent.transform);
                enemyItem.InitEnemy(enemy);
                enemyList.Add(enemyItem);
                enemy.newAdd = false;
			}
		}
	}

	IEnumerator StartFight()
	{
		EnemyGroup enemyGroup = GameObject.FindGameObjectWithTag("EnemyGroup").GetComponent<EnemyGroup>();
		yield return new WaitForSeconds(enemyGroup.enemiesInFight.Count*0.5f);
		FightController.instance.startRound?.Invoke();
	}
	public void SetEnemyList()
	{
		StartCoroutine(SetEnemyListOneByOne());
		StartCoroutine(StartFight());
	}

	public void SetEnemyListAdd()
	{
		StartCoroutine(SetEnemyListOne());
	}
}
