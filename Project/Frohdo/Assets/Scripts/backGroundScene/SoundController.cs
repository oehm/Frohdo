using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {

    private static SoundController instance = null;

    private List<AudioClip> Pukeclips;
    private List<AudioClip> Background_clips;

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
        loadAudioFiles();
    }

    private void loadAudioFiles()
    {
        Pukeclips.Add((AudioClip)Resources.Load("/Sounds/puke1.wav"));
    }

    public AudioClip getRandomPukeSound()
    {
        if (Pukeclips.Count > 0)
        {
            return Pukeclips[Random.Range(0,Pukeclips.Count)];
        }
        else
        {
            Debug.Log("No Puke Sound-files in list");
        }
        return null;
    }
}
