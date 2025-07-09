using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Symbol", menuName = "Symbol/AddSymbol")]
public class SymbolSO : ScriptableObject 
{
	public string symbolName;
    public float symbolAttack;//用于显示的伤害值
    public List<SymbolAttackGrid> symbolAttacks;
    public Sprite symbolIcon;
    public float randomRank = 10;
    public SymbolType symbolType;
    public RandomType randomType;
    
    public List<Effect> effects;
}

[Serializable]
public class SymbolAttackGrid
{
	public Vector3Int position;
	public float attack;
}