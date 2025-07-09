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
    [HideInInspector]
    public FightWeight fightweight;
    public float attack;
    public List<EnemySkillSO> skill;
	
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

    void EnemyDie()
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

    public void JoinFight()
    {
        inFight = true;
        EnemyGroup group = GameObject.FindGameObjectWithTag("EnemyGroup").GetComponent<EnemyGroup>();
        group.enemiesInFight.Add(this);
    }
}