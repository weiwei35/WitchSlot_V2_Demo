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

public class SymbolManager : MonoBehaviour 
{
	public List<SymbolSO> symbolDataList;//所有符文
	public SymbolLibrarySO symbolLibrary;//初始牌堆
	public SymbolLibrarySO playerSymbolLibrary;//玩家牌堆

	private void Start()
	{
		InitCardData();
	}

	public void InitCardData(){
		Addressables.LoadAssetsAsync<SymbolSO>("SymbolData",null).Completed += OnCardDataLoaded;
	}

	private void OnCardDataLoaded(AsyncOperationHandle<IList<SymbolSO>> handle)
	{
		if(handle.Status == AsyncOperationStatus.Succeeded){
			symbolDataList = new List<SymbolSO>(handle.Result);
		}
		else{
			Debug.LogError("No Card Data");
		}
	}

}
