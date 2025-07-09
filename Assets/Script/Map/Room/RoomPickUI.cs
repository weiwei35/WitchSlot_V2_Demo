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

public class RoomPickUI : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
	public RoomDataSO roomData;
	public PickRoomPanel pickRoomPanel;
	public int roomIndex;
	public Image roomImage;
	public TMP_Text roomName;
	public TMP_Text roomType;
	
	public ObjectEventSO LoadRoomEvent;
	public void OnPointerClick(PointerEventData eventData)
	{
		//加载对应房间
		LoadRoomEvent.RaiseEvent(roomData,this);
	}

	public void SetSelect()
	{
		roomImage.color = Color.red;
		pickRoomPanel.currentIndex = roomIndex;
	}

	public void SetDeselect()
	{
		roomImage.color = Color.white;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		pickRoomPanel.SetRoomDeSelected();
		SetSelect();
	}
	
}
