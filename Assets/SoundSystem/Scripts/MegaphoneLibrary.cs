using System;
using System.Collections.Generic;
using UnityEngine;
using static MegaphoneLibrary;

public class MegaphoneLibrary : MonoBehaviour
{
    [Serializable]
    public struct SoundPreset
    {
        public string name;
        public int id;
        public AudioClip audioClip;
    }

    [SerializeField]
    private SoundPreset[] _soundPresets;

    private readonly Dictionary<string, SoundPreset> _soundLibrary = new();

    private void Awake()
    {
        for (int i = 0; i < _soundPresets.Length; i++)
        {
            SoundPreset soundPreset = _soundPresets[i];
            soundPreset.id = i;
            _soundLibrary.Add(soundPreset.name, soundPreset);
        }
    }

    public SoundPreset GetSoundPreset(string presetName)
    {
        if (_soundLibrary.ContainsKey(presetName))
            return _soundLibrary[presetName];

        Debug.LogError($"Megaphone library does not contain a preset named '{presetName}'");
        return new();
    }

    public SoundPreset GetSoundPresetById(int id)
    {
        foreach (KeyValuePair<string, SoundPreset> keyValuePair in _soundLibrary)
        {
            SoundPreset soundPreset = keyValuePair.Value;
            if(soundPreset.id == id)
                return soundPreset;
        }

        Debug.LogError($"Megaphone library does not contain a preset with the given id: '{id}'");
        return new();
    }
}
