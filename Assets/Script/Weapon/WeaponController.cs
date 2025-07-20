using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
	List<Vector2Int> pos = new List<Vector2Int>();
	Dictionary<Vector2Int,SymbolSO> symbols = new Dictionary<Vector2Int,SymbolSO>();
	[Header("广播事件")] 
	public ObjectEventSO GridMoveEvent;
	public ObjectEventSO GridCallEvent;
	public DictionaryEventSO WeaponCallSymbol;
	private void Start()
	{
		weaponItemUI = GetComponent<WeaponItem_UI>();
		weapon = weaponItemUI.weapon;
		coldTime = weapon.coldTime;
		pos = weapon.hurtArea.ToList();

		foreach (var attackPos in pos)
		{
			symbols.Add(attackPos,weapon.symbol);
		}
	}

	public void ResetWeapon()
	{
		if (weapon != null)
		{
			coldTime = weapon.coldTime;
			pos.Clear();
			pos = weapon.hurtArea.ToList();
			symbols.Clear();
			foreach (var attackPos in pos)
			{
				symbols.Add(attackPos,weapon.symbol);
			}
			CheckRotate();
		}
	}
	public void PlayerWalk()
	{
		if (coldTime > 0)
			coldTime--;
		if (coldTime<=0)
		{
			CheckRotate();
			CallSymbol();
			coldTime = -1;
		}
	}

	private void CallSymbol()
	{
		GridCallEvent.RaiseEvent(null,this);
		GridMoveEvent.RaiseEvent(null,this);
		WeaponCallSymbol.RaiseEvent(symbols,this);//传递字典<位置，符文>
	}

	private void CheckRotate()
	{
		pos.Clear();
       	foreach (var hurtPos in weapon.hurtArea)
       	{
       		Vector2Int posRotate = ToolFunctions.RotateGridInt(hurtPos,GridMove.rotateDirection);
       		pos.Add(posRotate);
       	}
        symbols.Clear();
        foreach (var attackPos in pos)
        {
	        symbols.Add(attackPos,weapon.symbol);
        }
	}

	public void EndSymbolAttack()
	{
		if (weapon != null)
		{
			coldTime = weapon.coldTime;
			pos.Clear();
			pos = weapon.hurtArea.ToList();
			symbols.Clear();
			foreach (var attackPos in pos)
			{
				symbols.Add(attackPos,weapon.symbol);
			}
			CheckRotate();
		}
	}
}
