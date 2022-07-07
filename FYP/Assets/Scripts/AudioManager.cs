using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    void Awake()
    {
        instance = this;
        Sound.Initialize();
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound.SoundType sound;
        public AudioClip audioClip;
    }

    public SoundAudioClip[] soundAudioClipsArray;
}

