using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class StarController : MonoBehaviour
{
	public ObjectEventSO GetStarEvent;
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			DOTween.Kill(gameObject);
			GetStarEvent.RaiseEvent(null,this);
			Destroy(gameObject);
		}
	}
}
