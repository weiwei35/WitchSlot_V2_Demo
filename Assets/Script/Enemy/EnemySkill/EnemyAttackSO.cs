using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy/Attack")]
//实现怪物的行为逻辑基类
public class EnemyAttackSO : ScriptableObject 
{
    public EnemyMoveAction moveAction;
    public List<EnemyAction> effects;
}
[Serializable]
public struct EnemyMoveAction
{
    public Sprite icon;
    public Effect effect;
}
[Serializable]
public struct EnemyAction
{
    public Sprite icon;
    public Effect effect;
    public int coldTime;
}