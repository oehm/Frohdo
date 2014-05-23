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

    public string LevelHash 
    { 
        get 
        { 
            return levelHash_; 
        } 
        set 
        { 
            levelHash_ = value;
            isRunning = false;
            pukeCount = 0;
            timeCount = 0;
        } 
    }
    private string levelHash_;
    public bool isRunning{get;set;}

    public int pukeCount { get; private set; }
    public int timeCount { get; private set; }

	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        LevelHash = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {        
            timeCount += (int)(Time.deltaTime * 1000.0f);
        }
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
