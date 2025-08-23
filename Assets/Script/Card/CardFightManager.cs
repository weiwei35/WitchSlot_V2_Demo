using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CardFightManager : MonoBehaviour 
{
	//攻击方式：花色/攻击范围：牌型+点数
	public Player player;
	public EnemyGroup enemyGroup;
	private float gridSize = 0.8f;
	private int hurtCount = 0;

	public GameObject effect;

	public void GetCard(object o)
	{
		PokerEvaluator.PokerEvaluationResult result = (PokerEvaluator.PokerEvaluationResult)o;
		List<CardSuit> cardSuits = new List<CardSuit>();
		hurtCount = 0;
		foreach (var card in result.relevantCards)
		{
			hurtCount += card.rank;
			if (!cardSuits.Contains(card.suit))
			{
				cardSuits.Add(card.suit);
			}
		}
		switch (result.handType)
		{
			case PokerHand.高牌:
				SpecialAttack_1();
				break;
			case PokerHand.一对:
				break;
			case PokerHand.顺子:
				break;
			case PokerHand.同花:
				break;
			case PokerHand.三条:
				break;
		}
	}

	public void NormalAttack(object o1,object o2)//1.角色面前格子坐标2.卡牌数据
	{
		Vector3 facePos = (Vector3)o1;
		CardData cardData = (CardData)o2;
		switch (cardData.suit)
		{
			case CardSuit.Clubs:
				StartCoroutine(ClubsAttack(facePos));
				break;
			case CardSuit.Diamonds:
				DiamondsAttack(facePos);
				break;
			case CardSuit.Hearts:
				HeartsAttack(facePos);
				break;
			case CardSuit.Spades:
				SpadesAttack();
				break;
		}
	}

	IEnumerator ClubsAttack(Vector3 facePos)
	{
		Vector3 playerPos = player.transform.position;
        Vector3 distance = facePos - playerPos;
        List<Vector3> hurtPos = new List<Vector3>();
        hurtPos.Add(playerPos+distance*1);
        hurtPos.Add(playerPos+distance*2);
        hurtPos.Add(playerPos+distance*3);
        foreach (var pos in hurtPos)
        {
        	SetEnemyHurt(pos, 1);
        	var effectObj = Instantiate(effect, pos, Quaternion.identity);
	        yield return new WaitForSeconds(0.2f);
        }
	}

	void DiamondsAttack(Vector3 facePos)
	{
		Vector3 playerPos = player.transform.position;
		Vector3 distance = facePos - playerPos;
		Vector3 left = new Vector2(-distance.y, distance.x);
		List<Vector3> hurtPos = new List<Vector3>();
		hurtPos.Add(facePos);
		hurtPos.Add(facePos + left);
		hurtPos.Add(facePos - left);
		foreach (var pos in hurtPos)
		{
			SetEnemyHurt(pos, 1);
			var effectObj = Instantiate(effect, pos, Quaternion.identity);
		}
	}
	void SpadesAttack()
	{
		Vector3 playerPos = player.transform.position;
		List<Vector3> hurtPos = new List<Vector3>();
		hurtPos.Add(playerPos+new Vector3(1, 0, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(-1, 0, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(0, 1, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(0, -1, 0)*gridSize);
		foreach (var pos in hurtPos)
		{
			SetEnemyHurt(pos, 1);
			var effectObj = Instantiate(effect, pos, Quaternion.identity);
		}
	}
	
	void HeartsAttack(Vector3 facePos)
	{
		Vector3 playerPos = player.transform.position;
		Vector3 distance = facePos - playerPos;
		Vector3 left = new Vector2(-distance.y, distance.x);
		List<Vector3> hurtPos = new List<Vector3>();
		hurtPos.Add(facePos + distance);
		hurtPos.Add(facePos + left);
		hurtPos.Add(facePos - left);
		foreach (var pos in hurtPos)
		{
			SetEnemyHurt(pos, 1);
			var effectObj = Instantiate(effect, pos, Quaternion.identity);
		}
	}

	public void SpecialAttack_1()
	{
		List<Vector3> hurtPos = new List<Vector3>();
		Vector3 playerPos = player.transform.position;
		hurtPos.Add(playerPos+new Vector3(1, 0, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(-1, 0, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(0, 1, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(0, -1, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(1, 1, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(-1, 1, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(-1, -1, 0)*gridSize);
		hurtPos.Add(playerPos+new Vector3(1, -1, 0)*gridSize);
		Vector3 attackPos = hurtPos[Random.Range(0, hurtPos.Count)];
		
		SetEnemyHurt(attackPos, hurtCount);
		var effectObj = Instantiate(effect, attackPos, Quaternion.identity);
		hurtCount = 0;
	}
	public void SetEnemyHurt(Vector3 pos,float hurt)
	{
		// yield return new WaitForSeconds(0.5f);
		foreach (EnemyCommon enemy in enemyGroup.enemies)
		{
			if (enemy != null && Vector3.Distance(pos, enemy.transform.position) < 0.1f) //判断怪物位置是否有符文
			{
				enemy.TakeDamage(hurt);
				if (enemyGroup.enemies.Count == 0) break;
			}
		}
	}
}
