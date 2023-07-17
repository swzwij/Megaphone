using UnityEditor;
using UnityEngine;

namespace Megaphone
{
    [CustomEditor(typeof(MegaphoneSpeaker))]
    public class MegaphoneSpreakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MegaphoneSpeaker megaphoneSpeaker = (MegaphoneSpeaker)target;

            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.ObjectField("Audio Clip", megaphoneSpeaker.Clip, typeof(AudioClip), false);

                EditorGUILayout.Space(10);

                EditorGUILayout.Toggle("Is playing", megaphoneSpeaker.IsPlaying);
                EditorGUILayout.Toggle("Is paused", megaphoneSpeaker.IsPaused);
                EditorGUILayout.Toggle("Is looping", megaphoneSpeaker.IsLooping);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}