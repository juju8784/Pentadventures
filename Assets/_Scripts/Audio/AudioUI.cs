using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider uisfxSlider;
    [SerializeField] Slider voicesSlider;
    [SerializeField] GameObject surePrompt;
    bool saved = true;
    float multiplier = 20.0f;
    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(MasterChanged);
        musicSlider.onValueChanged.AddListener(MusicChanged);
        sfxSlider.onValueChanged.AddListener(SFXChanged);
        uisfxSlider.onValueChanged.AddListener(UISFXChanged);
        voicesSlider.onValueChanged.AddListener(VoicesChanged);
        saved = true;
    }

    private void Start()
    {
        SetMixerVolumeToPlayerPrefs();
    }

    public void UnsavedChangesPrompt(GameObject menu)
    {
        if (!saved)
        {
            surePrompt.SetActive(true);
        }
        else
        {
            surePrompt.SetActive(false);
            menu.SetActive(false);
        }
    }

    public void CancelChanges()
    {
        SetMixerVolumeToPlayerPrefs();
        saved = true;
    }

    public void SaveChanges()
    {
        PlayerPrefs.SetFloat("Master", masterSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("UISFX", uisfxSlider.value);
        PlayerPrefs.SetFloat("Voices", voicesSlider.value);
        SetMixerVolumeToPlayerPrefs();
        saved = true;
    }
    
    private void MasterChanged(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * multiplier);
        saved = false;
    }

    private void MusicChanged(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * multiplier);
        saved = false;
    }

    private void SFXChanged(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * multiplier);
        saved = false;
    }

    private void UISFXChanged(float value)
    {
        mixer.SetFloat("UISFXVolume", Mathf.Log10(value) * multiplier);
        saved = false;
    }

    private void VoicesChanged(float value)
    {
        mixer.SetFloat("VoicesVolume", Mathf.Log10(value) * multiplier);
        saved = false;
    }

    private void SetMixerVolumeToPlayerPrefs()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master", 1);
        MasterChanged(masterSlider.value);

        musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
        MusicChanged(musicSlider.value);

        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1);
        SFXChanged(sfxSlider.value);

        uisfxSlider.value = PlayerPrefs.GetFloat("UISFX", 1);
        UISFXChanged(uisfxSlider.value);

        voicesSlider.value = PlayerPrefs.GetFloat("Voices", 1);
        VoicesChanged(voicesSlider.value);
    }


}
