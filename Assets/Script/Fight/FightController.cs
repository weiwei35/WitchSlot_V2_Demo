using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class FightController : MonoBehaviour 
{
	//单例
	public static FightController instance;
	public bool inFight;
	private void Awake()
	{
		if(instance == null){
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	private void Start()
	{
	}
	//选房间
	public GameObject PickRoomPanel;
	public void OpenPickRoomPanel(object o)
	{
		PickRoomPanel.SetActive(true);
		setRoom?.Invoke();
	}

	public void FirstSetRoom()
	{
		setRoom?.Invoke();
	}
	//结束战斗
	public ObjectEventSO ShowNextEvent;
	private void EndFight()
	{
		inFight = false;
		//加载下层楼梯
		// if(enemyGroup.enemies.Count == 0)
		// 	ShowNextEvent.RaiseEvent(null,this);
	}

	//委托
	public delegate void SetRoom();//设置房间
	public SetRoom setRoom;
}
