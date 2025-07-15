using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// 在UI中显示攻击
/// </summary>
public class GridView_UI : MonoBehaviour 
{
	//显示格子内符文
	public GameObject gridGroup;
	public GridPosition gridObj;
	public SymbolView_UI symbol;
	
	GridPosition gridPlayer;
	public Sprite playerSprite;
	
	List<GridPosition> gridList = new List<GridPosition>();
	public void InitGrid(object o)
	{
		gridList.Clear();
		foreach (Transform grid in gridGroup.transform)
		{
			Destroy(grid.gameObject);
		}
		List<Vector2Int> gridPos = ((List<Vector2Int>)o).ToList();
		float minY = float.MaxValue;
		foreach (var pos in gridPos)
		{
			var grid = Instantiate(gridObj, gridGroup.transform);
			grid.gridPosition = new Vector2Int((int)pos.x, (int)pos.y);
			grid.transform.localPosition = new Vector3(pos.x,pos.y)*100;
			
			gridList.Add(grid);
		}
		gridPlayer = Instantiate(gridObj, gridGroup.transform);
		gridPlayer.gridPosition = new Vector2Int(0,0);
		gridPlayer.transform.localPosition = new Vector3(0,0,0);
		gridPlayer.GetComponent<Image>().sprite = playerSprite;
		gridPlayer.GetComponent<Image>().SetNativeSize();
		foreach (Transform grid in gridGroup.transform)
		{
			if(grid.transform.localPosition.y < minY) minY = grid.transform.localPosition.y;
		}
		if (minY <= 0)
		{
			foreach (Transform grid in gridGroup.transform)
			{
				// grid.transform.localPosition -= new Vector3(0, minY-100, 0);
			}
		}
	}
	//在格子区域显示召唤出的符文
	public void SetGridSymbol(Dictionary<Vector2Int,SymbolSO> symbolDic)
	{
		ClearGrid();
		foreach (Transform grid in gridGroup.transform)
		{
			GridPosition gridPosition = grid.GetComponent<GridPosition>();
			if (symbolDic.ContainsKey(gridPosition.gridPosition))
			{
				SymbolView_UI gridSymbol = Instantiate(symbol, grid);
				gridSymbol.symbolData = symbolDic[gridPosition.gridPosition];
				gridSymbol.SetSymbol();
			}
		}
	}

	//将格子槽位的坐标属性旋转，位置上不用旋转
	public void RotateGrid(object o)
	{
		int angle = (int)o;
		foreach (Transform grid in gridGroup.transform)
		{
			GridPosition gridPosition = grid.GetComponent<GridPosition>();
			gridPosition.gridPosition = ToolFunctions.RotateGridInt(gridPosition.gridPosition,angle);
		}
	}
	//清除符文
	public void ClearGrid()
	{
		foreach (Transform grid in gridGroup.transform)
		{
			if(grid.childCount>0) Destroy(grid.GetChild(0).gameObject);
		}
	}

	public Color selectColor;
	public Color defaultColor;
	public void ShowWeaponHurtArea(object o)
	{
		List<Vector2Int> gridPos = ((List<Vector2Int>)o).ToList();
		foreach (var pos in gridPos)
		{
			foreach (var grid in gridList)
			{
				if (grid.gridPosition == pos)
				{
					grid.GetComponent<Image>().color = selectColor;
				}
			}
		}
	}

	public void ResetWeaponHurtArea()
	{
		foreach (var grid in gridList)
		{
			grid.GetComponent<Image>().color = defaultColor;
		}
	}
}
