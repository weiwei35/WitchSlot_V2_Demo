using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class StartPanelController : MonoBehaviour 
{
	public void OnGameStart()
	{
		// fadeImage.FadeTo("GameScene");//加载游戏场景
	}
	public void OnGameExit()//退出游戏
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit();
	}

	public void ShowAboutUs()
	{
		
	}
	public GameObject SettingPanel;
	public void ShowSetting()//设置界面
	{
		SettingPanel.SetActive(true);
	}
	
	public Button[] buttons;
	public AudioSO audiodata;
	
	[Header("FeedBacks")]
	public MMF_Player openStartPanelFeedback;
	private void Start()
	{
		foreach (var button in buttons)
		{
			button.onClick.AddListener(() => { AudioManager.instance.PlayAudio(audiodata.GetAudioNameByType("Click1"));});
		}
		openStartPanelFeedback.PlayFeedbacks();
	}
}
