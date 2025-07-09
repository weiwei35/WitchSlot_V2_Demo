using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class WeaponGroup : MonoBehaviour 
{
	public WeaponManager manager;
	public GameObject weaponGrid_top;
	public GameObject weaponGrid_weapon;
	public GameObject weaponGrid_wear;
	public GameObject weaponGrid_jewelry;

	public WeaponItem_UI weaponItem;

	public void SetWeaponItem()
	{
		foreach (var weapon in manager.playerWeaponLibrary.weapons)
		{
			var weaponObj = Instantiate(weaponItem);
			switch (weapon.type)
			{
				case WeaponType.帽子:
					weaponObj.transform.SetParent(weaponGrid_top.transform);
					weaponObj.weapon = weapon;
					weaponObj.Init();
					break;
				case WeaponType.武器:
					weaponObj.transform.SetParent(weaponGrid_weapon.transform);
					weaponObj.weapon = weapon;
					weaponObj.Init();
					break;
				case WeaponType.珠宝:
					weaponObj.transform.SetParent(weaponGrid_jewelry.transform);
					weaponObj.weapon = weapon;
					weaponObj.Init();
					break;
				case WeaponType.衣服:
					weaponObj.transform.SetParent(weaponGrid_wear.transform);
					weaponObj.weapon = weapon;
					weaponObj.Init();
					break;
			}
			weaponObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			weaponObj.transform.localScale = Vector3.one;
		}
	}
}
