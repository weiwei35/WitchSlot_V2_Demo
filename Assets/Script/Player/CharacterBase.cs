using System;
using DamageNumbersPro;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	Animator animator;

	public IntVariable hp;
	public IntVariable defense;
	public int CurrentHp { get=>hp.currentValue; set=>hp.SetValue(value); }

	public bool isDead = false;
	
	public DamageNumber damageNum_hurt;
	public DamageNumber damageNum_heal;
	public DamageNumber damageNum_defence;
	public virtual void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		CurrentHp = hp.maxValue;
		ResetDefense();
	}

	public void TakeDamage(int damage)
	{
		var currentDamage = damage;
		//先扣除护甲
		if (defense != null)
		{
			currentDamage = defense.currentValue - damage>=0?0:damage - defense.currentValue;
			var currentDefense = defense.currentValue - damage>=0?defense.currentValue - damage:0;
			defense.currentValue = currentDefense;
		}

		if (currentDamage > 0)
		{
			damageNum_hurt.Spawn(transform.position,currentDamage);
		}
		if (CurrentHp > currentDamage)
		{
			// Debug.Log(damage);
			CurrentHp -= currentDamage;
		}
		else
		{
			CurrentHp = 0;
			//死亡
			isDead = true;
		}
	}

	public void UpdateDefense(int value)
	{
		var amount = defense.currentValue + value;
		defense.SetValue(amount);
		damageNum_defence.Spawn(transform.position,value);
	}
	public void ResetDefense()
	{
		if (defense != null) defense.SetValue(0);
	}

	public void UpdateHp(int value)
	{
		if(CurrentHp+value>=hp.maxValue) CurrentHp = hp.maxValue;
		else CurrentHp += value;
		
		damageNum_heal.Spawn(transform.position,value);
	}
}
