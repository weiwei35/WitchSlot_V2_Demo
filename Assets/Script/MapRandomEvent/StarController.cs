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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			transform.DOMove(other.transform.position, 0.2f).SetAutoKill(true).SetLink(gameObject).OnComplete(() =>
			{
				DOTween.Kill(gameObject);
				GetStarEvent.RaiseEvent(null,this);
				Destroy(gameObject);
			});
		}
	}
}
