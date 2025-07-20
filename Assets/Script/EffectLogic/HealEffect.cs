using UnityEngine;
[CreateAssetMenu(fileName = "Effect", menuName = "Effects/HealEffect")]

public class HealEffect : Effect 
{
	public override void ApplyEffect(CharacterBase from, CharacterBase to)
	{
		if(to == null) return;
		switch (targetType)
		{
			case EffectTargetType.Self:
				to.UpdateHp(value);
				break;
		}
	}

	public override void ApplyEffect(CharacterBase from, CharacterBase to, Vector3 position)
	{
		throw new System.NotImplementedException();
	}
}
