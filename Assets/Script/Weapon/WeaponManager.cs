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
	public List<WeaponSO> weaponDataList;//所有符文
	public WeaponLibraryData playerWeaponLibrary;//玩家牌堆

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
		}
		else{
			Debug.LogError("No Weapon Data");
		}
	}
}
