using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemyCommon_Mouse : EnemyCommon 
{
	private List<Vector3> areaPoints = new()
	{
		new (-1, 0),
		new (0, -1),
		new (0, 1),
		new (1, 0),
	};
	public override bool ReadyToAttack(Player targetPlayer)
	{
		Vector3 pos = (targetPlayer.transform.position - transform.position)/gridSize;
		foreach (var point in areaPoints)
		{
			if (Vector3.Distance(pos, point) < 0.1f)
			{
				return true;
			}
		}
		return false;
	}
}
