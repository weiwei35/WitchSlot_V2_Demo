using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// 场景过渡，每个场景中都需要添加FadeCanvas
/// </summary>
public class FadeController : MonoBehaviour 
{
	private Image fadeImage;
	private float alpha = 0f;

	private void Start()
	{
		fadeImage = GetComponent<Image>();
		StartCoroutine(FadeIn());//场景加载时渐入
	}

	public void FadeTo(string sceneName)//切换场景时调用
	{
		StartCoroutine(FadeOut(sceneName));
	}

	IEnumerator FadeIn()
	{
		alpha = 1f;
		while (alpha > 0)
		{
			alpha -= Time.deltaTime;
			fadeImage.color = new Color(0, 0, 0, alpha);
			yield return null;
		}
	}
	IEnumerator FadeOut(string sceneName)
	{
		alpha = 0f;
		while (alpha < 1)
		{
			alpha += Time.deltaTime;
			fadeImage.color = new Color(0, 0, 0, alpha);
			yield return null;
		}
		SceneManager.LoadScene(sceneName);
	}
}
