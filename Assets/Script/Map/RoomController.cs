using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class RoomController : MonoBehaviour
{
	public Tilemap bgTilemap;
	public Tilemap wallTilemap;
	public GameObject next;
	public void ShowNext(object o)
	{
		next.SetActive(true);
	}
}
