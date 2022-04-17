using System;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            //s.source.volume = s.volume;
        }
    }


    void Update()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = (float)PlayerPrefs.GetFloat("Volume");
        }
    }
    public void PlaySound(string name)
    {
        Sound d = Array.Find(sounds, sound => sound.name == name);
        if (d == null)
            return;
        d.source.Play();
    }
    public void StopSound(string name)
    {
        Sound d = Array.Find(sounds, sound => sound.name == name);
        if (d == null)
            return;
        d.source.Stop();
    }
    public void StopAllSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }


    public static void Play(string name)
    {
        try
        {
            Instance.PlaySound(name);
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Entering game from level scene can lead to loss of Audio and is not reccomended!");
        }

    }
    public static void Stop(string name)
    {
        try
        {
            Instance.StopSound(name);
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Entering game from level scene can lead to loss of Audio and is not reccomended!");
        }
    }
    public static void StopAll()
    {
        try
        {
            Instance.StopAllSounds();
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Entering game from level scene can lead to loss of Audio and is not reccomended!");
        }
    }
}
