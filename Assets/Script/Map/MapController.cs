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
	//Asset文件保存路径
	private const string assetPath = "Assets/GameData/EnemyHP/";
	public IntEventSO enemyHPEvent;
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
			}

			StartCoroutine(RoomLoaded());
		}
	}

	void DeleteRoomBefore()
	{
		foreach (Transform room in transform)
		{
			Destroy(room.gameObject);
		}
	}

	IEnumerator RoomLoaded()
	{
		yield return new WaitForSeconds(1f);
		// FightController.instance.StartFight();
	}

	//TEST：引导遮罩
	public void SetGuidMask()
	{
		Tilemap tilemap = GameObject.FindWithTag("MapWall").GetComponent<Tilemap>();
		Bounds bounds = ToolFunctions.GetTilemapBounds(tilemap);
		// Debug.Log($"Tilemap范围: 左下角 {bounds.min}, 右上角 {bounds.max}");
		Vector3 maskCenter = bounds.center;
		Vector3[] corners = new Vector3[2];
		corners[0] = bounds.min;
		corners[1] = bounds.max;
		// ShowUIMask_Tutorial.instance.Guide(corners,maskCenter);
	}
}
