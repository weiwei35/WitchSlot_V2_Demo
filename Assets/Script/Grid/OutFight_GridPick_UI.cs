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
/// 每次生成可选的格子
/// 玩家选择后再生成
/// </summary>
public class OutFight_GridPick_UI : MonoBehaviour 
{
	List<Vector2Int> points = new List<Vector2Int>
	{
		new (1,0),new (0,1),new (0,-1)
	};
	// public List<DrawGridObj> gridObjects;,new (-1,0)
	public List<OutFight_GridObj_UI> gridObjects;
	public OutFight_GridObj_UI gridObjectPrefab;
	public GameObject gridObjectParent;

	public GridView_UI gridUI;
	public int gridCount;
	public TMP_Text gridCountText;
	public Button submitButton;

	public ObjectEventSO SetGridArea;

	[Header("玩家面向")] public GameObject playerFace;
	public static Vector2Int playerFaceGridPos;

	private bool isPicking = true;
	private void Start()
	{
		SetGridCanPick();
	}
	private void Update()
	{
		gridCountText.text = gridCount.ToString();

		if (gridCount == 0 && isPicking)
		{
			isPicking = false;
			StopPick();
		}

		if (gridCount > 0)
		{
			submitButton.gameObject.SetActive(false);
			isPicking = true;
		}
	}

	private void StopPick()
	{
		foreach (Transform grid in gridObjectParent.transform)
		{
			if (!grid.GetComponent<OutFight_GridObj_UI>().selected)
			{
				Destroy(grid.gameObject);
			}
		}
		
		submitButton.gameObject.SetActive(true);
		// submitButton.onClick.AddListener(SubmitGrid);
	}

	public void SubmitGrid()
	{
		// 执行中心化转换
		List<Vector2Int> gridPoints = new List<Vector2Int>();
		foreach (Transform grid in gridObjectParent.transform)
		{
			if (grid.GetComponent<OutFight_GridObj_UI>().selected)
			{
				gridPoints.Add(grid.GetComponent<OutFight_GridObj_UI>().gridPos);
			}
		}
		var result = PointSetCenterizer.RecenterPointSet(gridPoints);
		playerFaceGridPos = result.RecenteredPoints[0];
		gridUI.InitGrid(result.RecenteredPoints);
		SetGridArea.RaiseEvent(result.RecenteredPoints,this);
	}
	public void RemoveGrid(OutFight_GridObj_UI gridObj)
	{
		gridObjects.Remove(gridObj);
		Destroy(gridObj.gameObject);
		foreach (Transform grid in gridObjectParent.transform)
		{
			if (!grid.GetComponent<OutFight_GridObj_UI>().selected)
			{
				Destroy(grid.gameObject);
			}
		}
		SetGridCanPick();
		CheckConnect();
	}
	public void SetGridCanPick()
	{
		if (gridObjects.Count == 0)
		{
			var grid = Instantiate(gridObjectPrefab, gridObjectParent.transform);
			grid.transform.localPosition = Vector3.zero;
			grid.gridPos = new Vector2Int(0, 0);
			playerFace.transform.position = gridObjectParent.transform.GetChild(0).position-new Vector3(100,0,0);
		}
		else
		{
			//动态加载当前可选格子
			SetNearGrid();
		}
	}
	private int deletMinY = int.MaxValue;
	private int deletMaxY = int.MinValue;
	private int deletMinX = int.MaxValue;
	private int deletMaxX = int.MinValue;
	private bool hasSideX = false;
	private bool hasSideY = false;
	private void CheckAreaSize(List<Vector2Int> nears)
	{
		int minY = Int32.MaxValue;
		int maxY = Int32.MinValue;
		int minX = Int32.MaxValue;
		int maxX = Int32.MinValue;
		hasSideX = false;
		hasSideY = false;
		for (int i = 0; i < nears.Count; i++)
		{
			if(nears[i].y < minY)
			{
				minY = nears[i].y;
			}
			if(nears[i].y > maxY)
			{
				maxY = nears[i].y;
			}
			if (maxY - minY >= 5 && !hasSideY)
			{
				deletMinY = minY;
				deletMaxY = maxY;
				hasSideY = true;
			}
			if(nears[i].x < minX)
			{
				minX = nears[i].x;
			}
			if(nears[i].x > maxX)
			{
				maxX = nears[i].x;
			}
			if (maxX - minX >= 6 && !hasSideX)
			{
				deletMinX = minX;
				deletMaxX = maxX;
				hasSideX = true;
			}
		}
	}
	private List<Vector2Int> RemoveUnenableNearGrid(List<Vector2Int> nears)
	{
		CheckAreaSize(nears);
		foreach (var near in nears.ToList())
		{
			if (hasSideX)
			{
				if (near.x >= deletMaxX || near.x <= deletMinX) nears.Remove(near);
			}
		}
		foreach (var near in nears.ToList())
		{
			if (hasSideY)
			{
				if (near.y >= deletMaxY || near.y <= deletMinY) nears.Remove(near);
			}
		}
		return nears;
	}
	private void SetNearGrid()
	{
		foreach (Transform grid in gridObjectParent.transform)
		{
			if (!grid.GetComponent<OutFight_GridObj_UI>().selected)
			{
				Destroy(grid.gameObject);
			}
		}
		List<Vector2Int> pointsNear = new List<Vector2Int>();
		List<Vector2Int> pointsPicked = new List<Vector2Int>();
		foreach (var gridPicked in gridObjects)
		{
			pointsPicked.Add(gridPicked.gridPos);
		}

		foreach (var pos in pointsPicked)
		{
			foreach (var nearPos in points)
			{
				if (!pointsPicked.Contains(pos + nearPos) && !pointsNear.Contains(pos + nearPos))
				{
					pointsNear.Add(pos + nearPos);
				}
			}
		}
		pointsNear = RemoveUnenableNearGrid(pointsNear);
		foreach (var near in pointsNear)
		{
			var grid = Instantiate(gridObjectPrefab, gridObjectParent.transform);
			grid.transform.localPosition = new Vector3(near.x,near.y)*120;
			grid.gridPos = near;
		}

		StartCoroutine(SetCenter());
	}

	IEnumerator SetCenter()
	{
		yield return new WaitForSeconds(0.1f);
		GetComponent<UICenterParent>().SetCenter();
		
		playerFace.transform.position = gridObjectParent.transform.GetChild(0).position-new Vector3(100,0,0);
	}
	void CheckConnect()
	{
		List<Vector2Int> pickedPos = new List<Vector2Int>();
		foreach (var pickedGrid in gridObjects)
		{
			pickedPos.Add(pickedGrid.gridPos);
		}

		foreach (var pickedGrid in gridObjects.ToList())
		{
			int i = 0;
			foreach (var pos in points)
			{
				if (pickedPos.Contains(pos + pickedGrid.gridPos)) break;
				i++;
			}
			if(i==points.Count && gridObjects.Count>1)//与当前所有选择格子不相邻
			{
				pickedGrid.selected = false;
				gridCount++;
				RemoveGrid(pickedGrid);
			}
		}
	}
}
