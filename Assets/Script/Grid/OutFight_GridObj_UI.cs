using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class OutFight_GridObj_UI : MonoBehaviour
{
	public Vector2Int gridPos;
	private OutFight_GridPick_UI gridPick;
	
	public bool selected = false;

	private void Start()
	{
		gridPick = GetComponentInParent<OutFight_GridPick_UI>();
		// GetComponent<Button>().onClick.AddListener(() =>
		// {
		// 	selected = !selected;
		// 	if(selected)
		// 	{
		// 		gridPick.gridObjects.Add(this);
		// 		gridPick.gridCount--;
		// 		gridPick.SetGridCanPick();
		// 	}
		// 	else
		// 	{
		// 		gridPick.gridCount++;
		// 		gridPick.RemoveGrid(this);
		// 	}
		// });
	}
	private void Update()
	{
		if (selected)
		{
			GetComponent<Image>().color = Color.green;
		}
		else
		{
			GetComponent<Image>().color = Color.white;
		}
	}
}
