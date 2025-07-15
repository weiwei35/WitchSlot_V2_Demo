using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridMove : MonoBehaviour
{
	public static int rotateDirection = 0;
	
	private bool isEndSet = true;
	bool canMove = false;

	private GridView_Map gridGroup;
	private GridController gridController;
	private void Start()
	{
		gridGroup = GetComponent<GridView_Map>();
		gridController = GetComponent<GridController>();
	}

	public void SetMove()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		canMove = true;
		isEndSet = false;
	}

	public void EndSet()
	{
		canMove = false;
		isEndSet = true;
	}

	private void Update()
	{
		if (canMove && !isEndSet)
		{
			SetGridGroupPos();
			//todo:右键撤回攻击范围
			if (Input.GetMouseButtonDown(1))
			{
				// gridController.RotateGrid(90);
			}
		}
	}

	public void SetGridGroupRotate()
	{
		Direction dir = GetFacePos();
		switch (dir)
		{
			case Direction.Up:
				gridController.RotateGrid(270);
				rotateDirection = 270;
				break;
			case Direction.Down:
				gridController.RotateGrid(90);
				rotateDirection = 90;
				break;
			case Direction.Left:
				gridController.RotateGrid(180);
				rotateDirection = 180;
				break;
			case Direction.Right:
				gridController.RotateGrid(0);
				rotateDirection = 0;
				break;
		}
	}
	/// <summary>
	/// 根据鼠标位置设置对应朝向的攻击范围
	/// </summary>
	public Direction savedDirection = Direction.Right;
	public void SetGridGroupPos()
	{
		Direction dir = GetFacePos();
		switch (dir)
		{
			case Direction.Up:
				transform.position = gridController.player.transform.position + new Vector3(0, gridGroup.gridSize, 0);
				if(savedDirection != Direction.Up)
				{
					savedDirection = Direction.Up;
					SetGridGroupRotate();
				}
				break;
			case Direction.Down:
				transform.position = gridController.player.transform.position + new Vector3(0, -gridGroup.gridSize, 0);
				if(savedDirection != Direction.Down)
				{
					savedDirection = Direction.Down;
					SetGridGroupRotate();
				}
				break;
			case Direction.Left:
				transform.position = gridController.player.transform.position + new Vector3(-gridGroup.gridSize, 0, 0);
				if(savedDirection != Direction.Left)
				{
					savedDirection = Direction.Left;
					SetGridGroupRotate();
				}
				break;
			case Direction.Right:
				transform.position = gridController.player.transform.position + new Vector3(gridGroup.gridSize, 0, 0);
				if(savedDirection != Direction.Right)
				{
					savedDirection = Direction.Right;
					SetGridGroupRotate();
				}
				break;
		}
	}

	public Direction inputDirection = Direction.Right;
	Direction GetFacePos()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			inputDirection = Direction.Up;
			return Direction.Up;
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			inputDirection = Direction.Down;
			return Direction.Down;
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			inputDirection = Direction.Left;
			return Direction.Left;
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			inputDirection = Direction.Right;
			return Direction.Right;
		}

		return inputDirection;
	}
	/// <summary>
	/// 获取鼠标在角色的方位
	/// </summary>
	private Direction GetMousePos()
	{
		Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
		// 计算坐标差值
		Vector2 difference = mousePos - gridController.player.transform.position;
    
		// 计算水平/垂直方向比例
		float horizontalRatio = Mathf.Abs(difference.x / difference.y);
    
		// 基于坐标差判断主要方向
		if (Mathf.Abs(difference.y) > Mathf.Abs(difference.x))
		{
			return difference.y > 0 ? Direction.Up : Direction.Down;
		}

		return difference.x > 0 ? Direction.Right : Direction.Left;
	}
}
