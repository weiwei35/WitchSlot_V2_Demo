using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapon",menuName = "Weapon/data")]
public class WeaponSO : ItemInfoDataSO
{
    public int coldTime;
    public WeaponType type;
    public RandomType randomType;
    // public List<Vector2Int> hurtArea;
    // public SymbolSO symbol;
    public List<SymbolList> symbolList;
}

[Serializable]
public class SymbolList
{
    public SymbolSO symbol;
    public List<Vector2Int> area;
    public List<Vector2Int> selectArea;
}