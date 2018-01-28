using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/Sound Data")]
public class SoundListScriptable : ScriptableObject {

    [System.Serializable]
    public class AudioItem
    {
        public string Name;
        public AudioClip Clip;
    }

    [SerializeField] private List<AudioItem> _soundsList = new List<AudioItem>();

    public AudioClip GetSomeClip(string clipName)
    {
        return _soundsList.Where(sound => sound.Name == clipName).ToList().FirstOrDefault().Clip;
    }
}
