using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ToolFunctions : MonoBehaviour 
{
	/// <summary>
	/// 按权重随机抽选一个列表中元素
	/// </summary>
	/// <param name="items"></param>
	/// <param name="weights"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static T WeightedRandom<T>(List<T> items, List<float> weights)
	{
		// 1. 验证输入有效性
		if (items == null || weights == null || items.Count == 0 || items.Count != weights.Count)
			throw new ArgumentException("无效的输入列表");

		// 2. 计算总权重
		float totalWeight = 0f;
		foreach (float weight in weights)
		{
			if (weight < 0) 
				throw new ArgumentException("权重值不能为负数");
			totalWeight += weight;
		}

		// 3. 生成随机位置
		float randomValue = Random.Range(0f, totalWeight);

		// 4. 查找对应的元素
		float cumulativeWeight = 0f;
		for (int i = 0; i < items.Count; i++)
		{
			cumulativeWeight += weights[i];
			if (randomValue <= cumulativeWeight)
			{
				return items[i];
			}
		}
    
		// 5. 安全后备方案（理论上不应执行到这里）
		return items[items.Count - 1];
	}

	/// <summary>
	/// 绕原点(0,0)顺时针旋转angle度
	/// </summary>
	/// <param name="originalPoint">原始坐标</param>
	/// <returns>旋转后的新坐标</returns>
	public static Vector2Int RotateGridInt(Vector2Int originalPoint, int angle)
	{
		// 将角度归一化到0-360度范围
		angle = ((angle % 360) + 360) % 360;
    
		// 确保角度是90的倍数
		int normalizedAngle = (angle / 90) % 4 * 90;
    
		switch (normalizedAngle)
		{
			// 90度旋转：(x, y) → (y, -x)
			case 90:
				return new Vector2Int(originalPoint.y, -originalPoint.x);
        
			// 180度旋转：(x, y) → (-x, -y)
			case 180:
				return new Vector2Int(-originalPoint.x, -originalPoint.y);
        
			// 270度旋转：(x, y) → (-y, x)
			case 270:
				return new Vector2Int(-originalPoint.y, originalPoint.x);
        
			// 0/360度：保持原位置
			default:
				return new Vector2Int(originalPoint.x, originalPoint.y);
		}
	}

    
	/// <summary>
	/// 绕原点(0,0)顺时针旋转90度 (浮点版本)
	/// </summary>
	/// <param name="originalPoint">原始坐标</param>
	/// <returns>旋转后的新坐标</returns>
	public static Vector2 RotateGridFloat(Vector2 originalPoint)
	{
		// 旋转公式: (x, y) → (y, -x)
		return new Vector2(
			originalPoint.y, 
			-originalPoint.x
		);
	}
	/// <summary>
	/// 计算终点与起点之间的角度差
	/// </summary>
	/// <param name="startAngle"></param>
	/// <param name="endAngle"></param>
	/// <returns></returns>
	public static int CalculateClockwiseRotation(int startAngle, int endAngle)
	{
		// 将角度转换为0-3的索引（0°=0, 90°=1, 180°=2, 270°=3）
		int startIndex = (startAngle % 360) / 90;
		int endIndex = (endAngle % 360) / 90;
    
		// 计算顺时针方向步数差
		int steps = (endIndex - startIndex + 4) % 4;
    
		return steps * 90; // 转换为角度
	}

	/// <summary>
	/// 获取tilemap区域
	/// </summary>
	/// <param name="tilemap"></param>
	/// <returns></returns>
	public static Bounds GetTilemapBounds(Tilemap tilemap)
	{
		Vector3 min = new Vector3(float.MaxValue, float.MaxValue);
		Vector3 max = new Vector3(float.MinValue, float.MinValue);
		bool found = false;
        
		foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
		{
			if (tilemap.HasTile(pos))
			{
				found = true;
				Vector3 worldPos = tilemap.CellToWorld(pos);
				worldPos += tilemap.cellSize * 0.5f; // 中心对齐
                
				if (worldPos.x < min.x) min.x = worldPos.x;
				if (worldPos.y < min.y) min.y = worldPos.y;
				if (worldPos.x > max.x) max.x = worldPos.x;
				if (worldPos.y > max.y) max.y = worldPos.y;
			}
		}
        
		return found ? new Bounds((min + max)*0.5f, max-min+tilemap.cellSize) : new Bounds();
	}
	
	/// <summary>
	/// 根据武器设置攻击范围
	/// </summary>
	/// <param name="weapons"></param>
	/// <returns></returns>
	public static List<Vector2Int> SetGrid(List<WeaponSO> weapons)
	{
		List<Vector2Int> gridPos = new List<Vector2Int>();
		foreach (var weapon in weapons)
		{
			// Debug.LogError(weapon.weaponName);
			foreach (var pos in weapon.hurtArea)
			{
				if(!gridPos.Contains(pos)) gridPos.Add(pos);
			}
		}
		return gridPos;
	}

	/// <summary>
	/// 动态设置敌人的生命值
	/// </summary>
	/// <param name="enemy"></param>
	public static void SetEnemyHP(EnemyCommon enemy)
	{
		string assetPath = "Assets/GameData/EnemyHP/";
		var enemyHP = ScriptableObject.CreateInstance<IntVariable>();
		int enemyHPCount = (int)enemy.enemyHP;
		enemyHP.maxValue = enemyHPCount;
		enemyHP.currentValue = enemyHPCount;
		enemy.hp = enemyHP;
		//保存怪物生命值文件
		if (!Directory.Exists(assetPath))
			Directory.CreateDirectory(assetPath);
		string fullPath = assetPath + "/" +enemy.GetInstanceID()+ "Data.asset";
		UnityEditor.AssetDatabase.DeleteAsset(fullPath);
		UnityEditor.AssetDatabase.CreateAsset(enemyHP, fullPath);
		UnityEditor.AssetDatabase.Refresh();
	}

#if UNITY_EDITOR
	private void OnApplicationQuit()
	{
		string assetPath = "Assets/GameData/EnemyHP/";
		string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject", new[] { assetPath });
    
		foreach (string guid in guids) {
			string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
			UnityEditor.AssetDatabase.DeleteAsset(path);
		}
    
		UnityEditor.AssetDatabase.Refresh();
	}
#endif


}
