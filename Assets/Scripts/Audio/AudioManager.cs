using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private object s;

    public static AudioManager instance;

    public static AudioManager GetInstance()
    {
        return instance;
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Dark_Forest" || sceneName == "DarkForestP2")
        {
            Play("Crickets");
            Play("BGM - Dark Forest");
        }
        else if (sceneName == "Melee_Testing")
        {
            Play("BGM - Before Dark Forest");
        }
        else if (sceneName == "Menu")
        {
            Play("BGM - Before Dark Forest");
        }
        else if (sceneName == "Village")
        {
            Play("RoaringFire");
            Play("BGM - Village");
        }
        else if (sceneName == "Village_Home")
        {
            Play("RoaringFire");
            Play("BGM - Village Home");
        }
        else if (sceneName == "Boss1_Testing")
        {
            Play("Crickets");
            Play("Boss Wizard Theme");
        }
        else if (sceneName == "Boss3_Testing")
        {
            Play("General Crain Boss Theme");
        }
        else if (sceneName == "Castle Level")
        {
            Play("BGM - Castle");
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: \"" + name + "\" not found!");
            return;
        }

        s.source.Play();
    }

    public void StopPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volume / 2f, s.volume / 2f));
        //s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitch / 2f, s.pitch / 2f));

        s.source.Stop();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }

        if (s.source.isPlaying)
            return true;
        else
            return false;
    }

    // Used to distort the pitch of all sounds. Used mainly in TriggerDeathCutscene.cs
    public void Distort(float amount)
    {
        // Distort all game sounds except the Retry button and DeathBell sounds.
        foreach (Sound s in sounds.Skip(2))
        {
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            // Decrease the last two parameters to increase the time it takes to reach the target pitch/volume. Decrease to decrease the time it takes.
            StartCoroutine(decreasingPitchAndSound(s, s.source.pitch, s.source.pitch + amount, 10f, 150f));
        }
    }

    IEnumerator decreasingPitchAndSound(Sound s, float start, float end, float durationPitch, float durationVolume)
    {
        float percentPitch = 0;
        float percentVolume = 0;
        float timeFactorPitch = 1 / durationPitch;
        float timeFactorVolume = 1 / durationVolume;
        while (percentPitch < 1)
        {
            percentPitch += Time.deltaTime * timeFactorPitch;
            s.source.pitch = Mathf.Lerp(start, end, Mathf.SmoothStep(0, 1, percentPitch));

            percentVolume += Time.deltaTime * timeFactorVolume;
            s.source.volume = Mathf.Lerp(s.source.volume, 0, Mathf.SmoothStep(0, 1, percentVolume));
            yield return null;
        }
    }
}
