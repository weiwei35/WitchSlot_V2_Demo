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
	public GameObject symbolParent;
	public WeaponSymbolUI symbolGrid;
	public WeaponManager weaponManager;
	
	public List<OutFight_GridObj_UI> gridObjects;

	public void SetGridObj()
	{
		gridObjects.Clear();
		List<WeaponSO> currentWeapons = new List<WeaponSO>();
		currentWeapons.Add(RandomWeaponWithType(weaponManager.weaponDataList,WeaponType.武器));
		currentWeapons.Add(RandomWeaponWithType(weaponManager.weaponDataList,WeaponType.衣服));
		// List<WeaponSO> currentWeapons = RandomWeapon(weaponManager.weaponDataList, 2);
		SetWeapon(currentWeapons);
		SetWeaponSymbolGrid(currentWeapons);
	}

	public void EndSymbolSet()
	{
		if (symbolParent.transform.childCount == 0)
		{
			GetComponent<SlotSystem>().RemoveAllSlots();
		}
	}

	public void SetWeaponSymbolGrid(List<WeaponSO> weapons)
	{
		int count = 0;
		foreach (var weapon in weapons)
		{
			List<SymbolSO> symbols =ToolFunctions.GetWeaponSymbol(weapon);
			foreach (var symbol in symbols)
			{
				count++;
			}
		}
		int width = count * 100+(count-1)*20;
		float startX = width / 2;
		int i = 0;
		foreach (var weapon in weapons)
		{
			List<SymbolSO> symbols =ToolFunctions.GetWeaponSymbol(weapon);
            foreach (var symbol in symbols)
            {
            	var obj = Instantiate(symbolGrid, symbolParent.transform);
	            obj.transform.position -= new Vector3(startX, 0, 0);
	            obj.transform.position += new Vector3(i*120, 0, 0);
	            obj.transform.position += new Vector3(50, 0, 0);
            	obj.name = symbol.symbolName;
	            obj.symbolIcon.sprite = symbol.symbolIcon;
	            obj.symbol = symbol;
	            i++;
            }
		}
	}

	public void SubmitBtn()
	{
		DraggableImage[] draggableImages = GameObject.FindObjectsOfType<DraggableImage>();
		List<Vector2Int> pos = new List<Vector2Int>();
		Dictionary<Vector2Int,SymbolSO> symbolList = new Dictionary<Vector2Int, SymbolSO>();
		foreach (var drag in draggableImages)
		{
			WeaponSymbolUI symbol = drag.GetComponent<WeaponSymbolUI>();
			pos.Add(drag.symbolPos);
			symbolList.Add(drag.symbolPos,symbol.symbol);
		}
		
		SetGridHurtArea.RaiseEvent(symbolList,this);
	}

	public Color selectColor;
	public Color defaultColor;
	public void ShowWeaponHurtArea(object o)
	{
		List<Vector2Int> gridPos = ((List<Vector2Int>)o).ToList();
		foreach (var pos in gridPos)
		{
			foreach (var grid in gridObjects)
			{
				if (grid.gridPos == pos)
				{
					grid.gameObject.GetComponent<Image>().color = selectColor;
				}
			}
		}
	}

	public void ResetWeaponHurtArea()
	{
		foreach (var grid in gridObjects)
		{
			grid.gameObject.GetComponent<Image>().color = defaultColor;
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
		LayoutRebuilder.ForceRebuildLayoutImmediate(weaponParent.GetComponent<RectTransform>());
	}

	public WeaponSO RandomWeaponWithType(List<WeaponSO> source,WeaponType type)
	{
		List<WeaponSO> weapons = new List<WeaponSO>();
		foreach (var weapon in source)
		{
			if(weapon.type == type) weapons.Add(weapon);
		}
		
		return weapons[Random.Range(0, weapons.Count)];
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
	public DictionaryEventSO SetGridHurtArea;
	public void SubmitGrid(List<Vector2Int> pos)
	{
		// playerFaceGridPos = new Vector2Int(1,0);
		// // gridUI.InitGrid(gridPos);
		// SetGridHurtArea.RaiseEvent(pos,this);
	}
}
