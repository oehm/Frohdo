using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour
{

    private static SoundController instance = null;

    private List<AudioClip> Pukeclips;
    private List<AudioClip> Background_clips;

    public float PukeSoundVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("PukeVolume"))
                return PlayerPrefs.GetFloat("PukeVolume");
            else
                return 1;
        }
        set
        {
            PlayerPrefs.SetFloat("PukeVolume", value);
        }
    }

    public float BackgroundSoundVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("BackgroundVolume"))
                return PlayerPrefs.GetFloat("BackgroundVolume");
            else
                return 0.5f;
        }
        set
        {
            PlayerPrefs.SetFloat("BackgroundVolume", value);
            if (_backgroundSource != null)
                _backgroundSource.volume = value;
        }
    }

    private AudioSource _backgroundSource;

    bool _backloopstarted;

    public static SoundController Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        Pukeclips = new List<AudioClip>();
        Background_clips = new List<AudioClip>();
        _backgroundSource = this.gameObject.GetComponent<AudioSource>();
        _backloopstarted = false;
        _backgroundSource.volume = BackgroundSoundVolume;
        loadAudioFiles();
    }

    private void loadAudioFiles()
    {
        //Background Clips load
        Background_clips.Add((AudioClip)Resources.Load("Sounds/Backgrounds/Background1"));

        //PukeSounds load
        Pukeclips.Add((AudioClip)Resources.Load("Sounds/Pukes/puke1"));
    }

    public void startBackgroundSoundLoop()
    {
        _backloopstarted = true;
        if (Background_clips.Count > 0 && !_backgroundSource.isPlaying)
        {
            _backgroundSource.clip = getnewRandomBackgroundClip();
            if (_backgroundSource.clip != null) _backgroundSource.Play();
        }
    }

    private AudioClip getnewRandomBackgroundClip()
    {
        if (Background_clips.Count > 0)
        {
            return Background_clips[Random.Range(0, Background_clips.Count - 1)];
        }
        return null;
    }

    public void pauseBackgroundSoundLoop()
    {
        _backloopstarted = false;
        if (_backgroundSource != null && _backgroundSource.isPlaying)
        {
            _backgroundSource.Pause();
        }
    }

    public void resumeBackgroundSoundLoop()
    {
        _backloopstarted = true;
        if (_backgroundSource != null)
        {
            _backgroundSource.Play();
        }
    }

    public void stopBackgroundSoundLoop()
    {
        _backloopstarted = false;
        if (_backgroundSource != null && _backgroundSource.isPlaying)
        {
            _backgroundSource.Stop();
        }
    }

    public AudioClip getRandomPukeSound()
    {
        if (Pukeclips.Count > 0)
        {
            return Pukeclips[Random.Range(0, Pukeclips.Count - 1)];
        }
        else
        {
            Debug.Log("No Puke Sound-files in list");
        }
        return null;
    }

    void Update()
    {
        if (_backloopstarted && !_backgroundSource.isPlaying) //if soundloop is started once.. we will get a random background every time the old one is over. (when paused or stopped manually, nothing happens!
        {
            startBackgroundSoundLoop();
        }
    }
}
