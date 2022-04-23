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
    public void PlayReversedSound(string name)
    {
        Sound d = Array.Find(sounds, sound => sound.name == name);
        if (d == null)
            return;

        d.source.pitch = -1;
        d.source.timeSamples = d.clip.samples - 1;
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
    public bool IsPlayingSound(string name)
    {
        Sound d = Array.Find(sounds, sound => sound.name == name);
        return d.source.isPlaying;
    }


    public static void Play(string name)
    {
        try
        {
            if(Instance.IsSecretScene())
            {
                Instance.PlayReversedSound(name);
            }
            else
            {
                Instance.PlaySound(name);
            }

        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Entering game from level scene will lead to loss of Audio and is not reccomended!");
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
            Debug.LogWarning("Entering game from level scene will lead to loss of Audio and is not reccomended!");
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
            Debug.LogWarning("Entering game from level scene will lead to loss of Audio and is not reccomended!");
        }
    }

    public static bool IsPlaying(string name)
    {
        try
        {
            return Instance.IsPlayingSound(name);
        }
        catch(NullReferenceException)
        {
            Debug.LogWarning("Entering game from level scene will lead to loss of Audio and is not reccomended!");
        }
        return false;
    }

    public bool IsSecretScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelSecret";
    }
}
