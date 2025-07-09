using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : CharacterBase 
{
	public TMP_Text hpText;
	public Sprite playerIcon;
	public string playerName;
	public List<EnemySkillSO> skill;
	private void Update()
	{
		hpText.text = CurrentHp.ToString();
	}
}
