using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "weapon", menuName = "Weapon/Library")]
public class WeaponLibraryData : ScriptableObject 
{
	public List<WeaponSO> weapons = new(); 
}
