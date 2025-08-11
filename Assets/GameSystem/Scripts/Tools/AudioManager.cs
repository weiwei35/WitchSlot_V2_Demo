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
public class AudioManager : MonoBehaviour 
{
	//单例
	public static AudioManager instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public List<Audio> audios = new List<Audio>();//所有音频
	Dictionary<string,AudioSource> audioDictionary = new Dictionary<string, AudioSource>();
	private void Start()//初始化所有音频
	{
		foreach (var audio in audios)
		{
			GameObject audioObject = new GameObject(audio.audioClip.name);
			audioObject.transform.SetParent(transform);
			
			AudioSource audioSource = audioObject.AddComponent<AudioSource>();
			audioSource.clip = audio.audioClip;
			audioSource.playOnAwake = audio.playOnAwake;
			audioSource.loop = audio.loop;
			audioSource.volume = audio.volume;
			audioSource.outputAudioMixerGroup = audio.audioMixerGroup;

			if (audioSource.playOnAwake)
			{
				audioSource.Play();
			}
			
			audioDictionary.Add(audio.audioClip.name, audioSource);
		}
	}

	public void PlayAudio(string audioClipName, bool needWaitBefore = false)
	{
		if (!instance.audioDictionary.ContainsKey(audioClipName))
		{
			Debug.LogWarning("Audio clip " + audioClipName + " not exist");
			return;
		}

		if (needWaitBefore)
		{
			if(!instance.audioDictionary[audioClipName].isPlaying)
				instance.audioDictionary[audioClipName].Play();
		}else instance.audioDictionary[audioClipName].Play();
	}

	public void StopAudio(string audioClipName)
	{
		if (!instance.audioDictionary.ContainsKey(audioClipName))
		{
			Debug.LogWarning("Audio clip " + audioClipName + " not exist");
			return;
		}
		instance.audioDictionary[audioClipName].Stop();
	}
}
[Serializable]
public class Audio
{
	public AudioClip audioClip;
	public bool playOnAwake;
	public bool loop;
	[Range(0,1)]public float volume = 1;
	public AudioMixerGroup audioMixerGroup;
}