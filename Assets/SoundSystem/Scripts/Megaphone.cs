using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MegaphoneLibrary))]
public class Megaphone : MonoBehaviour
{
    #region Singleton Behaviour

    /// <summary>
    /// API Manager instance.
    /// </summary>
    private static Megaphone _instance;

    /// <summary>
    /// Getting the API manager instance.
    /// </summary>
    public static Megaphone Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<Megaphone>();

            if (_instance != null)
                return _instance;

            GameObject singletonObject = new();
            _instance = singletonObject.AddComponent<Megaphone>();
            singletonObject.name = typeof(Megaphone).ToString();
            DontDestroyOnLoad(singletonObject);

            return _instance;
        }
    }

    #endregion

    private MegaphoneLibrary _library;

    private Dictionary<AudioSource, List<AudioClip>> _audioQueueClips = new();
    private Dictionary<AudioSource, Coroutine> _currentlyPlaying = new();

    private void Awake()
    {
        _library = GetComponent<MegaphoneLibrary>();
    }

    public void PlayAudio(AudioSource audioSource, AudioClip audioClip)
    {
        if(audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void StopAudio(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
            return;

        audioSource.Stop();
    }

    public void PauseAudioForSeconds(AudioSource audioSource, float timeSeconds)
    {
        if(!audioSource.isPlaying)
        {
            Debug.LogWarning($"{audioSource} is not playing a audio clip so it cannot be paused.");
            return;
        }

        StartCoroutine(PauseAudio(audioSource, timeSeconds));
    }

    public void PlayAudioPreset(AudioSource audioSource, string audioPreset)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = _library.GetSoundPreset(audioPreset).audioClip;
        audioSource.Play();
    }

    public void PlayAudioPresetId(AudioSource audioSource, int id)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = _library.GetSoundPresetById(id).audioClip;
        audioSource.Play();
    }

    public void QueueAudio(AudioSource audioSource, AudioClip audioClip) 
    {
        
    }

    public void QueueAudioPreset(AudioSource audioSource, string audioPreset) 
    {
        
    }

    public void QueueSkip(AudioSource audioSource)
    {

    }

    public void QueuePause(AudioSource audioSource) 
    {
        
    }

    public void QueuePlay(AudioSource audioSource)
    {

    }

    private IEnumerator PauseAudio(AudioSource audioSource, float timeSeconds)
    {
        audioSource.Pause();
        yield return new WaitForSeconds(timeSeconds);
        audioSource.UnPause();
    }

    private IEnumerator StartAudioQueue(AudioSource audioSource)
    {
        for (int i = 0; i < Mathf.Infinity; i++)
        {
            if (_currentlyPlaying.ContainsKey(audioSource))
                _currentlyPlaying[audioSource] = null;
            else
                _currentlyPlaying.Add(audioSource, null);

            _currentlyPlaying[audioSource] = StartCoroutine(PlayAudioQeueItem(audioSource, _audioQueueClips[audioSource][i]));

            yield return new WaitUntil(() => _currentlyPlaying[audioSource] == null);

            if (_audioQueueClips[audioSource].Count - 1 <= i)
                yield break;
        }
    }

    private IEnumerator PlayAudioQeueItem(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();

        yield return new WaitForSeconds(audioClip.length);

        _currentlyPlaying[audioSource] = null;
    }
}
