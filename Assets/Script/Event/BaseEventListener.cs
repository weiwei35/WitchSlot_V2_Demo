using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour 
{
	public BaseEventSO<T> eventSO;
	public UnityEvent<T> response;
	public UnityEvent<T,T> response2Para;

	private void OnEnable()
	{
		if (eventSO != null)
		{
			eventSO.OnEventRaised += OnEventRaised;
			eventSO.OnEventRaised2Para += OnEventRaised2Para;
		}
		
	}

	private void OnDisable()
	{
		if (eventSO != null)
		{
			eventSO.OnEventRaised -= OnEventRaised;
			eventSO.OnEventRaised2Para -= OnEventRaised2Para;
		}
	}

	public void OnEventRaised(T value)
	{
		response.Invoke(value);
	}
	public void OnEventRaised2Para(T value1,T value2)
	{
		response2Para.Invoke(value1,value2);
	}
}
