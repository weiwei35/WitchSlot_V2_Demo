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

public class TouchRoomSide : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public ObjectEventSO EnterMapAreaEvent;
    public ObjectEventSO ExitMapAreaEvent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.LogError("地图区域");
        EnterMapAreaEvent.RaiseEvent(null,this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.LogError("离开地图区域");
        ExitMapAreaEvent.RaiseEvent(null,this);
    }
}
