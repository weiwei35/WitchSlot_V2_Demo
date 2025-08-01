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
	private void Awake()
	{
		if(instance == null){
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	//选房间
	public GameObject PickRoomPanel;
	public void OpenPickRoomPanel(object o)
	{
		PickRoomPanel.SetActive(true);
		setRoom?.Invoke();
	}

	//委托
	public delegate void SetRoom();//设置房间
	public SetRoom setRoom;
}
