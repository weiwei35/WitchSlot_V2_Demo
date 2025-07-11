using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class FightController : MonoBehaviour 
{
	//单例
	public static FightController instance;
	public bool inFight;
	private void Awake()
	{
		if(instance == null){
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		startRound += StartMove;
		endRound += EndFight;
	}
	//选房间
	public GameObject PickRoomPanel;
	public void OpenPickRoomPanel(object o)
	{
		PickRoomPanel.SetActive(true);
		setRoom?.Invoke();
	}

	public void FirstSetRoom()
	{
		setRoom?.Invoke();
	}
	//结束战斗
	public ObjectEventSO ShowNextEvent;
	private void EndFight()
	{
		inFight = false;
		//加载下层楼梯
		if(enemyGroup.enemies.Count == 0)
			ShowNextEvent.RaiseEvent(null,this);
	}

	//开始战斗--只加入范围内的怪
	public void StartFight()
	{
		inFight = true;
		FightCount();
		SetFightIcon();
		SetTarget();
		
		prepareFight?.Invoke();
	}

	public void AddFight()
	{
		FightCount();
		SetFightIcon();
		SetTarget();
		
		meetEnemy?.Invoke();
	}

	private void StartMove()
	{
		sortedList[fightIndexCurrent].myTurn();
	}
	
	//结束当前回合
	public void NextStep()
	{
		endStep?.Invoke();
		if(fightIndexCurrent<sortedList.Count-1) fightIndexCurrent++;
		else fightIndexCurrent = 0;
		sortedList[fightIndexCurrent].myTurn();

		SetTarget();
	}

	//重新展示攻击顺序（有怪被击杀）
	public void ReCountFightWeight(int fightIndex)
	{
		if(fightIndex < fightIndexCurrent) fightIndexCurrent--;
		foreach (var fightObj in sortedList.ToList())
		{
			if (fightObj.fightIndex == fightIndex)
			{
				sortedList.Remove(fightObj);
			}
		}

		SetFightIcon();
		SetTarget();
	}
	
	//攻击顺序展示
	public GameObject fightIconGroup;
	public GameObject fightIcon;
	List<GameObject> fightIconList = new List<GameObject>();
	void SetFightIcon()
	{
		foreach (Transform fight in fightIconGroup.transform)
		{
			Destroy(fight.gameObject);
		}
		fightIconList.Clear();
		foreach (var fight in sortedList)
		{
			var fightObj = Instantiate(fightIcon,fightIconGroup.transform);
			if (fight.playerFight != null) fightObj.GetComponent<Image>().sprite = fight.playerFight.icon;
			if (fight.enemyController != null) fightObj.GetComponent<Image>().sprite = fight.enemyController.enemyCommon.enemyIcon;
			
			fightIconList.Add(fightObj);
		}
	}

	void SetTarget()
	{
		foreach (var fight in fightIconList)
        {
        	if (fight != null) fight.transform.GetChild(0).gameObject.SetActive(false);
        }

		if (fightIndexCurrent < fightIconList.Count - 1)
			fightIconList[fightIndexCurrent].transform.GetChild(0).gameObject.SetActive(true);
	}
	
	//先攻计算
	private EnemyGroup enemyGroup;
	private PlayerFight playerFight;
	public int fightIndexCurrent = 0;
	private List<FightWeight> sortedList;
	public void FightCount()
	{
		objectsToSort.Clear();
		enemyGroup = GameObject.FindWithTag("EnemyGroup").GetComponent<EnemyGroup>();
		playerFight = GameObject.FindWithTag("Player").GetComponent<PlayerFight>();
		foreach (var enemy in enemyGroup.enemiesInFight)
		{
			objectsToSort.Add(enemy.fightweight);
		}
		// 执行带权重的随机排序
		sortedList = WeightedRandomSort(objectsToSort);
		sortedList.Insert(0,playerFight.fightWeight);//角色先开始攻击
		// 输出结果
		LogController.instance.logDelegate?.Invoke("攻击顺序：");
		foreach (var item in sortedList)
		{
			LogController.instance.logDelegate?.Invoke(item.name);
		}
		for (int i = 0; i < sortedList.Count; i++)
		{
			sortedList[i].fightIndex = i;
		}
	}

	public List<FightWeight> objectsToSort = new List<FightWeight>();
	// 带权重的随机排序方法
	public static List<T> WeightedRandomSort<T>(List<T> items) where T : FightWeight
	{
		List<T> result = new List<T>();
		List<T> workingList = new List<T>(items);
        
		int totalItems = items.Count;
		for (int i = 0; i < totalItems; i++)
		{
			// 1. 计算权重和
			float totalWeight = workingList.Sum(item => item.weight);
            
			// 2. 随机选择 (权重越高被选中的概率越大)
			float randomPoint = Random.Range(0, totalWeight);
            
			// 3. 找到随机点对应的项目
			float cumulativeWeight = 0;
			foreach (var item in workingList)
			{
				cumulativeWeight += item.weight;
				if (randomPoint <= cumulativeWeight)
				{
					result.Add(item);
					workingList.Remove(item);
					break;
				}
			}
		}
        
		return result;
	}

	//委托
	public delegate void MeetEnemy();//遭遇怪物
	public MeetEnemy meetEnemy;
	
	public delegate void SetRoom();//设置房间
	public SetRoom setRoom;
	public delegate void PrepareFight();//准备战斗信息
	public PrepareFight prepareFight;
	public delegate void StartRound();//准备结束正式开战
	public StartRound startRound;
	public delegate void EndStep();//回合结束
	public EndStep endStep;
	public delegate void EndRound();//战斗结束
	public EndRound endRound;
}
