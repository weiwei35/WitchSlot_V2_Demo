using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// 显示玩家在地图中的攻击范围
/// </summary>
public class GridView_Map : MonoBehaviour 
{
	public GridController gridController;
	
	[Header("攻击范围")]
	public Color gridColor;
	public Color gridColorRed;
	public Player player;
	public GridObj gridObj;
	public GameObject gridParent;
	public List<GridObj> gridObjs;
	public float gridSize = 0.8f;
	
	bool isShowHurt = false;
	public void SetGrid(Dictionary<Vector2Int, SymbolSO> symbolDic)
	{
		gridObjs.Clear();
		isShowHurt = false;
		foreach (Transform grid in gridParent.transform)
		{
			Destroy(grid.gameObject);
		}
		var pos = Vector3.zero;
		foreach (var grid in symbolDic)
		{
			var gridPos = new Vector3(grid.Key.x, grid.Key.y)*gridSize/*- new Vector3(gridController.playerPos.x, gridController.playerPos.y)+player.transform.position*/;
			var obj = Instantiate(gridObj, gridParent.transform);
			obj.transform.localPosition = gridPos;
			obj.gameObject.SetActive(false);
			obj.symbol = grid.Value;
			obj.gridPos = new Vector3(grid.Key.x, grid.Key.y);
			gridObjs.Add(obj);
		}
		gridParent.transform.localPosition = new Vector3(-GridController.playerFaceGridPosCurrent.x, -GridController.playerFaceGridPosCurrent.y)*gridSize;
	}

	public void ShowGrid()
	{
		if (gridObjs != null)
			foreach (var grid in gridObjs)
			{
				grid.GetComponent<SpriteRenderer>().material.color = gridColor;
				grid.gameObject.SetActive(true);
				if(!isShowHurt) SetHurt();
			}
	}
	public GameObject hurtObj;
	public GameObject symbolObj;
	
	Dictionary<Vector2,GameObject> hurtObjDict = new Dictionary<Vector2,GameObject>();
	private List<GameObject> symbolList = new List<GameObject>();
	public void SetHurt()
	{
		foreach (var grid in gridObjs)
		{
			if (grid.transform.childCount>0)
			{
				foreach (Transform gridChild in grid.transform)
				{
					Destroy(gridChild.gameObject);
				}
			}
		}
		hurtObjDict.Clear();
		symbolList.Clear();
		foreach (var grid in gridObjs)
		{
			Vector3 gridPos = new Vector3(grid.gridPos.x, grid.gridPos.y);
			var symbol = Instantiate(symbolObj, grid.transform);
			symbol.GetComponent<SpriteRenderer>().sprite = grid.symbol.symbolIcon;
			symbolList.Add(symbol);
			if (grid.symbol.symbolAttacks.Count > 0)
			{
				//多个攻击格子
				foreach (var gridChild in grid.symbol.symbolAttacks)
				{
					Vector3 position = gridPos + gridChild.position;
					if (hurtObjDict.ContainsKey(position))
					{
						float t = Convert.ToInt32(hurtObjDict[position].GetComponent<TMP_Text>().text);
						t += gridChild.attack;
						hurtObjDict[position].GetComponent<TMP_Text>().text = t.ToString();
					}
					else
					{
						var gridObjChild = Instantiate(hurtObj, grid.transform);
						gridObjChild.transform.localPosition = gridChild.position;
                        gridObjChild.GetComponent<TMP_Text>().text = gridChild.attack.ToString();
                        hurtObjDict.Add(position, gridObjChild);
					}
				}
			}
			else
			{
				if (hurtObjDict.ContainsKey(gridPos))
				{
					float t = Convert.ToInt32(hurtObjDict[gridPos].GetComponent<TMP_Text>().text);
					t += grid.symbol.symbolAttack;
					hurtObjDict[gridPos].GetComponent<TMP_Text>().text = t.ToString();
				}
				else
				{
					var gridObj = Instantiate(hurtObj, grid.transform);
					gridObj.transform.localPosition = Vector3.zero;
                    gridObj.GetComponent<TMP_Text>().text = grid.symbol.symbolAttack.ToString();
                    hurtObjDict.Add(gridPos, gridObj);
				}
			}
		}
		isShowHurt = true;
		// HideSymbol();
	}

	bool isHideSymbol = false;
	private void Update()
	{
		if (isShowHurt)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if(!isHideSymbol) HideSymbol();
				else ShowSymbol();
			}
		}
	}

	void HideSymbol()
	{
		isHideSymbol = true;
		foreach (var symbol in symbolList)
		{
			symbol.SetActive(false);
		}
		foreach (var grid in gridObjs)
		{
			grid.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	void ShowSymbol()
	{
		isHideSymbol = false;
		foreach (var symbol in symbolList)
		{
			symbol.SetActive(true);
		}
		foreach (var grid in gridObjs)
		{
			grid.GetComponent<SpriteRenderer>().enabled = true;
		}
	}
}
