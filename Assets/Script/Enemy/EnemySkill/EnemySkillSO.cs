
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy/Skill")]
//记录技能描述
public class EnemySkillSO : ScriptableObject 
{
	public string skillName;
	[TextArea]
	public string skillDesc;
}
