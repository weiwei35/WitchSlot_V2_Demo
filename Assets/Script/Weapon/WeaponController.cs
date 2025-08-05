using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
//using NaughtyAttributes;
using Random = UnityEngine.Random;
/// <summary>
/// 玩家每次行走，蓄力+1
/// 蓄力达到coldtime，自动召唤符文
/// 已经召唤的符文，玩家每次行走自动刷新，直到释放符文
/// </summary>
public class WeaponController : MonoBehaviour
{
	private int coldTime;
	private WeaponItem_UI weaponItemUI;
	private WeaponSO weapon;
	Dictionary<Vector2Int,SymbolSO> symbols = new Dictionary<Vector2Int,SymbolSO>();
	Dictionary<Vector2Int,SymbolSO> symbolsCurrent = new Dictionary<Vector2Int,SymbolSO>();
	private bool attacked = false;
	public bool canAttack = false;
	public WeaponGroup group;
	[Header("广播事件")] 
	public DictionaryEventSO WeaponCallSymbol;

	bool canCallSymbol = false;
	public void SetStartFight()
	{
		canCallSymbol = true;
	}

	public void SetEndFight()
	{
		canCallSymbol = false;
	}
	private void Start()
	{
		weaponItemUI = GetComponent<WeaponItem_UI>();
		weapon = weaponItemUI.weapon;
		foreach (var symbolList in weapon.symbolList)
		{
			foreach (var areaPos in symbolList.area)
			{
				symbols.Add(areaPos, symbolList.symbol);
			}
		}
		coldTime = symbols.Count;
	}

	public void ResetWeapon()
	{
		if (weapon != null)
		{
			symbols.Clear();
			foreach (var symbolList in weapon.symbolList)
			{
				foreach (var areaPos in symbolList.area)
				{
					symbols.Add(areaPos, symbolList.symbol);
				}
			}
			coldTime = symbols.Count;
		}
	}

	public void GetCurrentSymbols(int count)
	{
		symbolsCurrent.Clear();
		List<Vector2Int> keys = new List<Vector2Int>();
		List<SymbolSO> values = new List<SymbolSO>();
		foreach (var symbolList in weapon.symbolList)
		{
			foreach (var areaPos in symbolList.area)
			{
				keys.Add(areaPos);
				values.Add(symbolList.symbol);
			}
		}
		for (int i = 0; i < count; i++)
		{
			symbolsCurrent.Add(keys[i],values[i]);
		}
	}
	//每次移动，每个装备加载1个符文
	public void PlayerWalk()
	{
		if(!canCallSymbol) return;
		if (coldTime > 0)
		{
			coldTime--;
			GetCurrentSymbols(symbols.Count - coldTime);
			CallSymbol(symbolsCurrent);
		}
		if (coldTime<=0 && !attacked)
		{
			canAttack = true;
			attacked = true;
			CallSymbol(symbols);
			coldTime = -1;
			group.SetWeaponReady();
		}
	}

	private void CallSymbol(Dictionary<Vector2Int,SymbolSO> symbolsResult)
	{
		WeaponCallSymbol.RaiseEvent(symbolsResult,this);//传递字典<位置，符文>
	}
	public void EndSymbolAttack()
	{
		attacked = false;
		canAttack = false;
		if (weapon != null)
		{
			symbols.Clear();
			foreach (var symbolList in weapon.symbolList)
			{
				foreach (var areaPos in symbolList.area)
				{
					symbols.Add(areaPos, symbolList.symbol);
				}
			}
			coldTime = symbols.Count;
		}
	}

	public void SetJewelryGrid(object o)
	{
		if (weapon.type == WeaponType.珠宝)
		{
			Vector2Int gridPos = (Vector2Int)o;
			foreach (var symbolList in weapon.symbolList)
			{
				if (symbolList.selectArea.Count > 0)
				{
					symbolList.area.Clear();
					symbolList.area.Add(gridPos);
					ResetWeapon();
					group.ResetHurtArea();
					break;
				}
			}
		}
	}
}
