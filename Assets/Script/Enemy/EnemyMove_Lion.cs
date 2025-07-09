using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

	/// <summary>
	/// 当怪物与角色距离>3时，先移动到距离为3的四点中最近一点
	/// </summary>
public class EnemyMove_Lion : EnemyMove 
{
	public override bool ReadyToAttack()
	{
		Vector3 playerPos = targetPlayer.transform.position;
		float distance = Vector3.Distance(playerPos,transform.position);
		if (distance > stopDistance * gridSize || 
		    (Mathf.Abs(playerPos.y- transform.position.y)>0.1f&&
		     Mathf.Abs(playerPos.x- transform.position.x)>0.1f))
			return false;
		return true;
	}
	
	public override void FindPath()
	{
		Vector3 playerPos = targetPlayer.transform.position;
		float distancePlayer = Vector3.Distance(playerPos,transform.position);
		int steps = Mathf.RoundToInt(distancePlayer / gridSize);
			movePath.Clear();
            // movePath = Testing_PathFinding.instance.FindPath(transform.position, targetPlayer.transform.position);
            Vector3[] playerNearPos = new Vector3[4];
            playerNearPos[0] = targetPlayer.transform.position+new Vector3(steps*gridSize,0f,0f);
            playerNearPos[1] = targetPlayer.transform.position+new Vector3(-steps*gridSize,0f,0f);
            playerNearPos[2] = targetPlayer.transform.position+new Vector3(0f,steps*gridSize,0f);
            playerNearPos[3] = targetPlayer.transform.position+new Vector3(0f,-steps*gridSize,0f);
            int closePosIndex = 0;
            float distance = float.MaxValue;
            for (int i = 0; i < playerNearPos.Length; i++)
            {
            	if (Vector3.Distance(playerNearPos[i], transform.position) < distance)
            	{
            		closePosIndex = i;
            		distance = Vector3.Distance(playerNearPos[i], transform.position);
            	}
            }
            movePath = Testing_PathFinding.instance.FindPath(transform.position, playerNearPos[closePosIndex]);
	}

	protected internal override void MoveToPlayer()
	{
		FindPath();
		if (movePath.Count > 1 && canMove)
		{
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			rb.MovePosition(movePath[1]);
		}
	}
}
