using UnityEngine;

public abstract class Effect : ScriptableObject
{
	public int value;
	public EffectTargetType targetType;
	public EffectType effectType;
	
	public GameObject effectPrefab;
	
	public abstract void ApplyEffect(CharacterBase from, CharacterBase to);
	public abstract void ApplyEffect(CharacterBase from, CharacterBase to,Vector3 position);
}
