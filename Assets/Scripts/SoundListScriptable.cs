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
        public List<AudioClip> ClipList = new List<AudioClip>();
    }

    [SerializeField] private List<AudioItem> _soundsList = new List<AudioItem>();

    public AudioClip GetSomeClip(string clipName)
    {
        AudioItem itemToGet = _soundsList.Where(sound => sound.Name == clipName).ToList().FirstOrDefault();

        if (itemToGet == null || itemToGet.ClipList.Count == 0)
            return null;

        return itemToGet.ClipList[Random.Range(0, itemToGet.ClipList.Count)];
    }
}
