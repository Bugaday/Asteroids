using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Dictionary<string,Sound> ClipDict = new Dictionary<string,Sound>();

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.playOnAwake = false;

            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            ClipDict.Add(sound.name, sound);
        }

    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = ClipDict[name];
        s.source.PlayOneShot(s.source.clip);
    }
}
