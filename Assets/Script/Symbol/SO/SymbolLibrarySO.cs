using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SymbolLibrary", menuName = "Symbol/Library")]
public class SymbolLibrarySO : ScriptableObject 
{
	public List<SymbolLibrary> symbols = new(); 
}

[Serializable]
public class SymbolLibrary{
	public SymbolSO symbolData;
	public int amount;
}
