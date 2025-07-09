using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PlayerSkillList : MonoBehaviour 
{
	public EnemySkillItem skillItem;

	public void InitSkillList(List<EnemySkillSO> skills)
	{
		foreach (var skill in skills)
		{
			var skillItem = Instantiate(this.skillItem, transform);
			skillItem.InitSkillText(skill.skillName,skill.skillDesc);
		}
	}
}
