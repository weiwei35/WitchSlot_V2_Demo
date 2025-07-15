using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// 用于将符文抽牌、弃牌
/// </summary>
public class SymbolDeck : MonoBehaviour 
{
	public SymbolManager symbolManager;
	private List<SymbolSO> symbolToAdd = new List<SymbolSO>();//抽牌堆
	private List<SymbolSO> symbolUsed = new List<SymbolSO>();//弃牌堆
	private List<SymbolSO> symbolListCurrent = new List<SymbolSO>();//当前符文列表

	public ObjectEventSO PlayerSymbolReadyEvent;

	private void Start()
	{
		FightController.instance.setRoom += InitializeDeck;
		// FightController.instance.endStep += ResetDesk;
	}

	public void ResetDesk()
	{
		symbolListCurrent.Clear();
	}

	public void InitializeDeck()
	{
		symbolToAdd.Clear();
		foreach (var symbolList in symbolManager.symbolLibrary.symbols)
		{
			for (int i = 0; i < symbolList.amount; i++)
			{
				symbolToAdd.Add(symbolList.symbolData);
			}
		}
		
		ShuffleSymbolList();
		PlayerSymbolReadyEvent.RaiseEvent(null,this);
	}

	private void SetSymbolList(int amount)
	{
		// amount = (int)MathF.Min(amount, symbolToAdd.Count);
		// for (int i = 0; i < amount; i++)
		// {
		// 	if (symbolToAdd.Count == 0)
		// 	{
		// 		//抽牌堆空了，弃牌堆拿回
		// 		foreach (var symbol in symbolUsed.ToList())
		// 		{
		// 			symbolToAdd.Add(symbol);
		// 			symbolUsed.Remove(symbol);
		// 		}
		//
		// 		ShuffleSymbolList();
		// 	}
		// 	symbolListCurrent.Add(symbolToAdd[0]);
		// 	symbolToAdd.RemoveAt(0);
		// }
		for (int i = 0; i < amount; i++)
		{
			symbolListCurrent.Add(symbolToAdd[Random.Range(0, symbolToAdd.Count)]);
		}
	}

	public List<SymbolSO> GetSymbolList(int amount)
	{
		SetSymbolList(amount);
		return symbolListCurrent;
	}

	public void DiscardSymbol(Object symbol)
	{
		// var symbolSo = symbol as SymbolSO;
		// symbolUsed.Add(symbolSo);
		// symbolListCurrent.Remove(symbolSo);
	}

	private void ShuffleSymbolList()
	{
		for (int i = 0; i < symbolToAdd.Count; i++)
		{
			SymbolSO symbol = symbolToAdd[i];
			int randomIndex = Random.Range(i, symbolToAdd.Count);
			symbolToAdd[i] = symbolToAdd[randomIndex];
			symbolToAdd[randomIndex] = symbol;
		}
	}
}
