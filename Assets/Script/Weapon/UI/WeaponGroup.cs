using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class WeaponGroup : MonoBehaviour 
{
	public WeaponManager manager;
	public BagController bagController;
	public GameObject weaponGrid_top;
	public GameObject weaponGrid_weapon;
	public GameObject weaponGrid_wear;
	public GameObject weaponGrid_jewelry;

	public WeaponItem_UI weaponItem;

	public List<WeaponItem_UI> weaponList;
	
	public void SetWeaponItem()
	{
		foreach (var weapon in manager.playerWeaponLibrary.weapons)
		{
			if(weaponList.Any(item => item.weapon == weapon)) continue;
			var weaponObj = Instantiate(weaponItem);
			weaponList.Add(weaponObj);
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

	public void TakeOnWeapon(WeaponSO weapon)
	{
		var weaponObj = Instantiate(weaponItem);
		weaponList.Add(weaponObj);
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
	[ContextMenu("GetRandomWeapon")]//菜单中调用方法
	public WeaponSO GetRandomWeapon()
	{
		List<WeaponSO> weaponCanGet = new();
		foreach (var weapon in manager.weaponDataList)
		{
			if(!manager.playerWeaponLibrary.weapons.Contains(weapon))
				weaponCanGet.Add(weapon);
		}

		WeaponSO getWeapon = weaponCanGet[Random.Range(0, weaponCanGet.Count)];
		
		bool addToBag = false;
		foreach (var weapon in weaponList)
		{
			if(weapon.weapon.type == getWeapon.type) addToBag = true;
		}

		manager.playerWeaponLibrary.weapons.Add(getWeapon);
		if (addToBag)
		{
			PutWeaponInBag(getWeapon);
		}
		else
		{
			SetWeaponItem();
			ResetHurtArea();
		}
		return getWeapon;
	}

	private void PutWeaponInBag(WeaponSO weapon)
	{
		bagController.AddWeapon(weapon);
	}
	private void RemoveWeaponFromBag(WeaponSO weapon)
	{
		bagController.RemoveWeapon(weapon);
	}
	public ObjectEventSO ChangeGridHurtArea;
	public void ChangeWeapon(WeaponSO weapon)
	{
		for (int i = 0; i < weaponList.Count; i++)
		{
			if (weaponList[i].weapon.type == weapon.type)
			{
				//替换当前两个装备
				WeaponSO weaponOn = weapon;
				WeaponSO weaponOff = weaponList[i].weapon;
				Destroy(weaponList[i].GameObject());
				weaponList.RemoveAt(i);
				RemoveWeaponFromBag(weaponOn);
				PutWeaponInBag(weaponOff);
				TakeOnWeapon(weaponOn);
				break;
			}
		}
		ResetHurtArea();
	}

	private void ResetHurtArea()
	{
		//重新设置攻击区域
		List<WeaponSO> currentWeapons = weaponList.Select(obj => obj.weapon).ToList();
		List<Vector2Int> gridPos = new List<Vector2Int>();
		gridPos = ToolFunctions.SetGrid(currentWeapons);
		ChangeGridHurtArea.RaiseEvent(gridPos,this);
	}
}
