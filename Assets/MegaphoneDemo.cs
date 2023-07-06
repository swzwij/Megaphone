using UnityEngine;
using UnityEngine.UI;

namespace Megaphone.Demo
{
    public class MegaphoneDemo : MonoBehaviour
    {
        [SerializeField]
        private Button Button;

        [SerializeField]
        private Button Button2;

        [SerializeField]
        private AudioSource audioSource;

        private void OnEnable()
        {
            Button.onClick.AddListener(Click);
            Button2.onClick.AddListener(Click2);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(Click);
            Button2.onClick.RemoveListener(Click2);
        }

        private void Click()
        {
            AudioClip clip = Megaphone.Instance.AudioClip("HORN");
            Megaphone.Instance.PlayAudio(audioSource, clip);
        }

        private void Click2()
        {
            AudioClip clip1 = Megaphone.Instance.AudioClip("HORN");
            AudioClip clip2 = Megaphone.Instance.AudioClip("PUNCH");
            AudioClip clip3 = Megaphone.Instance.AudioClip("ELEVATOR_MUSIC");

            Megaphone.Instance.QueueAudio(audioSource, clip1);
            Megaphone.Instance.QueueAudio(audioSource, clip2);
            Megaphone.Instance.QueueAudio(audioSource, clip3);
        }
    }
}