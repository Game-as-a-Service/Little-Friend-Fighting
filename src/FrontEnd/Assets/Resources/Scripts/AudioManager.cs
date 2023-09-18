using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioLookup> audioLookups;

    public AudioClip GetAudioClip(string key) =>
        audioLookups.Find(x => x.Key == key).AudioClip;
}

[Serializable]
class AudioLookup
{
    public string Key;
    public AudioClip AudioClip;
}