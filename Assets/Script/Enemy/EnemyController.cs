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

	public int stepStatic = 3;
	private int step;

	public EnemyAttackSO attackData;
	private EnemyAction currentAction;
	
	public SpriteRenderer actionIcon;
	public TMP_Text actionText;
	
	bool skillReady = false;
	private void Start()
	{
		enemyMove = GetComponent<EnemyMove>();
		enemyCommon = GetComponent<EnemyCommon>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		SetMoveAction();
	}
	private void SetMovement()
	{
		if (enemyMove.canMove)
		{
			SetMoveAction();
		}
		else
		{
			SetSkillAction();
		}
	}
	public void StartFight()
	{
		Destroy(stepObj);
		step = stepStatic;
		targetIcon.SetActive(true);
		//是否符合攻击条件？攻击：移动
		if (!skillReady)
		{
			StartCoroutine(EnemyMoveStep());
		}
		else
		{
			skillReady = false;
			currentAction.effect.ApplyEffect(enemyCommon,player,stepObj.transform.position);
		}
	}
	public void AttackFail()
	{
		if (step > 0)
		{
			SetMoveAction();
			StartFight();
		}
	}
	public void AttackSuccess()
	{
		if (step > 0)
		{
			targetIcon.SetActive(false);
			transform.parent.GetComponent<EnemyGroup>().NextEnemyMove();
			SetMovement();
		}
	}

	IEnumerator EnemyMoveStep()
	{
		yield return new WaitForSeconds(0.5f);
		EnemyMove();
	}
	public void EnemyMove()
	{
		if(step <= 0)
		{
			targetIcon.SetActive(false);
			transform.parent.GetComponent<EnemyGroup>().NextEnemyMove();
			SetMovement();
			return;
		}
		step--;
		//是否在player附近？攻击：移动
		if (enemyMove.canMove)
		{
			currentAction.effect.ApplyEffect(enemyCommon,player);
			actionText.text = step.ToString();
		}
		else
		{
			//移动到可攻击位置
			step = 0;
			SetSkillAction();
		}
		StartCoroutine(EnemyMoveStep());
	}

	public GameObject moveStep;
	private GameObject stepObj;
	public Sprite moveRight;
	public Sprite moveLeft;
	public Sprite moveUp;
	public Sprite moveDown;
	public Sprite attackIcon;
	public Sprite attackBg;
	public Sprite moveBg;
	private void SetMoveAction()
	{
		currentAction = attackData.effects[0];
		actionIcon.sprite = currentAction.icon;
		actionText.text = stepStatic.ToString();
		
		//显示移动路线
		SetStepDirction(0);
	}
	private void SetSkillAction()
	{
		skillReady = true;
		currentAction = attackData.effects[1];
		actionIcon.sprite = currentAction.icon;
		actionText.text = currentAction.effect.value.ToString();
		
		//显示移动路线
		SetStepDirction(1);
	}

	private void SetStepDirction(int index)//0移动；1攻击
	{
		Destroy(stepObj);
		enemyMove.FindPath();
		var difference = enemyMove.movePath[1] - transform.position;
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
		stepObj.transform.position = enemyMove.movePath[1];
		SpriteRenderer stepBg = stepObj.GetComponent<SpriteRenderer>();
		SpriteRenderer stepIcon = stepObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
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
		if (enemyMove.canMove && !skillReady)
		{
			SetStepDirction(0);
		}
	}
}
