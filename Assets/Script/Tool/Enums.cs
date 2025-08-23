using UnityEngine;

public class Enums : MonoBehaviour 
{
    
}

public enum SymbolType
{
    空,
    烈火,
    寒冰,
    奥术
}
public enum EffectTargetType
{
    Self,
    One,
    All,
    Some,
    Random
}

public enum EffectType
{
    伤害,
    护甲,
    治愈
}
public enum RandomType
{
    Normal,
    Rare,
    Historic,
    Legendary
}

public enum RoomType
{
    怪物,
    精英怪,
    BOSS,
    商店,
    事件,
    休息
}
public enum Direction { Up, Down, Left, Right, Empty}

public enum WeaponType
{
    鞋子,
    武器,
    衣服,
    珠宝
}

// 定义花色枚举
public enum CardSuit {
    Spades,     // 黑桃
    Hearts,     // 红心
    Diamonds,   // 方片
    Clubs       // 梅花
}
public enum PokerHand {
    高牌,         // 高牌
    一对,          // 一对
    顺子,         // 顺子
    同花,            // 同花
    同花顺,
    三条,     // 三条
}