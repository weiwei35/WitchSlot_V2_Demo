using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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
	// public Sprite playerSprite;
	
	List<GridPosition> gridList = new List<GridPosition>();
	List<GridPosition> gridListExpanded = new List<GridPosition>();
	public void InitGrid(Dictionary<Vector2Int,SymbolSO> symbolDic)
	{
		foreach (var obj in gridListExpanded)
		{
			Destroy(obj.gameObject);
		}
		gridListExpanded.Clear();
		ClearGrid();
		gridList.Clear();
		Dictionary<Vector2Int, SymbolSO> symbolList = new Dictionary<Vector2Int, SymbolSO>();
		foreach (var symbol in symbolDic)
		{
			symbolList.Add(symbol.Key, symbol.Value);
		}
		// List<Vector2Int> gridPos = ((List<Vector2Int>)o).ToList();
		foreach (Transform grid in gridGroup.transform)
		{
			GridPosition pos = grid.GetComponent<GridPosition>();
			grid.GetComponent<Image>().color = unlockColor;
			if (symbolList.ContainsKey(pos.gridPosition))
			{
				gridList.Add(pos);
				grid.GetComponent<Image>().color = defaultColor;
				symbolList.Remove(pos.gridPosition);
			}
		}

		if (symbolList.Count > 0)
		{
			foreach (var pos in symbolList)
			{
				var grid = Instantiate(gridObj, gridGroup.transform);
				grid.gridPosition = pos.Key;
				grid.GetComponent<Image>().color = defaultColor;
				grid.transform.localPosition = new Vector3(pos.Key.x*120,pos.Key.y*120);
				gridList.Add(grid);
				gridListExpanded.Add(grid);
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
	//清除符文
	public void ClearGrid()
	{
		foreach (Transform grid in gridGroup.transform)
		{
			if(grid.childCount>0)
			{
				foreach (Transform gridChild in grid.transform)
				{
					Destroy(gridChild.gameObject);
				}
			}
		}
	}

	public Color selectColor;
	public Color defaultColor;
	public Color unlockColor;
	public void ShowWeaponHurtArea(object o)
	{
		// List<Vector2Int> gridPos = ((List<Vector2Int>)o).ToList();
		// foreach (var pos in gridPos)
  //       {
  //       	foreach (var grid in gridList)
  //       	{
  //       		if (grid.gridPosition == pos)
  //       		{
  //       			grid.GetComponent<Image>().color = selectColor;
  //       		}
  //       	}
  //       }
	}

	public void ResetWeaponHurtArea()
	{
		foreach (var grid in gridList)
		{
			if (grid != null) grid.GetComponent<Image>().color = defaultColor;
		}
	}

	public void ShowJewelryGrid(object o)
	{
		List<Vector2Int> gridPos = ((List<Vector2Int>)o).ToList();
		foreach (var pos in gridPos)
		{
			foreach (Transform grid in gridGroup.transform)
			{
				GridPosition gridPosition = grid.GetComponent<GridPosition>();
				if (gridPosition.gridPosition == pos)
				{
					grid.GetComponent<Image>().color = selectColor;
					gridPosition.canSelect = true;
				}
			}
		}
	}

	public void ResetJewelryGrid()
	{
		foreach (Transform grid in gridGroup.transform)
		{
			GridPosition gridPosition = grid.GetComponent<GridPosition>();
			gridPosition.canSelect = false;
		}
	}
}
