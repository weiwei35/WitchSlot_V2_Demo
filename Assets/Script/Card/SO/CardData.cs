using UnityEngine;

// 定义牌面等级
[CreateAssetMenu(fileName = "CardData", menuName = "Card/data")]
public class CardData : ScriptableObject {
    [Header("基础属性")]
    public CardSuit suit;
    public int rank; // 1-13 (A-K)
    public Sprite cardSprite; // 独立配置的美术图

    [Header("显示名称")]
    public string displayName; // 可选，如"King of Hearts"

    // 动态生成显示名称（可加在Awake或OnValidate）
    public void UpdateDisplayName() {
        string[] rankNames = {"A","2","3","4","5","6","7","8","9","10","J","Q","K"};
        displayName = $"{rankNames[rank-1]} of {suit}";
    }
}