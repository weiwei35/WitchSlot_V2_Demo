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

public class BoosterManager : MonoBehaviour 
{
	public static BoosterManager instance;

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

	public List<BoosterDataSO> boosterDataList;//所有道具
	public LibraryData playerLibrary;//玩家牌堆

	private void Start()
	{
		InitCardData();
		playerLibrary.boosters.Clear();
	}

	private void InitCardData(){
		Addressables.LoadAssetsAsync<BoosterDataSO>("BoosterData",null).Completed += OnCardDataLoaded;
	}

	private void OnCardDataLoaded(AsyncOperationHandle<IList<BoosterDataSO>> handle)
	{
		if(handle.Status == AsyncOperationStatus.Succeeded){
			boosterDataList = new List<BoosterDataSO>(handle.Result);
		}
		else{
			Debug.LogError("No Booster Data");
		}
	}

	public void GetBooster(object o)
	{
		BoosterDataSO booster = (BoosterDataSO)o;
		playerLibrary.boosters.Add(booster);
		foreach (var boosterData in boosterDataList.ToList())
		{
			if(boosterData == booster) boosterDataList.Remove(boosterData);
		}
	}
}
