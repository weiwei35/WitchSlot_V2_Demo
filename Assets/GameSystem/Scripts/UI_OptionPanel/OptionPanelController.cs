using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class OptionPanelController : MonoBehaviour 
{
	private void OnEnable()
	{
		ShowInputs();
	}

	public void BackToMenu()
	{
		gameObject.SetActive(false);
	}

	public GameObject[] Content;
	public Button[] TableButtons;
	private Button selectedButton;

	public void ShowControls()
	{
		selectedButton = TableButtons[0];
		foreach (var contentPanel in Content)
		{
			contentPanel.SetActive(false);
		}
		Content[0].SetActive(true);
	}
	public void ShowInputs()
	{
		selectedButton = TableButtons[1];
		foreach (var contentPanel in Content)
		{
			contentPanel.SetActive(false);
		}
		Content[1].SetActive(true);
	}
	public void ShowAudio()
	{
		selectedButton = TableButtons[2];
		foreach (var contentPanel in Content)
		{
			contentPanel.SetActive(false);
		}
		Content[2].SetActive(true);
	}
	public void ShowVideos()
	{
		selectedButton = TableButtons[3];
		foreach (var contentPanel in Content)
		{
			contentPanel.SetActive(false);
		}
		Content[3].SetActive(true);
	}
	public void ShowGames()
	{
		selectedButton = TableButtons[4];
		foreach (var contentPanel in Content)
		{
			contentPanel.SetActive(false);
		}
		Content[4].SetActive(true);
	}
	public AudioSO audiodata;
	private void Start()
	{
		foreach (var button in TableButtons)
		{
			button.onClick.AddListener(() => { AudioManager.instance.PlayAudio(audiodata.GetAudioNameByType("Click1"));});
		}
	}

	private void Update()
	{
		if (selectedButton != null) selectedButton.Select();
	}
}
