using UnityEngine;
[CreateAssetMenu(fileName = "Effect", menuName = "Effects/WalkEffect")]
public class WalkEffect : Effect 
{
    public override void ApplyEffect(CharacterBase from, CharacterBase to)
    {
        if(to == null) return;
        //追踪to目标
        if (targetType == EffectTargetType.One)
        {
            from.GetComponent<EnemyMove>()?.MoveToPlayer();
        }
    }

    public override void ApplyEffect(CharacterBase from, CharacterBase to, Vector3 position)
    {
        
    }
}