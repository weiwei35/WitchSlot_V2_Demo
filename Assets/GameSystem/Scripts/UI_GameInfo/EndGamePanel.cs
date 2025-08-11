using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class EndGamePanel : MonoBehaviour
{
	public TMP_Text gameInfoText;
	public void SetInfo(int level,float score)
	{
		gameInfoText.text = "Level Count:"+level+"\n"+"Score Sum:"+score;//结算信息
	}
}
