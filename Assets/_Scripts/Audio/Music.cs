using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public float masterVolume;
    public GameObject[] sfx;
    private void Start()
    {
        masterVolume = PlayerPrefs.GetFloat("Master");

        if (!backgroundMusic)
        {
            backgroundMusic = GameObject.Find("backgroundMusic").GetComponent<AudioSource>();
        }
        backgroundMusic.volume = (PlayerPrefs.GetFloat("Music") * masterVolume);
        //sfxExample.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume");


        if (sfx == null)
        {
            sfx = GameObject.FindGameObjectsWithTag("SFX");
        }
        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].GetComponent<AudioSource>().volume = (PlayerPrefs.GetFloat("SFX") * masterVolume);
        }
    }

    public void UpdateNewVolumeToAudio()
    {
        masterVolume = PlayerPrefs.GetFloat("Master");
        BackgroundMusic(PlayerPrefs.GetFloat("Music"));
        SFXVolumeUpdate(PlayerPrefs.GetFloat("SFX"));
    }

    public void UpdateNewVolumeToAudio(float BGvolume, float SFXvolume, float Mastervolume)
    {
        masterVolume = Mastervolume;
        BackgroundMusic(BGvolume);
        SFXVolumeUpdate(SFXvolume);
    }

    public void BackgroundMusic(float volume)
    {
        backgroundMusic.volume = (volume * masterVolume);
    }

    public void SFXVolumeUpdate(float volume)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].GetComponent<AudioSource>().volume = (volume * masterVolume);
    }
    }
}
