using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
	[TextArea]
	public string desc;
	
	public UnityAction<T> OnEventRaised;
	public UnityAction<T,T> OnEventRaised2Para;
	public string lastSender;

	public void RaiseEvent(T value, object sender = null)
	{
		OnEventRaised?.Invoke(value);
		lastSender = sender?.ToString();
	}
	
	public void RaiseEvent2Para(T value1,T value2, object sender = null)
	{
		OnEventRaised2Para?.Invoke(value1,value2);
		lastSender = sender?.ToString();
	}
}
