using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;

    // Start is called before the first frame update
    private void Start()
    {
        if (!sfxSource)
        {
            sfxSource = this.GetComponent<AudioSource>();
        }
    }
    public void PlaySound()
    {
        sfxSource.Play();
        
    }
}
