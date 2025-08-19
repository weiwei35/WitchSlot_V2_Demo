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
				Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
				GameObject[] pos = GameObject.FindGameObjectsWithTag("SetPos");
				GameObject startPos = GameObject.FindGameObjectWithTag("CenterPos");
				int index = 0;
				foreach (var enemy in roomData.enemies)
				{
					var enemyObj = Instantiate(enemy.enemy, enemyGroup.transform);
					enemyObj.transform.position = pos[index].transform.position;
					ToolFunctions.SetEnemyHP(enemyObj);
					enemyGroup.enemies.Add(enemyObj);
					index++;
				}
				player.transform.position = startPos.transform.position;
				enemyGroup.enemyAllDie = false;
			}
			CalculateEffectiveBounds(room.GetComponent<RoomController>().wallTilemap);
		}
	}

	void DeleteRoomBefore()
	{
		foreach (Transform room in transform)
		{
			Destroy(room.gameObject);
		}
	}
	
 	public Rect effectiveRect;
 	public Vector3[] cornerPoints;
    void CalculateEffectiveBounds(Tilemap tilemap)
    {
	    // 获取 Tilemap 的 cellBounds
	    BoundsInt bounds = tilemap.cellBounds;
	    // 初始化为极大/极小值
	    float minX = float.MaxValue;
	    float minY = float.MaxValue;
	    float maxX = float.MinValue;
	    float maxY = float.MinValue;
	    // 遍历 Tilemap 的每个单元格
	    foreach (Vector3Int cellPos in bounds.allPositionsWithin)
	    {
		    if (tilemap.HasTile(cellPos))
		    {
			    // 转换为世界坐标（使用 CellToWorld 正确处理缩放/偏移）
			    Vector3 worldPos = tilemap.CellToWorld(cellPos);
			    minX = Mathf.Min(worldPos.x, minX);
			    minY = Mathf.Min(worldPos.y, minY);
			    maxX = Mathf.Max(worldPos.x, maxX);
			    maxY = Mathf.Max(worldPos.y, maxY);
		    }
	    }
	    // 构造包围盒区域（AABB）
	    effectiveRect = new Rect(
		    minX,
		    minY,
		    maxX - minX,
		    maxY - minY
	    );
	    // 四个角点坐标（以世界坐标为单位）
	    cornerPoints = new Vector3[]
	    {
		    new Vector3(minX, maxY, 0),        // 左上角
		    new Vector3(maxX, maxY, 0),        // 右上角
		    new Vector3(minX, minY, 0),        // 左下角
		    new Vector3(maxX, minY, 0)         // 右下角
	    };
        int mapWidth = bounds.size.x;
        int mapHeight = bounds.size.y;
        Testing_PathFinding.instance.SetGridSystem(mapWidth, mapHeight,cornerPoints[2]);
    }
}
