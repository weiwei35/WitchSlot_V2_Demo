using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillItem", menuName = "ItemInfo/Skill")]
public class SkillItemSO : ItemInfoDataSO 
{
	public SymbolType skillType;

	public int cost;
	public float attck;
	public List<SymbolAttackGrid> attackArea;
	
	public List<Effect> effects;
}
