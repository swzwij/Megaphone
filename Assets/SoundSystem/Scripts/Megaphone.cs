using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megaphone
{
    [RequireComponent(typeof(MegaphoneLibrary))]
    public class Megaphone : MonoBehaviour
    {
        #region Singleton Behaviour

        /// <summary>
        /// Megaphone instance.
        /// </summary>
        private static Megaphone _instance;

        /// <summary>
        /// Getting the Megaphone instance.
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

        public AudioClip AudioClip(string presetName) => _library.GetAudioPreset(presetName).audioClip;

        private void Awake()
        {
            _library = GetComponent<MegaphoneLibrary>();
        }

        public void Play(MegaphoneSpeaker megaphoneSpeaker, AudioClip audioClip)
        {
            if (megaphoneSpeaker.IsPlaying)
                megaphoneSpeaker.Stop();

            megaphoneSpeaker.Clip = audioClip;
            megaphoneSpeaker.Play();
        }

        public void Stop(MegaphoneSpeaker megaphoneSpeaker)
        {
            if (!megaphoneSpeaker.IsPlaying)
            {
                Debug.LogWarning($"{megaphoneSpeaker} is not playing a audio clip so it cannot be stopped.");
                return;
            }

            megaphoneSpeaker.Stop();
            megaphoneSpeaker.Clip = null;
        }

        public void Loop(MegaphoneSpeaker megaphoneSpeaker, AudioClip audioClip) 
        {
            megaphoneSpeaker.Clip = audioClip;
            megaphoneSpeaker.IsLooping = true;
            megaphoneSpeaker.Play();
        }

        public void Pause(MegaphoneSpeaker megaphoneSpeaker)
        {
            if (!megaphoneSpeaker.IsPlaying)
            {
                Debug.LogWarning($"{megaphoneSpeaker} is not playing a audio clip so it cannot be paused.");
                return;
            }

            megaphoneSpeaker.Pause();
        }

        public void Play(MegaphoneSpeaker megaphoneSpeaker)
        {
            if (megaphoneSpeaker.IsPlaying)
            {
                Debug.LogWarning($"{megaphoneSpeaker} is already playing a audio clip so it cannot be unpaused.");
                return;
            }

            megaphoneSpeaker.Play();
        }

        public void PauseForSeconds(MegaphoneSpeaker megaphoneSpeaker, float timeSeconds)
        {
            if (!megaphoneSpeaker.IsPlaying)
            {
                Debug.LogWarning($"{megaphoneSpeaker} is not playing a audio clip so it cannot be paused.");
                return;
            }

            StartCoroutine(PauseAudio(megaphoneSpeaker.AudioSource, timeSeconds));
        }

        public void PlayPreset(MegaphoneSpeaker megaphoneSpeaker, string audioPreset)
        {
            if (megaphoneSpeaker.IsPlaying)
                megaphoneSpeaker.Stop();

            megaphoneSpeaker.Clip = _library.GetAudioPreset(audioPreset).audioClip;
            megaphoneSpeaker.Play();
        }

        public void PlayPresetId(MegaphoneSpeaker megaphoneSpeaker, int id)
        {
            if (megaphoneSpeaker.IsPlaying)
                megaphoneSpeaker.Stop();

            megaphoneSpeaker.Clip = _library.GetSoundPresetById(id).audioClip;
            megaphoneSpeaker.Play();
        }

        private IEnumerator PauseAudio(AudioSource audioSource, float timeSeconds)
        {
            audioSource.Pause();
            yield return new WaitForSeconds(timeSeconds);
            audioSource.UnPause();
        }

        /*#region Queue

        private Dictionary<MegaphoneSpeaker, List<AudioClip>> _audioQueueClips = new();

        private Dictionary<MegaphoneSpeaker, AudioQueueItem> _audioQueues = new();

        public void QueueClip(MegaphoneSpeaker megaphoneSpeaker, AudioClip audioClip)
        {
            if (megaphoneSpeaker.IsPlaying && _audioQueues[megaphoneSpeaker].Queue == null)
                megaphoneSpeaker.Stop();

            _audioQueueClips.TryAdd(megaphoneSpeaker, new());
            _audioQueueClips[megaphoneSpeaker].Add(audioClip);

            _audioQueues[megaphoneSpeaker] = new AudioQueueItem { Queue = StartCoroutine(StartAudioQueue(megaphoneSpeaker)) };
        }

        public void QueuePreset(MegaphoneSpeaker megaphoneSpeaker, string audioPreset)
            => QueueClip(megaphoneSpeaker, _library.GetAudioPreset(audioPreset).audioClip);


        public void QueueClipList(MegaphoneSpeaker megaphoneSpeaker, List<AudioClip> audioClips)
        {
            for (int i = 0; i < audioClips.Count; i++)
                QueueClip(megaphoneSpeaker, audioClips[i]);
        }

        public void Skip(MegaphoneSpeaker megaphoneSpeaker)
        {
            if (_audioQueues[megaphoneSpeaker].PlayingAudio == null)
            {
                Debug.LogWarning($"{megaphoneSpeaker} there is currently nothing to be skipped.");
                return;
            }

            StopCoroutine(_audioQueues[megaphoneSpeaker].PlayingAudio);
            _audioQueues[megaphoneSpeaker].PlayingAudio = null;
        }

        public void QueuePause(MegaphoneSpeaker megaphoneSpeaker)
        {
            //TODO:
        }

        public void QueuePlay(MegaphoneSpeaker megaphoneSpeaker)
        {
            //TODO:
        }

        private IEnumerator StartAudioQueue(MegaphoneSpeaker megaphoneSpeaker)
        {
            yield return new WaitUntil(() => _audioQueues.ContainsKey(megaphoneSpeaker));

            while (_audioQueueClips[megaphoneSpeaker].Count > 0)
            {
                _audioQueues[megaphoneSpeaker].PlayingAudio = StartCoroutine(PlayAudioQeueItem(megaphoneSpeaker, _audioQueueClips[megaphoneSpeaker][0]));
                yield return new WaitUntil(() => _audioQueues[megaphoneSpeaker].PlayingAudio == null);
                _audioQueueClips[megaphoneSpeaker].RemoveAt(0);
            }

            _audioQueues[megaphoneSpeaker].Queue = null;
        }

        private IEnumerator PlayAudioQeueItem(MegaphoneSpeaker megaphoneSpeaker, AudioClip audioClip)
        {
            megaphoneSpeaker.Clip = audioClip;
            megaphoneSpeaker.Play();

            bool isFinished = false;

            Action onFinish = () =>
            {
                isFinished = true;
            };

            megaphoneSpeaker.onFinish += onFinish;

            yield return new WaitUntil(() => isFinished == true);

            megaphoneSpeaker.onFinish -= onFinish;

            _audioQueues[megaphoneSpeaker].PlayingAudio = null;
        }
        #endregion*/
    }
}