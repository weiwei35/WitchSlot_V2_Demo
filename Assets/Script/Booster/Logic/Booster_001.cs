using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Booster_001 : Booster_Move 
{
	public override void BoosterEffect()
	{
		int hurtCounter = 0;
		foreach (var weapon in WeaponManager.instance.playerWeaponLibrary.weapons)
		{
			foreach (var symbolList in weapon.symbolList)
			{
				if (symbolList.symbol.effects[0].effectType == EffectType.伤害)
				{
					hurtCounter += symbolList.area.Count;
				}
			}
		}
		// Debug.Log("对目标："+playerFacePos+"造成伤害："+hurtCounter*hurtAmount);
		StartCoroutine(SetEnemyHurt(playerFacePos, hurtCounter * hurtAmount));
		var effectObj = Instantiate(effect, playerFacePos, Quaternion.identity);
	}
}
