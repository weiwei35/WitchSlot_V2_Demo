using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour 
{
	//进入房间后，每次行动召唤并且释放符文
	//房间怪物清空后，不再召唤符文

	public CardItemUI cardPrefab;
	public TMP_Text titleText;
	public bool canCallCard = false;
	List<CardData> hand = new List<CardData>();//手牌数据
	List<CardItemUI> cardItems = new List<CardItemUI>();//手牌UI

	// private int stepCount = 0;

	public ObjectEventSO ShowCardEvent;
	public ObjectEventSO OneCardEvent;
	public void OnEnable()
	{
		DeckManager.Instance.InitializeDeck();
	}

	public void SetCanCall()
	{
		canCallCard = true;
	}

	public void SetCannotCall()
	{
		canCallCard = false;
	}

	//每次行走翻一张牌（小攻击），3张牌都翻完（大攻击），第一张牌翻牌前重置牌组动画
	Vector3 attackPos = Vector3.zero;
	public void ShowCard(object o)
	{
		if(!canCallCard) return;
		attackPos = (Vector3)o;
		// if (stepCount == 0)
		// {
			CallCards();
		// }
		// if (stepCount < 3)
		// {
		// 	cardItems[stepCount]?.GetComponent<Animation>().Play();
		// 	OneCardEvent.RaiseEvent2Para(pos,cardItems[stepCount].data,this);
		// 	stepCount++;
		// }
		// if(stepCount == 3)
		// {
		// 	stepCount = 0;
			GetCardType();
		// }
	}

	public void CallCards()
	{
		if(transform.childCount>0)
		{
			foreach (Transform obj in transform)
			{
				Destroy(obj.gameObject);
			}
		}
		cardItems.Clear();
		hand.Clear();
		titleText.text = "";
		hand = DeckManager.Instance.DrawCards(3);
		// Debug.Log($"🎴 抽出：{hand.Count} 张牌");
		foreach (var card in hand)
		{
			// Debug.Log(card.displayName);
			var cardObj = Instantiate(cardPrefab, transform);
			cardObj.InitCard(card.cardSprite,card);
			cardObj?.GetComponent<Animation>().Play();
			cardItems.Add(cardObj);
		}
	}
	public void GetCardType()
	{
		// 获取牌型与关键牌
        PokerEvaluator.PokerEvaluationResult result = PokerEvaluator.EvaluateHand(hand);
        titleText.text = result.handType.ToString();
        // 标记关键牌
        foreach (var keyCard in result.relevantCards) {
        	var matchingCard = cardItems.FirstOrDefault(c => 
        		c.GetComponent<CardItemUI>().data == keyCard);
        	if (matchingCard != null) {
        		matchingCard.GetComponent<CardItemUI>().image
        			.color = Color.yellow; // 高亮黄色
		        OneCardEvent.RaiseEvent2Para(attackPos,matchingCard.data,this);
        	}
        }
        // 如果需要显示踢脚牌，也可以标记
        foreach (var kicker in result.kickerCards) {
        	var matchingCard = cardItems.FirstOrDefault(c => 
        		c.GetComponent<CardItemUI>().data == kicker);
        	if (matchingCard != null) {
        		matchingCard.GetComponent<CardItemUI>().image
        			.color = Color.gray; // 踢脚牌灰色标记
        	}
        }
        ShowCardEvent.RaiseEvent(result,this);
	}
}
