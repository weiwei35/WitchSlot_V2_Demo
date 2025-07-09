using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class TipsItem : MonoBehaviour 
{
	public TextMeshProUGUI title;
	public TextMeshProUGUI desc;
	public LayoutElement layoutElement;
	
	private void Update()
	{
		layoutElement.enabled = Mathf.Max(title.preferredWidth, desc.preferredWidth) > layoutElement.preferredWidth;
	}
}
