using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
	public ObjectEventSO EnterUndergroundEvent;
	private void OnEnable()
	{
		EnterUndergroundEvent.RaiseEvent(null,this);
		LoadFakeRoom();
	}
	public RoomDataSO fakeRoomData;

	public void LoadFakeRoom()
	{
		LoadRoom(fakeRoomData);
	}

	public void LoadRoom(object obj)
	{
		DeleteRoomBefore();
		RoomDataSO roomData = obj as RoomDataSO;
		if (roomData != null)
		{
			var room = Instantiate(roomData.roomPrefab, transform);
			//加载房间怪物
			if (roomData.roomType is RoomType.怪物 or RoomType.精英怪)
			{
				EnemyGroup enemyGroup = GameObject.FindWithTag("EnemyGroup").GetComponent<EnemyGroup>();
				GameObject[] pos = GameObject.FindGameObjectsWithTag("SetPos");
				int index = 0;
				foreach (var enemy in roomData.enemies)
				{
					var enemyObj = Instantiate(enemy.enemy, enemyGroup.transform);
					enemyObj.transform.position = pos[index].transform.position;
					ToolFunctions.SetEnemyHP(enemyObj);
					enemyGroup.enemies.Add(enemyObj);
					index++;
				}

				enemyGroup.enemyAllDie = false;
			}
		}
	}

	void DeleteRoomBefore()
	{
		foreach (Transform room in transform)
		{
			Destroy(room.gameObject);
		}
	}
}
