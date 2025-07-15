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
	bool needRefresh = false;
	List<Vector2Int> pos = new List<Vector2Int>();
	[Header("广播事件")] 
	public ObjectEventSO GridMoveEvent;
	public ObjectEventSO GridCallEvent;
	public ObjectEventSO WeaponCallSymbol;
	public ObjectEventSO WeaponRefreshSymbol;
	private void Start()
	{
		weaponItemUI = GetComponent<WeaponItem_UI>();
		weapon = weaponItemUI.weapon;
		coldTime = weapon.coldTime;
		pos = weapon.hurtArea.ToList();
	}

	public void ResetWeapon()
	{
		needRefresh = false;
		GridMove.rotateDirection = 0;
		if (weapon != null)
		{
			coldTime = weapon.coldTime;
			pos = weapon.hurtArea.ToList();
			CheckRotate();
		}
	}
	public void PlayerWalk()
	{
		if (needRefresh)
		{
			CheckRotate();
			WeaponRefreshSymbol.RaiseEvent(pos,this);
			StartCoroutine(CallSymbolDelay());
		}
		if (coldTime > 0)
			coldTime--;
		if (coldTime==0)
		{
			CheckRotate();
			CallSymbol();
			coldTime = -1;
			needRefresh = true;
		}
	}

	IEnumerator CallSymbolDelay()
	{
		yield return new WaitForSeconds(0.2f);
		CallSymbol();
	}

	private void CallSymbol()
	{
		GridCallEvent.RaiseEvent(null,this);
		GridMoveEvent.RaiseEvent(null,this);
		WeaponCallSymbol.RaiseEvent(pos,this);
	}

	private void CheckRotate()
	{
		pos.Clear();
       	foreach (var hurtPos in weapon.hurtArea)
       	{
       		Vector2Int posRotate = ToolFunctions.RotateGridInt(hurtPos,GridMove.rotateDirection);
       		pos.Add(posRotate);
       	}
	}
}
