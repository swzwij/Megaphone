using UnityEngine;
using UnityEngine.UI;

namespace Megaphone.Demo
{
    public class MegaphoneDemo : MonoBehaviour
    {
        [SerializeField]
        private Button _playAudioButton;

        [SerializeField]
        private Button _pauseAudioButton;

        [SerializeField]
        private Button _unpauseAudioButton;

        [SerializeField]
        private Button _stopAudioButton;

        [SerializeField]
        private Button _loopAudioButton;

        [SerializeField]
        private InputField _playAudioInputfield;

        [Space]

        [SerializeField]
        private Button Button2;

        [SerializeField]
        private MegaphoneSpeaker audioSource;

        private string _audioPresetString;

        private void OnEnable()
        {
            _playAudioButton.onClick.AddListener(PlayAudio);
            _pauseAudioButton.onClick.AddListener(PauseAudio);
            _unpauseAudioButton.onClick.AddListener(UnpauseAudio);
            _stopAudioButton.onClick.AddListener(StopAudio);
            _loopAudioButton.onClick.AddListener(LoopAudio);

            Button2.onClick.AddListener(Click2);

            _playAudioInputfield.onEndEdit.AddListener(SetAudioFile);
        }

        private void OnDisable()
        {
            _playAudioButton.onClick.RemoveListener(PlayAudio);
           Button2.onClick.RemoveListener(Click2);
        }

        private void PlayAudio()
        {
            AudioClip clip = Megaphone.Instance.AudioClip(_audioPresetString);
            Megaphone.Instance.Play(audioSource, clip);
        }

        private void PauseAudio()
        {
            Megaphone.Instance.Pause(audioSource);
        }

        private void UnpauseAudio()
        {
            Megaphone.Instance.Play(audioSource);
        }

        private void StopAudio()
        {
            Megaphone.Instance.Stop(audioSource);
        }

        private void LoopAudio()
        {
            Megaphone.Instance.Loop(audioSource, Megaphone.Instance.AudioClip(_audioPresetString));
        }

        private void SetAudioFile(string value)
        {
            _audioPresetString = value;
        }

        private void Click2()
        {
            AudioClip clip1 = Megaphone.Instance.AudioClip("HORN");
            AudioClip clip2 = Megaphone.Instance.AudioClip("PUNCH");
            AudioClip clip3 = Megaphone.Instance.AudioClip("ELEVATOR_MUSIC");

            //Megaphone.Instance.QueueClip(audioSource, clip1);
            //Megaphone.Instance.QueueClip(audioSource, clip2);
            //Megaphone.Instance.QueueClip(audioSource, clip3);
        }
    }
}