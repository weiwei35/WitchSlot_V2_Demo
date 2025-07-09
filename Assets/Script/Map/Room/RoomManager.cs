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

public class RoomManager : MonoBehaviour 
{
	public List<RoomDataSO> roomsAll = new List<RoomDataSO>();

	private void Start()
	{
		initSkillData();
	}

	public void initSkillData(){
		Addressables.LoadAssetsAsync<RoomDataSO>("RoomData",null).Completed += OnDataLoaded;
	}

	private void OnDataLoaded(AsyncOperationHandle<IList<RoomDataSO>> handle)
	{
		if(handle.Status == AsyncOperationStatus.Succeeded){
			roomsAll = new List<RoomDataSO>(handle.Result);
		}
		else{
			Debug.LogError("No Room Data");
		}
	}
}
