using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	Animator animator;

	public IntVariable hp;
	public IntVariable defense;
	public int CurrentHp { get=>hp.currentValue; set=>hp.SetValue(value); }

	public bool isDead = false;
	public virtual void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		CurrentHp = hp.maxValue;
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
	}

	public void ResetDefense()
	{
		defense.SetValue(0);
	}
}
