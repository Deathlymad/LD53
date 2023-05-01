using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioBackpack : MonoBehaviour
{
    public AudioClip Open;
    public AudioClip Close;
    public AudioSource aud;
    public bool BackpackState;
    public void PlayBackpack()
    {
        if (BackpackState)
        {
            aud.clip = Open;
            aud.Play();
            BackpackState = false;
        }
        else
        {
            aud.clip = Close;
            aud.Play();
            BackpackState = true;
        }
    }
}
