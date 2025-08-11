
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioSO", menuName = "AudioSO/Audio")]
public class AudioSO : ScriptableObject 
{
    public List<AudioData> audios = new List<AudioData>();

    public string GetAudioNameByType(string type)
    {
        foreach (var audio in audios)
        {
            if (audio.type == type)
            {
                return audio.clip.name;
            }
        }

        return null;
    }
    [Serializable]
    public class AudioData
    {
        public AudioClip clip;
        public string type;
    }
}
