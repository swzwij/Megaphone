using System;
using UnityEngine;

namespace Megaphone
{
    [Serializable]
    public struct AudioPreset
    {
        public string name;
        public int id;
        public AudioClip audioClip;
    }
}