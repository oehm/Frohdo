using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour
{

    private static SoundController instance = null;

    private List<AudioClip> Pukeclips;
    private List<AudioClip> Background_clips;

    private AudioSource _backgroundSource;
    private AudioSource _clickSoundsSource;

    bool _backloopstarted;

    public float MiscSoundVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("MiscVolume"))
                return PlayerPrefs.GetFloat("MiscVolume");
            else
                return 1;
        }
        set
        {
            PlayerPrefs.SetFloat("MiscVolume", value);
            _clickSoundsSource.volume = value/2;
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
        _backgroundSource = this.gameObject.GetComponents<AudioSource>()[0];
        _clickSoundsSource = this.gameObject.GetComponents<AudioSource>()[1];

        _backloopstarted = false;
        _backgroundSource.volume = BackgroundSoundVolume;

        _clickSoundsSource.volume = MiscSoundVolume;

        loadAudioFiles();
    }

    private void loadAudioFiles()
    {
        //Background Clips load
        Background_clips.Add((AudioClip)Resources.Load("Sounds/Backgrounds/Menue"));
        Background_clips.Add((AudioClip)Resources.Load("Sounds/Backgrounds/Menue2"));

        //PukeSounds load
        Pukeclips.Add((AudioClip)Resources.Load("Sounds/Pukes/puke1"));
        Pukeclips.Add((AudioClip)Resources.Load("Sounds/Pukes/puke2"));
        Pukeclips.Add((AudioClip)Resources.Load("Sounds/Pukes/puke3"));
        Pukeclips.Add((AudioClip)Resources.Load("Sounds/Pukes/puke4"));
        Pukeclips.Add((AudioClip)Resources.Load("Sounds/Pukes/puke5"));
        Pukeclips.Add((AudioClip)Resources.Load("Sounds/Pukes/puke6"));

        _clickSoundsSource.clip = (AudioClip)Resources.Load("Sounds/Misc/Click2");
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
            return Background_clips[Random.Range(0, Background_clips.Count)];
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
            return Pukeclips[Random.Range(0, Pukeclips.Count)];
        }
        else
        {
            Debug.Log("No Puke Sound-files in list");
        }
        return null;
    }

    public void playClickSound()
    {
        if (_clickSoundsSource.clip != null &&  _clickSoundsSource.isPlaying) _clickSoundsSource.Stop();
        //_clickSoundsSource.PlayOneShot(_clickSoundsSource.clip);
        _clickSoundsSource.Play();
    }

    void Update()
    {
        if (_backloopstarted && !_backgroundSource.isPlaying) //if soundloop is started once.. we will get a random background every time the old one is over. (when paused or stopped manually, nothing happens!
        {
            startBackgroundSoundLoop();
        }
    }
}
