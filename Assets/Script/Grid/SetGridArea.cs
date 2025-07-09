using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
//using NaughtyAttributes;
using Random = UnityEngine.Random;
/// <summary>
/// 从武器库随机武器，设置攻击范围UI显示
/// </summary>
public class SetGridArea : MonoBehaviour
{
	public WeaponManager weaponManager;
	
	public List<OutFight_GridObj_UI> gridObjects;
	public OutFight_GridObj_UI gridObjectPrefab;
	private List<Vector2Int> gridPos;
	public void SetGridObj()
	{
		gridPos = new List<Vector2Int>();
		List<WeaponSO> currentWeapons = RandomWeapon(weaponManager.weaponDataList, 2);
		gridPos = ToolFunctions.SetGrid(currentWeapons);
		SetWeapon(currentWeapons);

		foreach (var pos in gridPos)
		{
			var grid = Instantiate(gridObjectPrefab, transform);
			grid.transform.localPosition = new Vector3(pos.x,pos.y,0)*120;
			grid.gridPos = pos;
			gridObjects.Add(grid);
		}
	}

	public WeaponItem_UI weaponItem;
	public GameObject weaponParent;
	private void SetWeapon(List<WeaponSO> currentWeapons)
	{
		weaponManager.playerWeaponLibrary.weapons.Clear();
		foreach (var weapon in currentWeapons)
		{
			var weaponObj = Instantiate(weaponItem, weaponParent.transform);
			weaponObj.weapon = weapon;
			weaponObj.Init();
			
			weaponManager.playerWeaponLibrary.weapons.Add(weapon);
		}
	}

	//随机选取两个不同type的装备
	public List<WeaponSO> RandomWeapon(List<WeaponSO> source, int n)
	{
		// 验证输入数据
		if (source == null || source.Count == 0 || n <= 0) 
			return new List<WeaponSO>();
        
		// 获取类型分组，确保每个类型组非空
		var grouped = source
			.GroupBy(item => item.type)
			.Where(g => g.Any())
			.ToList();
        
		if (grouped.Count == 0) return new List<WeaponSO>();
        
		// 随机选择n个不同的类型组
		var typeGroups = grouped
			.OrderBy(_ => Guid.NewGuid()) // 随机排序
			.Take(Math.Min(n, grouped.Count))
			.ToList();
        
		// 从每组中随机选取一个项
		var result = new List<WeaponSO>();
        
		foreach (var group in typeGroups)
		{
			var randomIndex = Random.Range(0,group.Count());
			result.Add(group.Skip(randomIndex).First());
		}
        
		return result;
	}
	
	[Header("玩家面向")]
	public static Vector2Int playerFaceGridPos;
	public GridView_UI gridUI;
	public ObjectEventSO SetGridHurtArea;
	public void SubmitGrid()
	{
		playerFaceGridPos = new Vector2Int(1,0);
		gridUI.InitGrid(gridPos);
		SetGridHurtArea.RaiseEvent(gridPos,this);
	}
}
