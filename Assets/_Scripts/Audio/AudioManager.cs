using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundClip[] sounds;

    void Awake()
    {
        foreach(SoundClip s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.name = s.name;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }

    void Start()
    {
        PlayClip("MainSong");
    }

    public void PlayClip(string name)
    {
        SoundClip s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Error, '" + name + "' sound clip not found.");
            return;
        }
        s.source.Play();
    }

}
