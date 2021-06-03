using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

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

            float newVolume = Random.Range(-0.4f, 0.4f);
            float newPitch = Random.Range(-0.4f, 0.4f);

            sound.source.volume = sound.volume + newVolume;
            sound.source.pitch = sound.pitch + newPitch;

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
