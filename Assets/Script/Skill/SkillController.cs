using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class SkillController : MonoBehaviour
{
	[HideInInspector]
	public float skillPoint;
	public TMP_Text skillPointText;
	
	bool test = false;
	private void Update()
	{
		skillPointText.text = skillPoint.ToString();
		if (canUseSkill && skillAreaObj == null)
		{
			if (currentSkill.attackArea.Count == 0)
			{
				//在地图中显示技能区域
				skillAreaObj = Instantiate(skillAreaPrefab);
			}
		}

		if (skillAreaObj != null)
		{	
			//将鼠标位置控制在格子中心
			Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
			Vector3 pos = new Vector3(Mathf.Round(mousePos.x * 1.25f) * 0.8f, Mathf.Round(mousePos.y * 1.25f) * 0.8f, 0);
			Vector3 direction = mousePos - pos;
			direction.Normalize();
			Vector3 dir = new Vector3(direction.x>0?1:-1, direction.y>0?1:-1,0);
			pos += dir * 0.4f;
			skillAreaObj.transform.position = pos;
		}

		if (canSetSkill)
		{
			if (Input.GetMouseButtonDown(0))
			{
				canSelectSkill = false;
				canSetSkill = false;
				canUseSkill = false;
				//释放法术
				SkillAttack(currentSkill);
			}
		}

		if (skillDataList.Count > 0 && !test)
		{
			test = true;
			//TEST
			foreach (var skill in skillDataList)
			{
				AddSkill(skill);
			}
		}
	}

	public void SkillAttack(SkillItemSO skill)
	{
		currentSkill = null;
		skillPoint -= skill.cost;
		EnemyGroup enemyGroup = GameObject.FindWithTag("EnemyGroup").GetComponent<EnemyGroup>();
		Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
		foreach (var enemy in enemyGroup.enemies)
		{
			if (Vector3.Distance(enemy.transform.position, skillAreaObj.transform.position) <= 0.1f)
			{
				foreach (var effect in skill.effects)
				{
					effect.ApplyEffect(player,enemy);
				}
			}
		}
		if (Vector3.Distance(player.transform.position, skillAreaObj.transform.position) <= 0.1f)
		{
			foreach (var effect in skill.effects)
			{
				effect.ApplyEffect(player,null);
			}
		}
		HideSkillInfo();
		Destroy(skillAreaObj);
	}

	public void SetSkillPoint(object value)
	{
		float valueFloat = (float) value;
		skillPoint += valueFloat;

		if (currentSkill != null)
		{
			ShowSkillInfo(currentSkill);
		}
	}
	
	[HideInInspector]
	public SkillItemSO currentSkill;
	[HideInInspector]
	public bool canSelectSkill = false;
	[HideInInspector]
	public bool canUseSkill = false;
	[HideInInspector]
	public bool canSetSkill = false;
	public GameObject skillAreaPrefab;
	[HideInInspector]
	public GameObject skillAreaObj;

	public void SetSkill()
	{
		canSelectSkill = true;
	}
	[HideInInspector]
	public List<SkillItemSO> skillDataList;//所有法术
	[HideInInspector]
	public List<SkillItemSO> playerSkillList;

	private void Start()
	{
		initSkillData();
	}

	public void initSkillData(){
		Addressables.LoadAssetsAsync<SkillItemSO>("SkillData",null).Completed += OnDataLoaded;
	}

	private void OnDataLoaded(AsyncOperationHandle<IList<SkillItemSO>> handle)
	{
		if(handle.Status == AsyncOperationStatus.Succeeded){
			skillDataList = new List<SkillItemSO>(handle.Result);
		}
		else{
			Debug.LogError("No Skill Data");
		}
	}

	public void AddSkill(SkillItemSO skill)
	{
		playerSkillList.Add(skill);
		SetSkillList(skill);
	}

	public GameObject skillListParent;
	public SkillItem_UI skillItem_UI;
	private void SetSkillList(SkillItemSO skill)
	{
		foreach (Transform skillGrid in skillListParent.transform)
		{
			if (skillGrid.childCount == 0)
			{
				var skillObj = Instantiate(skillItem_UI,skillGrid.transform);
				skillObj.skillController = this;
				skillObj.Init(skill);
				return;
			}
		}
	}

	public GameObject skillListPanel;
	public GameObject skillInfoPanel;
	public Image skillIcon;
	public TMP_Text skillName;
	public TMP_Text skillCost;
	public TMP_Text skillDesc;
	
	public Button setSkillBtn;
	public Button returnBtn;
	
	public TipsEventSO HideTipsEvent;
	public void ShowSkillInfo(SkillItemSO skill)
	{
		HideTipsEvent.RaiseEventWithGameObject(null,gameObject,this);
		
		skillListPanel.SetActive(false);
		skillInfoPanel.SetActive(true);
		skillIcon.sprite = skill.itemIcon;
		skillName.text = skill.itemName;
		skillCost.text = skill.cost.ToString();
		skillDesc.text = skill.itemDesc;
		
		currentSkill = skill;

		returnBtn.onClick.AddListener(HideSkillInfo);
		if (skillPoint >= skill.cost)
		{
			setSkillBtn.gameObject.SetActive(true);
			setSkillBtn.onClick.AddListener(SetCurrentSkill);
		}
		else
		{
			setSkillBtn.gameObject.SetActive(false);
		}
	}
	private void SetCurrentSkill()
	{
		if (currentSkill != null)
		{
			canUseSkill = true;
			canSetSkill = true;
		}
	}

	private void HideSkillInfo()
	{
		skillListPanel.SetActive(true);
		skillInfoPanel.SetActive(false);
	}
}
