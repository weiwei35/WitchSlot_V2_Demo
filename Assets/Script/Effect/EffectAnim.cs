using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EffectAnim : MonoBehaviour 
{
	public void DestroyEffect()
	{
		Destroy(this.gameObject);
	}
}
