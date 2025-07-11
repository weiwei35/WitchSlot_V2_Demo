
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Effect", menuName = "Effects/CallEffect")]
public class CallEnemyEffect : Effect
{
	public EnemyCommon callEnemy;
	public override void ApplyEffect(CharacterBase from, CharacterBase to)
	{
	}

	public ObjectEventSO EnemyAttackFail;
	public ObjectEventSO EnemyAttackSuccess;
	public override void ApplyEffect(CharacterBase from, CharacterBase to, Vector3 position)
	{
		if (targetType == EffectTargetType.Self)
		{
			//周围召唤怪物
			var enemy= Instantiate(callEnemy,position,Quaternion.identity,from.transform.parent);
			ToolFunctions.SetEnemyHP(enemy);
			
			from.transform.parent.GetComponent<EnemyGroup>().enemies.Add(enemy);
			EnemyAttackSuccess.RaiseEvent(null, to);
		}
	}
}
