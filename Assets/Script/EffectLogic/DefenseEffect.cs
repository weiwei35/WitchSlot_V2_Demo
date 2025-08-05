
using UnityEngine;
[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Effects/DefenseEffect")]
public class DefenseEffect : Effect 
{
	public override void ApplyEffect(CharacterBase from, CharacterBase to)
	{
		if (targetType == EffectTargetType.Self)
		{
			from.UpdateDefense(value);
			var effect = Instantiate(effectPrefab, from.transform.position, Quaternion.identity);
			Destroy(effect, 5);
		}

		if (targetType == EffectTargetType.One)
		{
			to.UpdateDefense(value);
		}
	}

	public override void ApplyEffect(CharacterBase from, CharacterBase to, Vector3 position)
	{
	}
}
