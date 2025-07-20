using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class LoadingHallItems : MonoBehaviour 
{
	[HideInInspector]
	public static bool getWeapon = false;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerEnter();
		}
	}

	public virtual void PlayerEnter()
	{
		
	}
}
