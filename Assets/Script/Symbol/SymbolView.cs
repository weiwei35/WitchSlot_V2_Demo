using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class SymbolView : MonoBehaviour 
{
	public SpriteRenderer spriteRenderer;
	public TMP_Text attackText;
	
	SymbolSO symbolController;
	
	public void InitSymbol(SymbolSO symbol)
	{
		symbolController = symbol;
		spriteRenderer.sprite = symbolController.symbolIcon;
		attackText.text = symbolController.symbolAttack.ToString();
	}
}
