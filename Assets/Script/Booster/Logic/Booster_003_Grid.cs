using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Booster_003_Grid : MonoBehaviour 
{
	public int count = 0;
	public float hurt = 0;
	public GameObject effect;

	public void PlayerMove()
	{
		if (count == 0)
		{
			Destroy(gameObject);
		}
		else
		{
			count--;
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			EnemyCommon enemyBase = other.GetComponent<EnemyCommon>();
			var effectObj = Instantiate(effect);
			enemyBase.TakeDamage(hurt);
		}
	}

	// private void OnTriggerStay2D(Collider2D other)
	// {
	// 	if (other.tag == "Enemy")
	// 	{
	// 		EnemyCommon enemyBase = other.GetComponent<EnemyCommon>();
	// 		var effectObj = Instantiate(effect);
	// 		enemyBase.TakeDamage(hurt);
	// 	}
	// }
}
