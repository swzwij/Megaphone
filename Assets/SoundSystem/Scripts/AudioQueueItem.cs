using UnityEngine;

namespace Megaphone
{
    public class AudioQueueItem
    {
        private Coroutine _playingAudio;
        private Coroutine _queue;

        public Coroutine Queue
        {
            get => _queue;
            set => _queue = value;
        }

        public Coroutine PlayingAudio
        {
            get => _playingAudio;
            set => _playingAudio = value;
        }
    }
}