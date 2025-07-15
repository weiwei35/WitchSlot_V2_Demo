using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;
using CodeMonkey.Utils;
using DG.Tweening;

public class PlayerMove_new : MonoBehaviour 
{
	[Header("Movement Settings")]
	public float gridSize = 0.8f;
	public float moveDuration = 0.2f;
	public float inputCoolDown = 0.1f;
	private Vector3 lastValidPosition;
	private Vector3 targetPosition;
	private float lastMoveTime;
    
	private Queue<Vector3> moveQueue = new Queue<Vector3>();
	private bool useMoveQueue;

	public int moveStep = 2;
	private Vector3 targetPos;
	private Rigidbody2D rb;
	public bool canMove = false;
	bool isMoving = false;
	public bool inRound = false;

	public ObjectEventSO PlayerMoveEvent;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		lastValidPosition = transform.position;
		targetPosition = transform.position;
		
		FightController.instance.startRound += InRoundMove;
		FightController.instance.endRound += EndRound;
		FightController.instance.setRoom += StopMove;
	}

	private void OnDisable()
	{
		FightController.instance.startRound -= InRoundMove;
		FightController.instance.endRound -= EndRound;
		FightController.instance.setRoom -= StopMove;
	}

	public void StartMove()
	{
		canMove = true;
	}
	public void StopMove()
	{
		canMove = false;
	}
	public void PlayerMove()
	{
		canMove = true;
		moveStep = 2;
	}

	private void EndRound()
	{
		if (inRound) inRound = false;
		canMove = true;
		moveStep = int.MaxValue;
	}

	private void InRoundMove()
	{
		inRound = true;
		canMove = false;
	}

	private void Update()
	{
		// 冷却时间控制
		if (Time.time - lastMoveTime < inputCoolDown) return;
        
		// 队列控制系统
		if (useMoveQueue && moveQueue.Count > 0)
		{
			Vector3 nextMove = moveQueue.Dequeue();
			StartCoroutine(MoveToGrid(nextMove));
		}
		if (canMove && (moveStep > 0||!inRound) && !isMoving)
		{
			// 键盘输入检测
			if (Input.GetKey(KeyCode.A))
			{
				RegisterMove(-Vector3.right);
			}
			else if (Input.GetKey(KeyCode.D))
			{
				RegisterMove(Vector3.right);
			}
			else if (Input.GetKey(KeyCode.W))
			{
				RegisterMove(Vector3.up);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				RegisterMove(-Vector3.up);
			}
		}
	}

	// 统一注册移动请求
	private void RegisterMove(Vector3 direction)
	{
		moveStep--;
		if (isMoving) 
		{
			if(useMoveQueue) moveQueue.Enqueue(direction);
			return;
		}
        
		StartCoroutine(MoveToGrid(direction));
	}
	// 网格精确移动协程
	private IEnumerator MoveToGrid(Vector3 direction)
	{
		isMoving = true;
		lastMoveTime = Time.time;
        
		// === 核心改进：网格中心化计算 ===
		// 1. 计算当前所在的网格中心点位置
		Vector3 currentCenter = GetCurrentGridCenter();
        
		// 2. 计算目标网格的中心位置
		Vector3 targetCenter = currentCenter + direction * gridSize;
        
		// 3. 更新位置记录
		targetPosition = targetCenter;
		lastValidPosition = targetPosition;
        
		// === 使用刚体进行物理移动 ===
		yield return rb.DOMove(targetPosition, moveDuration)
			.SetEase(Ease.OutQuad)
			.SetUpdate(UpdateType.Fixed)
			.WaitForCompletion();
        
		// === 最终位置校正 ===
		// 强制对齐到网格中心（避免浮点误差）
		SnapToGridCenter();
        
		isMoving = false;
		// if(!isStay) PlayerMoveEvent.RaiseEvent(null, this);
	}
    
	// 获取当前位置所属网格的中心
	private Vector3 GetCurrentGridCenter()
	{
		// 计算当前所在的网格索引
		int gridX = Mathf.RoundToInt(transform.position.x / gridSize - 0.5f);
		int gridY = Mathf.RoundToInt(transform.position.y / gridSize - 0.5f);
        
		// 计算网格中心坐标
		float centerX = (gridX + 0.5f) * gridSize;
		float centerY = (gridY + 0.5f) * gridSize;
        
		return new Vector3(centerX, centerY, 0);
	}
    
	// 将角色对齐到最近的网格中心
	private void SnapToGridCenter()
	{
		Vector3 gridCenter = GetCurrentGridCenter();
		rb.MovePosition(gridCenter);
		PlayerMoveEvent.RaiseEvent(null, this);
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		// 只在移动时检测碰撞
		if (!isMoving) return;
    
		if (other.CompareTag("Enemy") || other.CompareTag("MapWall"))
		{
			rb.DOComplete();
			rb.MovePosition(lastValidPosition); // 回退到上次有效位置

			moveStep++;
			// 清空已排队的移动
			moveQueue.Clear();
			isMoving = false;
        
			// 可选：添加碰撞反馈效果
			ShakeCamera(0.1f, 0.15f);
		}
	}

// 可选：碰撞反馈效果
	private void ShakeCamera(float duration, float strength)
	{
		Camera.main.DOShakePosition(duration, strength);
	}

	// [Header("检测设置")]
	// [SerializeField] private float detectionRadius = 5f; // 检测半径
	// [SerializeField] private LayerMask enemyLayer;      // 敌人层级
	// public void CheckEnemyAround()
	// {
	// 	// 使用OverlapSphere高效检测圆形区域内的敌人
	// 	Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
	// 		transform.position, 
	// 		detectionRadius, 
	// 		enemyLayer.value);
 //        
	// 	// 处理检测结果
	// 	bool enemyAround = false;
	// 	if (hitColliders.Length > 0)
	// 	{
	// 		foreach (var col in hitColliders)
	// 		{
	// 			// 避免检测到自身
	// 			if (col.gameObject != this.gameObject)
	// 			{
	// 				// 计算实际距离
	// 				float distance = Vector3.Distance(transform.position, col.transform.position);
	//
	// 				if (distance <= detectionRadius)
	// 				{
	// 					enemyAround = true;
	// 					// Debug.Log($"发现敌人: {col.name} | 距离: {distance:F2}米");
	//
	// 					EnemyCommon enemyTarget = col.GetComponent<EnemyCommon>();
	// 					if (enemyTarget != null && !enemyTarget.inFight)
	// 					{
	// 						enemyTarget.JoinFight();
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	//
	// 	if (enemyAround && !inRound)
	// 	{
	// 		//进战
	// 		FightController.instance.StartFight();
	// 	}else if (enemyAround && inRound)
	// 	{
	// 		FightController.instance.AddFight();
	// 	}
	// }
}
