using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundListScriptable _soundListData;

    private AudioSource _myAudioSource;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this.gameObject);

        _myAudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySomeAudio(string clipName)
    {
        AudioClip clipToPlay = _soundListData.GetSomeClip(clipName).ClipList[Random.Range(0, _soundListData.GetSomeClip(clipName).ClipList.Count)]; ;

        if (clipToPlay)
            _myAudioSource.PlayOneShot(clipToPlay, _soundListData.GetSomeClip(clipName).Volume);
    }
}
