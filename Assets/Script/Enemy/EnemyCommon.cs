using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// 控制怪物属性：生命值、攻击力
/// </summary>
public class EnemyCommon : CharacterBase
{
    [Tooltip("怪物属性")] 
    public string enemyName;
    public Sprite enemyIcon;
    public float enemyHP;
    public int moveStep;
    public EnemyAttackSO attackData;
    [HideInInspector]
    public FightWeight fightweight;
    public float attack;
    public List<EnemySkillSO> skill;
    public bool isStandEnemy = false;
	
    [Tooltip("怪物展示")] 
    public TMP_Text hpText;
    public GameObject hpObj;

    public ObjectEventSO EnemyDieEvent;
    
    public bool inFight = false;
    public bool newAdd = true;

    public override void Awake()
    {
        base.Awake();
        fightweight = GetComponent<FightWeight>();
    }

    public virtual void EnemyDie()
    {
        transform.parent.GetComponent<EnemyGroup>().EnemyDie(this);
        EnemyDieEvent.RaiseEvent(this,this);
    }

    private void Update()
    {
        if (isDead)
        {
            EnemyDie();
            Destroy(gameObject);
        }
        // hpText.text = CurrentHp.ToString();
        var scale = hpObj.transform.localScale;
        scale.x = (float)CurrentHp / hp.maxValue;
        hpObj.transform.localScale = scale;
    }

    public virtual List<Vector3> SetSpcialPos()
    {
        EnemyMove enemyMove = GetComponent<EnemyMove>();
        enemyMove.FindPath();
        if(enemyMove.movePath.Count<=0) return new List<Vector3>{transform.position};
        return new List<Vector3>{enemyMove.movePath[1] };
    }
}