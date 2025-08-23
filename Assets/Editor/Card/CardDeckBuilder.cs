using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CardDeckBuilder : MonoBehaviour 
{
	const string cardPath = "Assets/Art/CardArt/";
	const string outputFolder = "Assets/GameData/Cards/";
	[MenuItem("Tools/Build Card Deck")]
	static void BuildDeck() {
		Debug.Log("🛠️ BuildDeck 方法被调用");
		if (!Directory.Exists(cardPath)) {
			Debug.LogError($"路径不存在: {cardPath}");
			return;
		}
		FileUtil.DeleteFileOrDirectory(outputFolder);
		Directory.CreateDirectory(outputFolder);
		int cardsCreated = 0;
		foreach (string filePath in Directory.GetFiles(cardPath, "*.png")) {
			string fileName = Path.GetFileNameWithoutExtension(filePath);
			CardData card = ScriptableObject.CreateInstance<CardData>();
			card.displayName = fileName;
			card.cardSprite = AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
			string[] parts = fileName.Split('_');
			if (parts.Length != 2)
			{
				Debug.LogWarning($"⚠️ 资源名格式错误：{fileName}，应为 <suit>_<rank> 格式");
				continue;
			}
			string suitStr = parts[0];
			string rankStr = parts[1];
			// 转换 suit 字符串为枚举
			try
			{
				card.suit = (CardSuit)Enum.Parse(typeof(CardSuit), suitStr, true);
			}
			catch (Exception)
			{
				Debug.LogError($"❌ 找不到对应的 CardSuit 枚举：{suitStr}");
				continue;
			}
			// 转换 rank 字符串为整数
			if (!int.TryParse(rankStr, out int rank))
			{
				Debug.LogError($"❌ rank 格式错误：{rankStr}");
				continue;
			}
			card.rank = rank;
            
			AssetDatabase.CreateAsset(card, 
				$"{outputFolder}/{fileName}.asset");
			cardsCreated++;
		}
		AssetDatabase.SaveAssets();
		Debug.Log($"✅ 成功创建 {cardsCreated} 张卡牌资源");
	}
}
