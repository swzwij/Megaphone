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

        private Dictionary<AudioSource, List<AudioClip>> _audioQueueClips = new();

        private Dictionary<AudioSource, AudioQueueItem> _audioQueues = new();

        public AudioClip AudioClip(string presetName) => _library.GetAudioPreset(presetName).audioClip;

        private void Awake()
        {
            _library = GetComponent<MegaphoneLibrary>();
        }

        public void PlayAudio(AudioSource audioSource, AudioClip audioClip)
        {
            if (audioSource.isPlaying)
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
            if (!audioSource.isPlaying)
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

            audioSource.clip = _library.GetAudioPreset(audioPreset).audioClip;
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
            if (audioSource.isPlaying)
                audioSource.Stop();

            _audioQueueClips.TryAdd(audioSource, new());
            _audioQueueClips[audioSource].Add(audioClip);

            _audioQueues[audioSource] = new AudioQueueItem { Queue = StartCoroutine(StartAudioQueue(audioSource)) };
        }

        public void QueueAudioPreset(AudioSource audioSource, string audioPreset)
            => QueueAudio(audioSource, _library.GetAudioPreset(audioPreset).audioClip);

        public void QueueSkip(AudioSource audioSource)
        {
            if (_audioQueues[audioSource].PlayingAudio == null)
            {
                Debug.LogWarning($"{audioSource} there is currently nothing to be skipped.");
                return;
            }

            StopCoroutine(_audioQueues[audioSource].PlayingAudio);
            _audioQueues[audioSource].PlayingAudio = null;
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
            yield return new WaitUntil(() => _audioQueues.ContainsKey(audioSource));

            while (_audioQueueClips[audioSource].Count > 0)
            {
                _audioQueues[audioSource].PlayingAudio = StartCoroutine(PlayAudioQeueItem(audioSource, _audioQueueClips[audioSource][0]));
                yield return new WaitUntil(() => _audioQueues[audioSource].PlayingAudio == null);
                _audioQueueClips[audioSource].RemoveAt(0);
            }

            _audioQueues[audioSource].Queue = null;
        }

        private IEnumerator PlayAudioQeueItem(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();

            yield return new WaitForSeconds(audioClip.length);

            _audioQueues[audioSource].PlayingAudio = null;
        }
    }
}