
using UnityEngine;
[CreateAssetMenu(fileName = "DamageEffect", menuName = "Effects/DamageEffect")]
public class DamageEffect : Effect 
{
	public override void ApplyEffect(CharacterBase from, CharacterBase to)
	{
		if(to == null) return;
		switch (targetType)
		{
			case EffectTargetType.One:
				to.TakeDamage(value);
				var effect = Instantiate(effectPrefab, to.transform.position, Quaternion.identity);
				Destroy(effect, 5);
				LogController.instance.logDelegate?.Invoke($"对{to.name}造成{value}点伤害");
				break;
			case EffectTargetType.All:
				var enemyGroup = GameObject.FindGameObjectWithTag("EnemyGroup").GetComponent<EnemyGroup>();
				foreach (var enemy in enemyGroup.enemiesInFight)
				{
					enemy.GetComponent<CharacterBase>().TakeDamage(value);
				}
				break;
		}
	}

	public override void ApplyEffect(CharacterBase from, CharacterBase to, Vector3 position)
	{
	}
}
