using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ControlsManager : MonoBehaviour
{
    public static KeyCode Forward = KeyCode.W;
    public static KeyCode Backwards = KeyCode.S;
    public static KeyCode Left = KeyCode.A;
    public static KeyCode Right = KeyCode.D;
    public static KeyCode Escape = KeyCode.Escape;
    public static KeyCode Quest = KeyCode.Q;
    public static KeyCode Party = KeyCode.Space;
    public static KeyCode SnapCam = KeyCode.V;
    public static KeyCode Goal = KeyCode.G;
    public static KeyCode Inventory = KeyCode.I;
    public  TextMeshProUGUI txt;

    public void Awake()
    {
        LoadLayout();
    }
    public void Keybinds(int key)
    {
        if(rebindRoutine != null)
                return;
            else
            {
                rebindRoutine = Rebinder(key);
                StartCoroutine(rebindRoutine);
            }
    }

    IEnumerator rebindRoutine;

     KeyCode lastPressedKey = KeyCode.None;

    public IEnumerator Rebinder(int key)
    {
        Debug.Log("Listening for key");
        lastPressedKey = KeyCode.None;
        txt.GetComponent<TextMeshProUGUI>().text = "_";
        while(lastPressedKey == KeyCode.None)
        {
            foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode))
                {
                    Debug.Log("KeyCode down: " + kcode);
                    lastPressedKey = kcode;
                    txt.GetComponent<TextMeshProUGUI>().text = kcode.ToString();
                    
                }
            }
            yield return new WaitForEndOfFrame();
        }

        switch(key)
        {
            case 1:
            Forward = lastPressedKey;
            break;
            case 2: 
            Backwards = lastPressedKey;
            break;
            case 3:
            Left = lastPressedKey;
            break;
            case 4: 
            Right = lastPressedKey;
            break;
            case 5:
            Escape = lastPressedKey;
            break;
            case 6:
            Quest = lastPressedKey;
            break;
            case 7:
            Party = lastPressedKey;
            break;
            case 8:
            SnapCam = lastPressedKey;
            break;
            case 9:
            Goal = lastPressedKey;
            break;
            case 10:
            Inventory = lastPressedKey;
            break;
        }
        rebindRoutine = null;
        Debug.Log("corutine ended");
        Debug.Log(Party);
    }

    public void SaveLayout()
    {
        PlayerPrefs.SetString("Forawrd",Forward.ToString());
        PlayerPrefs.SetString("Backwards",Backwards.ToString());
        PlayerPrefs.SetString("Left",Left.ToString());
        PlayerPrefs.SetString("Right",Right.ToString());
        PlayerPrefs.SetString("Escape",Escape.ToString());
        PlayerPrefs.SetString("Quest",Quest.ToString());
        PlayerPrefs.SetString("Party",Party.ToString());
        PlayerPrefs.SetString("SnapCam",SnapCam.ToString());
        PlayerPrefs.SetString("Goal",Goal.ToString());
        PlayerPrefs.SetString("Inventory",Inventory.ToString());
    }

    public void LoadLayout()
    {
        Forward = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Forawrd",KeyCode.W.ToString()));
        Backwards = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Backwards",KeyCode.S.ToString()));
        Left = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Left",KeyCode.A.ToString()));
        Right = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Right",KeyCode.D.ToString()));
        Escape = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Escape",KeyCode.Escape.ToString()));
        Quest = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Quest",KeyCode.Q.ToString()));
        Party = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Party",KeyCode.Space.ToString()));
        SnapCam = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("SnapCam",KeyCode.V.ToString()));
        Goal = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Goal",KeyCode.G.ToString()));
        Inventory = (KeyCode)Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Inventory",KeyCode.I.ToString()));

    }
}
