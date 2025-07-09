using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// 根据当前情况随机3个房间供玩家选择
/// </summary>
public class PickRoomPanel : MonoBehaviour
{
	public RoomPickUI pickedRoom;
	public int currentIndex = 0;
	public ObjectEventSO LoadRoomEvent;
	bool canPickRoom = false;
	private void Start()
	{
		FightController.instance.setRoom += SetRoom;
	}

	public void PrepareFight()
	{
		gameObject.SetActive(false);
	}

	//显示在面板中
	public RoomPickUI roomPrefab;
	public GameObject roomContainer;
	private List<RoomPickUI> rooms = new List<RoomPickUI>();
	private void SetRoom()
	{
		roomLevel++;
		foreach (var room in rooms)
		{
			Destroy(room.gameObject);
		}
		rooms.Clear();
		currentRooms.Clear();
		SetRandomRooms(3);
		currentIndex = 0;
		pickedRoom = rooms[currentIndex];
		pickedRoom.SetSelect();
		canPickRoom = true;
	}

	private void Update()
	{
		if (canPickRoom)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				canPickRoom = false;
				LoadRoomEvent.RaiseEvent(pickedRoom.roomData,this);
			}

			if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				foreach (RoomPickUI room in rooms) { room.SetDeselect(); }
				currentIndex = currentIndex + 1>=rooms.Count ? 0 : currentIndex+1;
				pickedRoom = rooms[currentIndex];
				pickedRoom.SetSelect();
			}
			if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				foreach (RoomPickUI room in rooms) { room.SetDeselect(); }
				currentIndex = currentIndex - 1<0 ? rooms.Count-1 : currentIndex-1;
				pickedRoom = rooms[currentIndex];
				pickedRoom.SetSelect();
			}
		}
	}

	public void SetRoomDeSelected()
	{
		foreach (RoomPickUI room in rooms) { room.SetDeselect(); }
	}

	private List<RoomDataSO> currentRooms = new List<RoomDataSO>();
	public void SetRandomRooms(int roomCount)
	{
		for (int i = 0; i < roomCount; i++)
		{
			var room = GetRandomRoom();
			currentRooms.Add(room);
		}

		int index = 0;
		foreach (var room in currentRooms)
		{
			var roomObj = Instantiate(roomPrefab, roomContainer.transform);
			roomObj.roomData = room;
			roomObj.pickRoomPanel = this;
			roomObj.roomIndex = index;
			roomObj.roomImage.sprite = room.roomIcon;
			roomObj.roomName.text = room.roomType.ToString();
			roomObj.roomType.text = room.symbolType.ToString();
			rooms.Add(roomObj);
			index++;
		}
	}
	//获取随机房间
	//随机规则：房间类型和属性有一种不同，需要达到房间条件
	public int roomLevel = 0;
	public RoomManager roomManager;
	public RoomDataSO GetRandomRoom()
	{
		List<RoomDataSO> canSelectRooms = new List<RoomDataSO>();
		canSelectRooms = roomManager.roomsAll.ToList();
		foreach (var room in canSelectRooms.ToList())
		{
			if (currentRooms.Count > 0)
				foreach (var roomSelect in currentRooms)
				{
					if (room.roomType == roomSelect.roomType && room.symbolType == roomSelect.symbolType)
					{
						canSelectRooms.Remove(room);
					}
				}
			if(room.roomLevel>roomLevel)
			{
				canSelectRooms.Remove(room);
			}
		}

		if (canSelectRooms.Count > 0)
		{
			List<float> roomsRandomRate = new List<float>();
			foreach (var room in canSelectRooms)
			{
				roomsRandomRate.Add(room.randomCount);
			}
			return ToolFunctions.WeightedRandom(canSelectRooms, roomsRandomRate);
		}
		Debug.LogError("No Room Selected");
		return null;
	}
}
