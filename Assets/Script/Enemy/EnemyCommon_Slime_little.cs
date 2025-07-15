using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemyCommon_Slime_little : EnemyCommon 
{
	private List<Vector3> areaPoints = new()
	{
		new(-3, -1),new(-3, 0),new(-3, 1),
		new (-2, -2),new (-2, -1),new (-2, 0),new (-2, 1),new (-2, 2),
		new (-1, -3),new (-1, -2),new (-1, -1),new (-1, 0),new (-1, 1),new (-1, 2),new (-1, 3),
		new (0, -3),new (0, -2),new (0, -1),new (0, 1),new (0, 2),new (0, 3),
		new (1, -3),new (1, -2),new (1, -1),new (1, 0),new (1, 1),new (1, 2),new (1, 3),
		new (2, -2),new (2, -1),new (2, 0),new (2, 1),new (2, 2),
		new(3, -1),new(3, 0),new(3, 1)
	};
	public override List<Vector3> SetSpcialPos()
	{
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		if (player != null)
		{
			Vector3 pos = (player.transform.position - transform.position)/gridSize;
			foreach (var point in areaPoints)
			{
				if (Vector3.Distance(pos, point) < 0.1f)
				{
					List<Vector3> hurtPos = new List<Vector3>();
					hurtPos.Add(player.transform.position);
					return hurtPos;
				}
			}
		}

		return new List<Vector3>{transform.position };
	}
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
