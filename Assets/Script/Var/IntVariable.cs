using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Var/Int")]
public class IntVariable : ScriptableObject 
{
	public float maxValue;
	public float currentValue;

	[CanBeNull]public IntEventSO ValueChangedEvent;

	public void SetValue(float value)
	{
		currentValue = value;
		ValueChangedEvent?.RaiseEvent(value,this);
	}
}
