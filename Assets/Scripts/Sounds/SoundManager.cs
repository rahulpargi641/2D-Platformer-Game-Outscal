using System;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    public bool IsMute = false;
    public float MusicVolume = 1f;
    public float SFXVoulume = 1f;
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource audioButtonClick;
    public AudioSource audioBgMusic;
    public AudioSource audioPlayerRun;
    public AudioSource audioEnemyWalk;
    public AudioSource audioLevelRelated;
    public AudioSource audioInteract;
    public AudioSource audioPlayerRelated;
    public SoundType[] m_SoundType;
 
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
        PlayBgMusic(ESounds.BackgroundMusic);
    }
    public void PlayButtonClickSound(ESounds sound)
    {
        if (IsMute) return;

        AudioClip clip = GetSoundClip(sound);
        if(clip != null)
        {
            audioButtonClick.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }
    }

    public void PlayEnemyPatrolSound(ESounds sound)
    {
        if (IsMute) return;
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            audioEnemyWalk.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }

    }


    public void PlayPlayerFootstepsSound(ESounds sound)
    {
        if (IsMute) return;
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            audioPlayerRun.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }
    }

    public void PlayPlayerRelatedSound(ESounds sound)
    {
        if (IsMute) return;
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            audioPlayerRelated.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }
    }
    public void PlayBgMusic(ESounds sound)
    {
        if (IsMute) return;

        AudioClip clip = GetSoundClip(sound);
        if(clip!=null)
        {
            audioBgMusic.clip = clip;
            audioBgMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound types" + sound);
        }
    }

    public void PlayLevelCompleteMusic(ESounds sound)
    {
        if (IsMute) return;

        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            audioLevelRelated.clip = clip;
            audioLevelRelated.Play();
        }
        else
        {
            Debug.LogError("Level COmplete Clip not found for sound types" + sound);
        }
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        audioBgMusic.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXVoulume = volume;
        audioButtonClick.volume = volume;
    }
    public void Mute(bool status)
    {
        IsMute = status;
    }
    private AudioClip GetSoundClip(ESounds sound)
    {
        SoundType item = Array.Find(m_SoundType, item => item.ES_Sound == sound);
        if (item != null)
            return item.m_AudioClip;
        else
            return null;
    }
}

[Serializable] 
public class SoundType
{
    public ESounds ES_Sound;
    public AudioClip m_AudioClip;
}

public enum ESounds
{
    PlayerRun, PlayerHurt, PlayerDie,
    ChomperWalk, ChomperRun, ChomperAttack,
    MenuButtonClick, MenuButtonStart, MenuButtonSelectLevel,
    EnviornmentalAmbiance, BackgroundMusic, LevelComplete, LevelFailed,
    keyPickup
}
