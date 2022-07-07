using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sound
{
    public enum SoundType
    {
        zombieScream,
        spiderScream,
        skeletonScream,
        eatingBait,
        mcWalking,
        mcDamaged,
        bossSwipe,
        enemySwipe,
    }

    private static Dictionary<SoundType, float> soundTimerDictionary;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<SoundType, float>();
        soundTimerDictionary[SoundType.zombieScream] = 0f;
        soundTimerDictionary[SoundType.skeletonScream] = 0f;
        soundTimerDictionary[SoundType.spiderScream] = 0f;
        soundTimerDictionary[SoundType.bossSwipe] = 0f;
        soundTimerDictionary[SoundType.eatingBait] = 0f;
        soundTimerDictionary[SoundType.mcWalking] = 0f;
    }

    //3D sounds
    public static void PlaySound(SoundType sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            //create and play audio source
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = getAudioClip(sound);
            //modify audio source components
            audioSource.volume = .2f;
            audioSource.pitch = 1f;
            audioSource.spatialBlend = 1f;
            audioSource.maxDistance = 100f;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    //2D sounds
    public static void PlaySound(SoundType sound)
    {
        if (CanPlaySound(sound))
        {
            //create and play audio source
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(getAudioClip(sound));
        }
    }

    private static bool CanPlaySound(SoundType sound)
    {
        switch (sound)
        {
            default:
                return true;

            //if in update, need a timer to control it
            case SoundType.zombieScream:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float zombieScreamTimerMax = 5f;

                    if (lastTimePlayed + zombieScreamTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;

            case SoundType.spiderScream:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float spiderScreamTimerMax = 5f;

                    if (lastTimePlayed + spiderScreamTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;

            case SoundType.skeletonScream:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float skeletonScreamTimerMax = 5f;

                    if (lastTimePlayed + skeletonScreamTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;

            case SoundType.bossSwipe:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float bossSwipeTimerMax = .8f;

                    if (lastTimePlayed + bossSwipeTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;

            case SoundType.eatingBait:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float eatingBaitTimerMax = 5f;

                    if (lastTimePlayed + eatingBaitTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;

            case SoundType.mcWalking:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float mcWalkingTimerMax = .8f;

                    if (lastTimePlayed + mcWalkingTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;
        }
    }
    
    //get the audio clip in the array
    private static AudioClip getAudioClip(SoundType sound)
    {
        foreach(AudioManager.SoundAudioClip soundAudioClip in AudioManager.instance.soundAudioClipsArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("sound " + sound + " not found");
        return null;
    }


}
