using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CardItemUI : MonoBehaviour 
{
	public Image image;
	public CardData data;
	public void InitCard(Sprite sprite,CardData cardData)
	{
		image.sprite = sprite;
		data = cardData;
	}
}
