using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System;

public class ScoreController : MonoBehaviour {

    private static ScoreController instance = null;
    public static ScoreController Instance
    {
        get
        {
            return instance;
        }
    }
    public bool isRunning{ get; set; }

    public string hash { get; set; }
    public int pukeCount { get; private set; }
    public int timeCount { get; private set; }

    public LevelAndType curLevel;

	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            timeCount += (int)(Time.deltaTime * 1000.0f);
        }
	}

    public void reset(LevelAndType newLevel, bool saveHighscoreForthisLevel = true)
    {
        isRunning = false;
        pukeCount = 0;
        timeCount = 0;
        if (newLevel.type == LevelLoader.LevelType.Story) hash = newLevel.LevelTitle + "_score";
        else hash = getMD5ofFile(newLevel.LeveltoLoad);
    }

    public void saveLocalTimeHighScore(string hash)
    {
        if(getlocalTimeHighscore(hash) == -1 || timeCount < getlocalTimeHighscore(hash))
        PlayerPrefs.SetInt("highscores_" + hash + "_Time", timeCount);
    }
    public void saveLocalPukeHighscore(string hash)
    {
        if (getlocalPukeHighscore(hash) == -1 || pukeCount < getlocalPukeHighscore(hash))
        PlayerPrefs.SetInt("highscores_" + hash + "_Pukes", pukeCount);
    }
    public int getlocalTimeHighscore(string hash)
    {
        if (PlayerPrefs.HasKey("highscores_" + hash + "_Time"))
        {
            return PlayerPrefs.GetInt("highscores_" + hash + "_Time");
        }
        return -1;
    }
    public int getlocalPukeHighscore(string hash)
    {
        if (PlayerPrefs.HasKey("highscores_" + hash + "_Pukes"))
        {
            return PlayerPrefs.GetInt("highscores_" + hash + "_Pukes");
        }
        return -1;
    }

    public void CountAPuke()
    {
        if (isRunning)
        {
            pukeCount++;
        }
    }

    public string getMD5ofFile(string s) //gets MD5 from File on Disc!!
    {
        FileStream f = File.OpenRead(s);

        byte[] data = new byte[f.Length];
        f.Read(data, 0, (int)f.Length);

        MD5 md5 = MD5.Create();
        byte[] hash = md5.ComputeHash(data);

        f.Close();

        return BitConverter.ToString(hash);
    }
}
