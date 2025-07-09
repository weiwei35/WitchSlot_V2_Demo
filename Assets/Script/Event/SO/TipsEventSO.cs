using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/TipsObject")]

public class TipsEventSO : BaseEventSO<ItemInfoDataSO> 
{
	public UnityAction<ItemInfoDataSO,GameObject> OnEventRaisedWithGameObject;
	public void RaiseEventWithGameObject(ItemInfoDataSO value,GameObject obj, object sender = null)
	{
		OnEventRaisedWithGameObject?.Invoke(value,obj);
		lastSender = sender?.ToString();
	}
}
