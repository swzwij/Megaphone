using System.Collections.Generic;
using UnityEngine;

namespace Megaphone
{
    public class MegaphoneLibrary : MonoBehaviour
    {
        [SerializeField]
        private AudioPreset[] _soundPresets;

        private readonly Dictionary<string, AudioPreset> _soundLibrary = new();

        private void Awake()
        {
            for (int i = 0; i < _soundPresets.Length; i++)
            {
                AudioPreset soundPreset = _soundPresets[i];
                soundPreset.id = i;
                _soundLibrary.Add(soundPreset.name, soundPreset);
            }
        }

        public AudioPreset GetAudioPreset(string presetName)
        {
            if (_soundLibrary.ContainsKey(presetName))
                return _soundLibrary[presetName];

            Debug.LogError($"Megaphone library does not contain a preset named '{presetName}'");
            return new();
        }

        public AudioPreset GetSoundPresetById(int id)
        {
            foreach (KeyValuePair<string, AudioPreset> keyValuePair in _soundLibrary)
            {
                AudioPreset soundPreset = keyValuePair.Value;
                if (soundPreset.id == id)
                    return soundPreset;
            }

            Debug.LogError($"Megaphone library does not contain a preset with the given id: '{id}'");
            return new();
        }
    }
}