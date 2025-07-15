using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
	
	protected Player targetPlayer;

	public bool canMove = false;

	private Rigidbody2D rb;
	private Vector3 lastValidPosition;
	EnemyCommon enemyCommon;
	private void Start()
	{
		enemyCommon = GetComponent<EnemyCommon>();
		rb = GetComponent<Rigidbody2D>();
		lastValidPosition = transform.position;
		targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		canMove = false;
	}

	private void Update()
	{
		if (targetPlayer != null)
		{
			canMove = !enemyCommon.ReadyToAttack(targetPlayer);
		}
	}
	
	public List<Vector3> movePath = new List<Vector3>();
	public virtual void FindPath()
	{
		movePath.Clear();
		movePath = Testing_PathFinding.instance.FindPath(transform.position, targetPlayer.transform.position);
	}
	bool isMoving = false;
	protected internal virtual void MoveToPlayer()
	{
		FindPath();
		if (movePath.Count > 1 && canMove)
		{
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			lastValidPosition = transform.position;
			isMoving = true;
			rb.DOMove(movePath[1], 0.1f)
				.SetEase(Ease.OutQuad)
				.SetUpdate(UpdateType.Fixed)
				.OnComplete(()=>isMoving=false);
			// rb.MovePosition(movePath[1]);
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(!isMoving) return;
		if (other.CompareTag("Enemy") || other.CompareTag("MapWall")|| other.CompareTag("Player"))
		{
			rb.DOComplete();
			rb.MovePosition(lastValidPosition); // 回退到上次有效位置
		}
	}
}
