using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	[HideInInspector]public int level = 0;
	GameView view;//游戏展示管理器

	private void Start()
	{
		view = GetComponent<GameView>();
	}

	public void LevelWin()
	{
		level++;
		view.LevelWin();//胜利弹窗
	}
	public void LevelLose()
	{
		view.LevelLose(level,level*10);//结算弹窗
	}
}
