using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAudioManager : MonoBehaviour
{
    public static MAudioManager instance;
    public AudioSource[] SFX, music;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        SFX = GetComponents<AudioSource>();
    }

    public void PlaySFX(AudioClip audioClip, float volume, Transform transform)
    {
        //AudioSource audioSource = Instantiate(SFX, transform.position, Quaternion.identity);
    }

    public void StopSounds()
    {
        foreach (AudioSource audioSource in SFX)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        foreach (AudioSource audioSource in SFX)
        {
            audioSource.volume = volume;
        }
    }
}
