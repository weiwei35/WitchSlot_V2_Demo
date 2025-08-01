using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GridPosition : MonoBehaviour
{
	public Vector2Int gridPosition;
	public bool canSelect;
	public ObjectEventSO SelectJewelryGrid;

	private void Start()
	{
		Button button = GetComponent<Button>();
		button.onClick.AddListener(SelectGrid);
	}

	public void SelectGrid()
	{
		if (canSelect)
		{
			SelectJewelryGrid?.RaiseEvent(gridPosition,null);
		}
	}
}
