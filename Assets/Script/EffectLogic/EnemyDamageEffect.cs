using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "EnemyEffects/Damage")]
public class EnemyDamageEffect : Effect
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
        }
    }

    public ObjectEventSO EnemyAttackFail;
    public ObjectEventSO EnemyAttackSuccess;
    public override void ApplyEffect(CharacterBase from, CharacterBase to, Vector3 position)
    {
        if (targetType == EffectTargetType.One)
        {
            var effect = Instantiate(effectPrefab, position, Quaternion.identity);
            Destroy(effect, 5);
            Vector3 distance = to.transform.position - position;
            if (distance.magnitude < 0.1f)
            {
                to.TakeDamage(value);
                LogController.instance.logDelegate?.Invoke($"对{to.name}造成{value}点伤害");
                EnemyAttackSuccess.RaiseEvent(null, to);
            }
            else
            {
                EnemyAttackFail.RaiseEvent(null,to);
            }
        }
    }
}