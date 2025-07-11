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
public enum Direction { Up, Down, Left, Right }

public enum WeaponType
{
    帽子,
    武器,
    衣服,
    珠宝
}