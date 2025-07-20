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
	
	bool canAttack = false;
	
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
		foreach (var grid in gridObjs.ToList())
		{
			Destroy(grid.gameObject);
		}
		gridObjs.Clear();
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
		canAttack = true;
	}

	public void ShowGrid()
	{
		foreach (var grid in gridObjs)
		{
			grid.GetComponent<SpriteRenderer>().material.color = gridColor;
			grid.gameObject.SetActive(true);
		}
		SetHurt();
	}

	public void ClearGrid()
	{
		foreach (var grid in gridObjs)
		{
			Destroy(grid.gameObject);
		}
		gridObjs.Clear();
		SetHurt();
		isHideSymbol = true;
		isShowHurt = false;
		canAttack = false;
	}
	public GameObject hurtObj;
	public GameObject symbolObj;
	public Dictionary<Vector2,GameObject> hurtObjDict = new Dictionary<Vector2,GameObject>();
	private List<GameObject> symbolList = new List<GameObject>();
	public void SetHurt()
	{
		hurtObjDict.Clear();
		symbolList.Clear();
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
		foreach (var grid in gridObjs)
		{
			Vector3 gridPos = new Vector3(grid.gridPos.x, grid.gridPos.y);
			var symbol = Instantiate(symbolObj, grid.transform);
			symbol.GetComponent<SpriteRenderer>().sprite = grid.symbol.symbolIcon;
			symbolList.Add(symbol);
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

		if (!endAttack)
		{
			if(!isShowHurt)
            	HideSymbol();
            else
            	ShowSymbol();
		}
	}

	bool isHideSymbol = true;
	bool endAttack = false;

	public void SetEndAttack()
	{
		endAttack = true;
		isShowHurt = false;
		StartCoroutine(SetAttack());
	}
	IEnumerator SetAttack()
	{
		yield return new WaitForSeconds(0.5f);
		endAttack = false;
	}
	private void Update()
	{
		if(!canAttack) return;
		if (!isShowHurt)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if(isHideSymbol)
				{
					ShowSymbol();
					isShowHurt = true;
				}
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if(!isHideSymbol)
				{
					HideSymbol();
					isShowHurt = false;
				}
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

		foreach (var hurt in hurtObjDict)
		{
			hurt.Value.SetActive(false);
		}
		foreach (var grid in gridObjs)
		{
			grid.GetComponent<SpriteRenderer>().enabled = false;
		}
		SymbolCanNotAttackEvent.RaiseEvent(null,this);
	}

	public ObjectEventSO SymbolCanAttackEvent;
	public ObjectEventSO SymbolCanNotAttackEvent;
	void ShowSymbol()
	{
		isHideSymbol = false;
		foreach (var symbol in symbolList)
		{
			symbol.SetActive(true);
		}
		foreach (var hurt in hurtObjDict)
		{
			hurt.Value.SetActive(true);
		}
		foreach (var grid in gridObjs)
		{
			grid.GetComponent<SpriteRenderer>().enabled = true;
		}
		SymbolCanAttackEvent.RaiseEvent(null,this);
	}
}
