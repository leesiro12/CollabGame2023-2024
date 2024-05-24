using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//code refactored from Rehope Games
public class MAudioManager : MonoBehaviour
{
    public static MAudioManager instance;
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public SoundScript[] musicClips;
    public SoundScript[] SFXClips;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  //prevents gameObject from being destroy when loading another scene
        }
        else
        {
            Destroy(gameObject);    //destroy any other instance of this
        }
    }
    private void Start()
    {
        
    }
    public void PlayMusic(string clipName)
    {
        SoundScript soundScript = Array.Find(musicClips, clip => clip.name == clipName);  //find specific audioclip by name(string)

        if (musicSource.isPlaying)
        {
            musicSource.Stop(); //stop audio source from playing to prevent looping
        }
        if (soundScript == null)
        {
            Debug.LogError("SOUND NOT FOUND");
        }
        else
        {
            musicSource.clip = soundScript.audioClip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string clipName)
    {
        SoundScript soundScript = Array.Find(SFXClips, clip => clip.name == clipName);  //find specific audioclip by name(string)

        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();   //stop audio source from playing to prevent looping
        }
        if (soundScript == null)
        {
            Debug.LogError("SOUND NOT FOUND");
        }
        else
        {
            sfxSource.PlayOneShot(soundScript.audioClip);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void MusicMuteToggle()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void SFXMuteToggle()
    {
        sfxSource.mute = !sfxSource.mute;
    }
}
