using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool IsMute = false;
    public float MusicVolume = 1f;
    public float SFXVoulume = 1f;
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource soundEffect;
    public AudioSource soundMusic;
    public AudioSource audioSourcePlayerFootSteps;
    public AudioSource audioEnemyPatrol;
    public SoundType[] Sounds;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetMusicVolume(0.1f);
        PlayMusic(global::Sounds.Music);
    }
    public void Play(Sounds sound)
    {
        if (IsMute) return;
        AudioClip clip = GetSoundClip(sound);
        if(clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }
    }

    public void PlayEnemyPatrolSound(Sounds sound)
    {
        if (IsMute) return;
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            audioEnemyPatrol.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }

    }


    public void PlayPlayerFootstepsSound(Sounds sound)
    {
        if (IsMute) return;
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            audioSourcePlayerFootSteps.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }
    }
    public void PlayMusic(Sounds sound)
    {
        if (IsMute) return;

        AudioClip clip = GetSoundClip(sound);
        if(clip!=null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        soundMusic.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXVoulume = volume;
        soundEffect.volume = volume;
    }
    public void Mute(bool status)
    {
        IsMute = status;
    }
    private AudioClip GetSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundTypes == sound);
        if(item !=null)
        {
            return item.soundClip;
        }
        else
        return null;
    }
}

[Serializable] 
public class SoundType
{
    public Sounds soundTypes;
    public AudioClip soundClip;
}

public enum Sounds
{
    ButtonClick, PlayerDeath, EnemyDeath, Music, PlayerFootsteps, EnemyPatrol
}
