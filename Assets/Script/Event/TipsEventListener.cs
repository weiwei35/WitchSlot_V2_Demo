using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class TipsEventListener : MonoBehaviour
{
    public TipsEventSO eventSOWithGameObj;
    public UnityEvent<ItemInfoDataSO,GameObject> responseWithGameObj;

    private void OnEnable()
    {
        if (eventSOWithGameObj != null)
            eventSOWithGameObj.OnEventRaisedWithGameObject += OnEventRaisedWithGameObj;
    }

    private void OnDisable()
    {
        if (eventSOWithGameObj != null)
            eventSOWithGameObj.OnEventRaisedWithGameObject += OnEventRaisedWithGameObj;
    }
    
    public void OnEventRaisedWithGameObj(ItemInfoDataSO value,GameObject obj)
    {
        responseWithGameObj.Invoke(value, obj);
    }
}
