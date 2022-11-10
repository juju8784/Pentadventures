using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float voicesVolume;
    [SerializeField] GameObject music;

    public void EraseThisMusic()
    {
        music = GameObject.FindGameObjectWithTag("Music");
        if (music)
        {
            music.GetComponent<AudioSource>().mute = true;
        }
    }

    public void PlayItNow()
    {
        music = GameObject.FindGameObjectWithTag("Music");
        if (music)
        {
            music.GetComponent<AudioSource>().mute = false;
        }
    }

    private void Start()
    {
        SetLocalVariablestoPlayerPrefs();
    }
    public void SetLocalVariablestoPlayerPrefs()
    {
        masterVolume = PlayerPrefs.GetFloat("Master");
        musicVolume = PlayerPrefs.GetFloat("Music");
        sfxVolume = PlayerPrefs.GetFloat("SFX");
        voicesVolume = PlayerPrefs.GetFloat("Voices");
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetVoicesVolume(float volume)
    {
        voicesVolume = volume;
    }

    public float GetVoicesVolume()
    {
        return voicesVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetFloat("Master", masterVolume);
        PlayerPrefs.SetFloat("Music", musicVolume);
        PlayerPrefs.SetFloat("SFX", sfxVolume);
        PlayerPrefs.SetFloat("Voices", voicesVolume);
    }
}
