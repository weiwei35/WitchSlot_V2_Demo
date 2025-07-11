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
	
	protected GameObject targetPlayer;

	public bool canMove = false;

	private Rigidbody2D rb;
	private Vector3 lastValidPosition;
	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		lastValidPosition = transform.position;
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
		Vector2 playerPos = targetPlayer.transform.position;
		Vector2 dir = playerPos - (Vector2)transform.position;
		float halfSize = stopDistance * gridSize + (gridSize * 0.5f);
		// 正确的矩形边界检测
		if (Mathf.Abs(dir.x) <= halfSize && Mathf.Abs(dir.y) <= halfSize)
		{
			return true; // 找到一个玩家后立即返回
		}
		return false;
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
