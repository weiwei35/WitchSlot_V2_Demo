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
/// 追踪player，躲避障碍
/// 到达player附近停止
/// </summary>
public class EnemyMove : MonoBehaviour
{
	public int stopDistance;
	public float gridSize = 0.8f;
	
	protected GameObject targetPlayer;

	public bool canMove = false;

	private void Start()
	{
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
		canMove = false;
	}

	private void Update()
	{
		if (targetPlayer != null)
		{
			canMove = !ReadyToAttack();
		}
	}

	public virtual bool ReadyToAttack()
	{
		Vector3 playerPos = targetPlayer.transform.position;
		float distance = Vector3.Distance(playerPos,transform.position);
		if (Mathf.Abs(distance - stopDistance * gridSize)>0.3f)
			return false;
		return true;
	}
	
	public List<Vector3> movePath = new List<Vector3>();
	public virtual void FindPath()
	{
		movePath.Clear();
		movePath = Testing_PathFinding.instance.FindPath(transform.position, targetPlayer.transform.position);
	}
	protected internal virtual void MoveToPlayer()
	{
		FindPath();
		if (movePath.Count > 1 && canMove)
		{
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			rb.MovePosition(movePath[1]);
		}
	}
}
