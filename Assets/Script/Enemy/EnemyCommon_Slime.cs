using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemyCommon_Slime : EnemyCommon 
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
			Vector3 pos = (player.transform.position - transform.position)/GetComponent<EnemyMove>().gridSize;
			foreach (var point in areaPoints)
			{
				if (Vector3.Distance(pos, point) < 0.1f)
				{
					List<Vector3> hurtPos = new List<Vector3>();
					hurtPos.Add(player.transform.position);
					hurtPos.Add(player.transform.position+new Vector3(-1,0)*GetComponent<EnemyMove>().gridSize);
					hurtPos.Add(player.transform.position+new Vector3(1,0)*GetComponent<EnemyMove>().gridSize);
					return hurtPos;
				}
			}
		}

		return new List<Vector3>{transform.position };
	}

	public EnemyCommon callEnemy;
	public override void EnemyDie()
	{
		base.EnemyDie();
		
		var enemy= Instantiate(callEnemy,transform.position,Quaternion.identity,transform.parent);
		ToolFunctions.SetEnemyHP(enemy);
			
		transform.parent.GetComponent<EnemyGroup>().enemies.Add(enemy);
	}
}
