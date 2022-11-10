using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInfomercials : MonoBehaviour
{
    public GameObject infomercials;
    public Toggle toggleui;
    bool toggle = true;

    public void Start()
    {
        toggle = (PlayerPrefs.GetInt("Infomercial", 1) == 1);
        if(toggleui)
        toggleui.isOn = toggle;
        if(infomercials)
        infomercials.SetActive(toggle);
    }

    public void Toggle()
    {
        toggle = toggleui.isOn;
    }


    public void Save()
    {
        PlayerPrefs.SetInt("Infomercial", (toggle) ? 1 : 0);
        if (infomercials)
            infomercials.SetActive(toggle);
    }

    public void Cancel()
    {
        toggle = (PlayerPrefs.GetInt("Infomercial", 1) == 1);
        if (infomercials)
            infomercials.SetActive(toggle);
    }
}
