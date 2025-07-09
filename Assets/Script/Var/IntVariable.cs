using UnityEngine;

[CreateAssetMenu(menuName = "Var/Int")]
public class IntVariable : ScriptableObject 
{
	public int maxValue;
	public int currentValue;

	public IntEventSO ValueChangedEvent;

	public void SetValue(int value)
	{
		currentValue = value;
		ValueChangedEvent?.RaiseEvent(value,this);
	}
}
