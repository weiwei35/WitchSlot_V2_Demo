using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;
/// <summary>
/// 控制玩家绘制格子攻击范围
/// 记录当前所有格子的坐标
/// 所有格子必须相邻
/// </summary>
public class DrawGridCanvas : MonoBehaviour
{
	public int width;
	public int height;
	public List<DrawGridObj> gridObjects;

	public int gridCount;
	public int gridCount_max;
	public TMP_Text gridCountText;

	public ObjectEventSO SetGridArea;

	private void Start()
	{
		gridCount_max = gridCount;
		for (int i = 0; i < gridObjects.Count; i++)
		{
			gridObjects[i].index = i + 1;
		}
	}

	private void Update()
	{
		gridCountText.text = gridCount.ToString();
	}

	private List<int> GetSelectGridIndex()
	{
		List<int> selectGridIndex = new List<int>();
		foreach (var obj in gridObjects)
		{
			if(obj.isSelected && obj.isActived)
			{
				selectGridIndex.Add(obj.index);
			}
		}
		return selectGridIndex;
	}

	//当前格子是否与整体区域相邻
	public bool IsGridCanSelect(int index)
	{
		//连续区域周围的所有格子
		List<int> nearIndex = new List<int>();
		foreach (var gridIndex in GetSelectGridIndex())
		{
			if (gridIndex != index)
			{
				if (gridIndex % height != 0)
				{
					int i = gridIndex+1;
					nearIndex.Add(i);
				}
				if (gridIndex % height != 1)
				{
					int i = gridIndex-1;
					nearIndex.Add(i);
				}
				if (gridIndex > height)
				{
					int i = gridIndex-height;
					nearIndex.Add(i);
				}
				if (gridIndex <= height * (width-1))
				{
					int i = gridIndex+height;
					nearIndex.Add(i);
				}
			}
		}
		if(gridCount == gridCount_max-1) return true;
		if(nearIndex.Contains(index)) return true;
		return false;
	}
	
	//计算选择区域的坐标
	List<Vector2> gridPos = new List<Vector2>();
	// 最终坐标存储：blockID -> 新坐标
	private Dictionary<int, Vector2Int> finalCoordinates = new Dictionary<int, Vector2Int>();
	// 中心方块索引
	private int centerBlockIndex = -1;
	public GridView_UI gridUI;

	public void GetGridPos()
	{
		gridPos.Clear();
		foreach (var grid in gridObjects)
		{
			if(grid.isSelected)
				gridPos.Add(grid.transform.position);
		}
	}
	/// <summary>
	/// 核心计算：确定中心方块并构建坐标系统
	/// </summary>
	void CalculateCoordinateSystem()
	{
		// 1. 计算所有方块的几何中心
		Vector2 centroid = CalculateCentroid();
        
		// 2. 选定距离几何中心最近的方块作为中心方块
		centerBlockIndex = FindCenterBlock(centroid);
        
		// 3. 构建坐标系统（以中心方块为原点）
		Vector2 centerPos = gridPos[centerBlockIndex];
        
		for (int i = 0; i < gridPos.Count; i++)
		{
			Vector2 pos = gridPos[i];
			Vector2Int newCoord = new Vector2Int(
				Mathf.RoundToInt(pos.x - centerPos.x)/120,
				Mathf.RoundToInt(pos.y - centerPos.y)/120
			);
			finalCoordinates.Add(i, newCoord);
		}
	}
	/// <summary>
	/// 计算方块集合的几何中心
	/// </summary>
	Vector2 CalculateCentroid()
	{
		Vector2 sum = Vector2.zero;
		foreach (Vector2 pos in gridPos)
		{
			sum += pos;
		}
		return sum / gridPos.Count;
	}
	/// <summary>
	/// 寻找距离中心点最近的方块
	/// </summary>
	int FindCenterBlock(Vector2 centroid)
	{
		float minDistance = float.MaxValue;
		int centerIndex = 0;
        
		for (int i = 0; i < gridPos.Count; i++)
		{
			float dist = Vector2.Distance(gridPos[i], centroid);
            
			// 优先选择距离最近的点
			if (dist < minDistance)
			{
				minDistance = dist;
				centerIndex = i;
			}
			// 距离相同时选择最小坐标值的方块（保证一致性）
			else if (Mathf.Approximately(dist, minDistance))
			{
				if (gridPos[i].x < gridPos[centerIndex].x || 
				    (Mathf.Approximately(gridPos[i].x, gridPos[centerIndex].x) && 
				     gridPos[i].y < gridPos[centerIndex].y))
				{
					centerIndex = i;
				}
			}
		}
        
		return centerIndex;
	}
	/// <summary>
	/// 输出结果（实际使用时可替换为实际游戏逻辑）
	/// </summary>
	void LogResults()
	{
		Debug.Log("--- 方块坐标系统已建立 ---");
		Debug.Log($"中心方块: #{centerBlockIndex} => (0,0)");
        
		foreach (var kvp in finalCoordinates)
		{
			Debug.Log($"方块 #{kvp.Key}: {gridPos[kvp.Key]} → {kvp.Value}");
		}
	}
}
