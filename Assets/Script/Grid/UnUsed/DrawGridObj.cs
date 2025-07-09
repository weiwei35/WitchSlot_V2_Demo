using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class DrawGridObj : MonoBehaviour 
{
	public bool isSelected = false;
	public bool isActived = false;
	public int index;

	Image image;
	Button button;
	DrawGridCanvas gridCanvas;

	private void Start()
	{
		gridCanvas = GetComponentInParent<DrawGridCanvas>();
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		button.onClick.AddListener(() =>
		{
			if (gridCanvas.gridCount > 0)
			{
				if (isSelected) gridCanvas.gridCount++;
				else gridCanvas.gridCount--;
				isSelected = !isSelected;
				isActived = !isActived;
			}else if (isSelected)
			{
				gridCanvas.gridCount++;
				isSelected = !isSelected;
			}
		});
	}

	private void Update()
	{
		if (isSelected)
		{
			if(gridCanvas.IsGridCanSelect(index))
			{
				isActived = true;
				image.color = Color.green;
			}
			else
			{
				isActived = false;
				image.color = Color.red;
			}
		}
		else
		{
			isActived = false;
			image.color = Color.white;
		}
	}
}
