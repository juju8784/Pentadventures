using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildHall : MonoBehaviour
{
    [SerializeField] AudioSource audio;

    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Music");
        if (temp)
        {
            audio = temp.GetComponent<AudioSource>();
            if (audio)
            {
                audio.mute = false;
            }
        }
    }


}
