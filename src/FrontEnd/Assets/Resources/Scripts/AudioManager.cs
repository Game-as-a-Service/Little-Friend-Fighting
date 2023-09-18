using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip;
    private bool _looping;
    private Action _onMusicCompleteCallback;
    
    public AudioClip GetAudioClip(string key)
    {
        // Key Value => Key AudioClip
        return audioClip;
    }
}