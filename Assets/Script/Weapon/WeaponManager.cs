using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class WeaponManager : MonoBehaviour 
{
	public static WeaponManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	public List<WeaponSO> weaponDataList;//所有符文
	public LibraryData playerWeaponLibrary;//玩家牌堆

	private void Start()
	{
		InitCardData();
	}

	private void InitCardData(){
		Addressables.LoadAssetsAsync<WeaponSO>("WeaponData",null).Completed += OnCardDataLoaded;
	}

	private void OnCardDataLoaded(AsyncOperationHandle<IList<WeaponSO>> handle)
	{
		if(handle.Status == AsyncOperationStatus.Succeeded){
			weaponDataList = new List<WeaponSO>(handle.Result);
			foreach (var weapon in weaponDataList)
			{
				if (weapon.type == WeaponType.珠宝)
				{
					foreach (var symbolList in weapon.symbolList)
					{
						if(symbolList.selectArea.Count>0) symbolList.area.Clear();
					}
				}
			}
		}
		else{
			Debug.LogError("No Weapon Data");
		}
	}
}
