using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemySkillItem : MonoBehaviour 
{
	public TMP_Text skillName;
	public TMP_Text skillDesc;

	public void InitSkillText(string skillName, string skillDesc)
	{
		this.skillName.text = skillName;
		this.skillDesc.text = skillDesc;
	}
}
