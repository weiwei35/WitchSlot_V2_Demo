using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class AudioSetting : MonoBehaviour 
{
	public AudioMixer mixer;

	public void SetBGMVolume(float volume)
	{
		mixer.SetFloat("BGM", volume);
	}
	public void SetSFXVolume(float volume)
	{
		mixer.SetFloat("SFX", volume);
	}

	public void SetMute(bool mute)
	{
		if (mute)
			mixer.SetFloat("Master", -80);
		else
			mixer.SetFloat("Master", 0);
	}
}
