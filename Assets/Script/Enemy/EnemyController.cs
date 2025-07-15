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
/// 控制怪物移动、攻击、结束回合
/// </summary>
public class EnemyController : MonoBehaviour
{
	[HideInInspector]
	public EnemyMove enemyMove;
	[HideInInspector]
	public EnemyCommon enemyCommon;
	private Player player;
	
	public GameObject targetIcon;
	private int step;
	private EnemyAction currentAction;
	private EnemyMoveAction currentmoveAction;
	
	public SpriteRenderer actionIcon;
	public TMP_Text actionText;
	
	bool skillReady = false;
	int timeCounter = 0;
	
	private void Start()
	{
		enemyMove = GetComponent<EnemyMove>();
		enemyCommon = GetComponent<EnemyCommon>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		SetMoveAction();
	}
	private void SetMovement()
	{
		if (enemyMove.canMove && !enemyCommon.isStandEnemy)
		{
			SetMoveAction();
		}
		else
		{
			SetSkillAction();
		}
	}
	public void AttackFail()
	{
		// if (isCurrentEnemy && !endAttack)
		// {
		// 	endAttack = true;
		// 	SetMovement();
		// 	if (enemyMove.canMove)
		// 	{
		// 		StartFight();
		// 	}
		// 	else
		// 	{
		// 		targetIcon.SetActive(false);
		// 		transform.parent.GetComponent<EnemyGroup>().NextEnemyMove();
		// 	}
		// }
	}
	public void AttackSuccess()
	{
		// if (isCurrentEnemy && !endAttack)
		// {
		// 	endAttack = true;
		// 	targetIcon.SetActive(false);
		// 	transform.parent.GetComponent<EnemyGroup>().NextEnemyMove();
		// 	SetMovement();
		// 	isCurrentEnemy = false;
		// }
	}

	public void MoveFollowPlayer()
	{
		StartCoroutine(EnemyMoveStep());
	}
	IEnumerator EnemyMoveStep()
	{
		yield return new WaitForSeconds(0.1f);
		EnemyMove();
	}
	public void EnemyMove()
	{
		if(!enemyCommon.inFight) return;
		// if(step <= 0)
		// {
			// targetIcon.SetActive(false);
			// transform.parent.GetComponent<EnemyGroup>().NextEnemyMove();
			// SetMovement();
			// isCurrentEnemy = false;
			// return;
		// }
		// step--;
		
		if(timeCounter > 0) timeCounter--;
		if (skillReady)
		{
			skillReady = false;
			foreach (var pos in hurtArea)
			{
				currentAction.effect.ApplyEffect(enemyCommon,player,pos);
			}
			return;
		}
		//是否在player附近？攻击：移动
		if (enemyMove.canMove || !enemyCommon.isStandEnemy)
		{
			currentmoveAction.effect.ApplyEffect(enemyCommon,player);
			actionText.text = step.ToString();
			SetMovement();
		}
		else
		{
			SetSkillAction();
		}
		// StartCoroutine(EnemyMoveStep());
	}

	public GameObject moveStep;
	private GameObject stepObj;
	private List<Vector3> hurtArea = new List<Vector3>();
	public Sprite moveRight;
	public Sprite moveLeft;
	public Sprite moveUp;
	public Sprite moveDown;
	public Sprite attackIcon;
	public Sprite attackBg;
	public Sprite moveBg;
	private void SetMoveAction()
	{
		currentmoveAction = enemyCommon.attackData.moveAction;
		actionIcon.sprite = currentmoveAction.icon;
		actionText.text = enemyCommon.moveStep.ToString();
		
		//显示移动路线
		SetStepDirction(0);
	}
	private void SetSkillAction()
	{
		for (int i = 0; i < enemyCommon.attackData.effects.Count; i++)
		{
			EnemyAction action =enemyCommon.attackData.effects[i];
			if (action.coldTime > 0 && timeCounter == 0)
			{
				skillReady = true;
				currentAction = action;
				actionIcon.sprite = currentAction.icon;
				actionText.text = currentAction.effect.value.ToString();
				//显示移动路线
				SetStepDirction(1);
				timeCounter = action.coldTime+1;
				break;
			}
			if (action.coldTime == 0)
			{
				skillReady = true;
				currentAction = action;
				actionIcon.sprite = currentAction.icon;
				actionText.text = currentAction.effect.value.ToString();
				//显示移动路线
				SetStepDirction(1);
				break;
			}
		}
	}

	private void SetStepDirction(int index)//0移动；1攻击
	{
		Destroy(stepObj);
		hurtArea = enemyCommon.SetSpcialPos();
		Vector3 stepPos = hurtArea[0];
		if(stepPos.Equals(transform.position)) return;
		var difference = stepPos - transform.position;
		Direction dir;
		if (Mathf.Abs(difference.y) > Mathf.Abs(difference.x))
		{
			dir = difference.y > 0 ? Direction.Up : Direction.Down;
		}
		else
		{
			dir = difference.x > 0 ? Direction.Right : Direction.Left;
		}
		stepObj = Instantiate(moveStep, transform);
		stepObj.transform.position = stepPos;
		SpriteRenderer stepBg = stepObj.GetComponent<SpriteRenderer>();
		SpriteRenderer stepIcon = stepObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
		if (hurtArea.Count > 1)
		{
			List<GameObject> stepObjs = new List<GameObject>();
			for (int i = 1; i < hurtArea.Count; i++)
			{
				var stepChild = Instantiate(stepObj);
				stepChild.transform.position = hurtArea[i];
				stepObjs.Add(stepChild);
			}

			foreach (var obj in stepObjs)
			{
				obj.transform.SetParent(stepObj.transform);
				obj.transform.localScale = Vector3.one;
			}
		}
		switch (index)
		{
			case 0:
				stepBg.sprite = moveBg;
				//根据方向显示移动icon
				switch (dir)
				{
					case Direction.Up:
						stepIcon.sprite = moveUp;
						break;
					case Direction.Down:
						stepIcon.sprite = moveDown;
						break;
					case Direction.Left:
						stepIcon.sprite = moveLeft;
						break;
					case Direction.Right:
						stepIcon.sprite = moveRight;
						break;
				}
				break;
			case 1:
				//显示攻击icon
				stepBg.sprite = attackBg;
				stepIcon.sprite = attackIcon;
				break;
		}
	}

	public void RefreshMoveStep()
	{
		if(!enemyCommon.inFight) return;
		if (enemyMove.canMove && !skillReady)
		{
			SetStepDirction(0);
		}
		else if (!enemyMove.canMove && stepObj==null)
		{
			SetStepDirction(1);
		}
	}
	
	
	[Header("检测设置")]
	[SerializeField] private float detectionRadius = 3f; // 检测半径
	[SerializeField] private float gridSize = 0.8f;
	[SerializeField] private LayerMask playerLayer;      // 检测层级
	public void CheckPlayerAround()
	{
		// 计算精确矩形尺寸
		float fullWidth = (detectionRadius * 2) * gridSize;
		Vector2 boxSize = new Vector2(fullWidth, fullWidth);

		// 高效矩形区域检测
		Collider2D[] hitColliders = Physics2D.OverlapBoxAll(
			transform.position, 
			boxSize, 
			0f,  // 角度
			playerLayer
		);
    
		// 处理检测结果
		if (hitColliders.Length > 0)
		{
			foreach (var col in hitColliders)
			{
				// 避免检测到自身
				if (col.gameObject == this.gameObject) continue;
            
				// 确保确实是玩家
				if (!col.CompareTag("Player")) continue;
            
				// 获取真实矩形边界 (解决AABB->OBB变换)
				Vector2 playerPos = col.transform.position;
				Vector2 dir = playerPos - (Vector2)transform.position;
				float halfSize = detectionRadius * gridSize + (gridSize * 0.5f);
            
				// 正确的矩形边界检测
				if (Mathf.Abs(dir.x) <= halfSize && Mathf.Abs(dir.y) <= halfSize)
				{
					// Debug.Log("Player detected at position" + playerPos);
					TriggerCombat();
					return; // 找到一个玩家后立即返回
				}
			}
		}
	}

// 战斗触发逻辑单独封装
	private void TriggerCombat()
	{
		if (!enemyCommon.inFight)
		{
			enemyCommon.inFight = true;
			SetMovement();
			transform.parent.GetComponent<EnemyGroup>()
				.enemiesInFight.Add(enemyCommon);
        
			if (!FightController.instance.inFight)
			{
				// FightController.instance.StartFight();
			}
			else
			{
				// FightController.instance.AddFight();
			}
		}
	}

}
