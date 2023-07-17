using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Megaphone
{
    [RequireComponent(typeof(AudioSource))]
    public class MegaphoneSpeaker : MonoBehaviour
    {
        /// <summary>
        /// Reference to the audio source.
        /// </summary>
        private AudioSource _audioSource;

        /// <summary>
        /// Whether the audio source is playing.
        /// </summary>
        private bool _isLooping;

        /// <summary>
        /// Whether the audio source is paused.
        /// </summary>
        private bool _isPaused;

        /// <summary>
        /// The coroutine waiting for when the audio clip finishes.
        /// </summary>
        private Coroutine _waitForClipFinishCoroutine;

        /// <summary>
        /// Action for when the audio clip gets started.
        /// </summary>
        public Action<AudioClip> onStart;

        /// <summary>
        /// Action for when the audio clip finished.
        /// </summary>
        public Action onFinish;

        /// <summary>
        /// Action for when the audio clip is getting paused.
        /// </summary>
        public Action<AudioClip> onPause;

        /// <summary>
        /// Action for when the audio clip is getting resumed.
        /// </summary>
        public Action<AudioClip> onResume;

        /// <summary>
        /// Action for when the audio clip gets canceled.
        /// </summary>
        public Action onCancel;

        /// <summary>
        /// Whether the audio source is playing.
        /// </summary>
        public bool IsPlaying => AudioSource.isPlaying;

        /// <summary>
        /// Whether the audio source is paused.
        /// </summary>
        public bool IsPaused => _isPaused;

        /// <summary>
        /// Whether the audio source is looping.
        /// </summary>
        public bool IsLooping
        {
            get => _isLooping;
            set
            {
                _isLooping = value;
                AudioSource.loop = value;
            }
        }

        /// <summary>
        /// The audio source.
        /// </summary>
        public AudioSource AudioSource
        {
            get
            {
                if (_audioSource == null)
                    _audioSource = GetComponent<AudioSource>();

                return _audioSource;
            }
            private set => _audioSource = value;
        }

        /// <summary>
        /// The audio clip being used in the audio source.
        /// </summary>
        public AudioClip Clip
        {
            get => AudioSource.clip;
            set => AudioSource.clip = value;
        }

        /// <summary>
        /// Set the audio source reference.
        /// </summary>
        private void Awake()
        {
            if( _audioSource == null )
                _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            onStart += OnStart;
            onFinish += OnFinish;
            onPause += OnPause;
            onResume += OnResume;
            onCancel += OnCancel;
        }

        private void OnDisable()
        {
            onStart -= OnStart;
            onFinish -= OnFinish;
            onPause -= OnPause;
            onResume -= OnResume;
            onCancel -= OnCancel;
        }

        /// <summary>
        /// Play the audio clip.
        /// </summary>
        public void Play()
        {
            if (_waitForClipFinishCoroutine != null)
            {
                StopCoroutine(_waitForClipFinishCoroutine);
                _waitForClipFinishCoroutine = null;
                onCancel?.Invoke();
            }

            if (_isPaused)
                onResume?.Invoke(AudioSource.clip);
            else
                onStart?.Invoke(AudioSource.clip);

            AudioSource.Play();
            _isPaused = false;

            _waitForClipFinishCoroutine = StartCoroutine(WaitForClipFinish());
        }

        /// <summary>
        /// Stop the audio source from playing.
        /// </summary>
        public void Stop()
        {
            AudioSource.Stop();
        }

        /// <summary>
        /// Pause the audio source.
        /// </summary>
        public void Pause()
        {
            onPause?.Invoke(AudioSource.clip);
            AudioSource.Pause();
        }

        /// <summary>
        /// Waiting for the audio source to stop playing.
        /// </summary>
        private IEnumerator WaitForClipFinish()
        {
            while(AudioSource.isPlaying || _isPaused)
                yield return null;

            onFinish?.Invoke();
        }

        private void OnStart(AudioClip _)
        {

        }

        private void OnFinish()
        {
            
        }

        private void OnPause(AudioClip _)
        {
            _isPaused = true;
        }

        private void OnResume(AudioClip _)
        {

        }

        private void OnCancel()
        {

        }
    }
}