using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemyCommon_FakeMonster : EnemyCommon 
{
	public override void EnemyDie()
	{
		//重置练习假人
		CurrentHp = hp.maxValue;
	}
}
