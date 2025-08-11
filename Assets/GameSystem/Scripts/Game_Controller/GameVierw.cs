using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GameView : MonoBehaviour 
{
	public TMP_Text levelText;
	GameController gameController;//游戏数据处理器

	public Button[] buttons;
	public AudioSO audiodata;
	private void Start()
	{
		gameController = GetComponent<GameController>();
		foreach (var button in buttons)
		{
			button.onClick.AddListener(() => { AudioManager.instance.PlayAudio(audiodata.GetAudioNameByType("Click2"));});
		}
	}
	
	private void Update()
	{
		levelText.text = gameController.level.ToString();//游戏信息
	}
	
	public GameObject nextGamePanel;
	public void LevelWin()
	{
		//胜利弹窗
		nextGamePanel.gameObject.SetActive(true);
	}
	
	public EndGamePanel endGamePanel;
	public void LevelLose(int level,float score)
	{
		//结算
		endGamePanel.gameObject.SetActive(true);
		endGamePanel.SetInfo(level,score);
	}
	
	public FadeController fadeImage;
	public void BackToMainMenu()
	{
		fadeImage.FadeTo("StartScene");//加载主菜单场景
	}
}
