using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip;
    public bool IsPlaying => _audioSource.isPlaying;
    private AudioSource _audioSource;
    private bool _looping;
    private Action _onMusicCompleteCallback;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        _audioSource.clip = audioClip;
    }

    public void PlaySoundFromTo(
        float startTime,
        float endTime,
        Action onMusicCompleteCallback = null)
    {
        _onMusicCompleteCallback = onMusicCompleteCallback;
        _audioSource.time = startTime;
        _audioSource.Play();
        StartCoroutine(StopAfterTime(endTime - startTime));
    }
    
    public void PlayLoopSoundFromTo(float startTime, float endTime)
    {
        _looping = true;
        _audioSource.time = startTime;
        _audioSource.Play();
        
        _audioSource.loop = false;
        _audioSource.PlayScheduled(AudioSettings.dspTime + (endTime - startTime));
        StartCoroutine(StartLooping(startTime, endTime));
    }
    public void StopLoopSound()
    {
        _looping = false;
        _audioSource.Stop();
    }

    private IEnumerator StopAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        _audioSource.Stop();
        _onMusicCompleteCallback?.Invoke();
    }

    private IEnumerator StartLooping(float startTime, float endTime)
    {
        while (_looping)
        {
            yield return new WaitUntil(() => _audioSource.time >= endTime);
            _audioSource.time = startTime;
            _audioSource.Play();
        }
    }
}