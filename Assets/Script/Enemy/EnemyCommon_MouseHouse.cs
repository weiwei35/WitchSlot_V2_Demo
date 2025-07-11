using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EnemyCommon_MouseHouse : EnemyCommon 
{
	private List<Vector3> callPoints = new()
	{
		new(-1, -1),
		new (-1, 0),
		new (-1, 1),
		new (0, -1),
		new (0, 1),
		new (1, -1),
		new (1, 0),
		new (1, 1),
	};
	public override List<Vector3> SetSpcialPos()
	{
		int randomGrid = Random.Range(0, 8);
		Vector3 enemyPos = transform.position + callPoints[randomGrid]*GetComponent<EnemyMove>().gridSize;
		return new List<Vector3> { enemyPos };
	}
}
