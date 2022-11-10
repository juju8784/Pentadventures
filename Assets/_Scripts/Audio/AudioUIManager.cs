using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUIManager : MonoBehaviour
{
    // This module is responsible for handling user Input for the Sound Manager.
    // Meaning it handles the User-Sound Manager interaction.
    // Most of these functions are called by buttons in the Audio Options UI.

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voicesSlider;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float voicesVolume;

    public void Start()
    {
        masterVolume = PlayerPrefs.GetFloat("Master");
        musicVolume = PlayerPrefs.GetFloat("Music");
        sfxVolume = PlayerPrefs.GetFloat("SFX");
        voicesVolume = PlayerPrefs.GetFloat("Voices");
        masterSlider.value = PlayerPrefs.GetFloat("Master");
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        voicesSlider.value = PlayerPrefs.GetFloat("Voices");
    }

    public void SaveChanges()
    {
        if (GameManager.instance.audioManager)
        {
            GameManager.instance.audioManager.SetMasterVolume(masterVolume);
            GameManager.instance.audioManager.SetMusicVolume(musicVolume);
            GameManager.instance.audioManager.SetSFXVolume(sfxVolume);
            GameManager.instance.audioManager.SetVoicesVolume(voicesVolume);
            GameManager.instance.audioManager.UpdatePlayerPrefs();

            GameManager.instance.music.UpdateNewVolumeToAudio();
        }
        else
        {
            Debug.Log("Audio Manager is not being set correclty. Null");
        }
    }

    public void MasterVolumeSlider()
    {
        GameManager.instance.audioManager.SetMasterVolume(masterVolume);
        GameManager.instance.music.UpdateNewVolumeToAudio(musicVolume, sfxVolume, masterVolume);
        masterVolume = masterSlider.value;
    }

    public void MusicVolumeSlider()
    {
        GameManager.instance.audioManager.SetMusicVolume(musicVolume);
        GameManager.instance.music.BackgroundMusic(musicVolume);
        musicVolume = musicSlider.value;
    }

    public void SFXVolumeSlider()
    {
        GameManager.instance.audioManager.SetSFXVolume(sfxVolume);
        GameManager.instance.music.SFXVolumeUpdate(sfxVolume);
        sfxVolume = sfxSlider.value;
    }

    public void VoicesVolumeSlider()
    {
        GameManager.instance.audioManager.SetVoicesVolume(voicesVolume);
        voicesVolume = voicesSlider.value;
    }

    public void CancelChanges()
    {
        GameManager.instance.audioManager.SetLocalVariablestoPlayerPrefs();

        masterSlider.value = PlayerPrefs.GetFloat("Master");
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        voicesSlider.value = PlayerPrefs.GetFloat("Voices");

        masterVolume = PlayerPrefs.GetFloat("Master");
        musicVolume = PlayerPrefs.GetFloat("Music");
        sfxVolume = PlayerPrefs.GetFloat("SFX");
        voicesVolume = PlayerPrefs.GetFloat("Voices");

        GameManager.instance.music.UpdateNewVolumeToAudio();
    }
}
