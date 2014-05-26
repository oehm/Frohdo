﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LevelUploadManager : MonoBehaviour
{

    public enum LevelUploadStatus { CheckingLevelType, ReadyForUpload, NoCustomLevel, Uploading, FailedOnMD5Check, FailedOnUpload, FinishedUploadSuccessful };
    public enum HashEqualsStatus { Unchecked, CheckingHash, HashSame, HashDiffer }

    private static LevelUploadManager instance = null;

    private string _cookie;

    private static LevelUploadStatus _globalStatus;

    private static HashEqualsStatus _globalHashStatus;

    public static LevelUploadStatus GlobalStatus { get { return _globalStatus; } }

    public static HashEqualsStatus GlobalHashStatus { get { return _globalHashStatus; } }

    private static WWWForm form;
    private static WWW request;

    bool checkStarted;
    bool hashCheckStarted;
    bool UploadStarted;

    public static LevelUploadManager Instance
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
        _globalStatus = LevelUploadStatus.CheckingLevelType;
        _globalHashStatus = HashEqualsStatus.Unchecked;
        checkStarted = false;
        UploadStarted = false;
        hashCheckStarted = false;

        form = new WWWForm();
    }

    void Update()
    {
        switch (_globalStatus)
        {
            case LevelUploadStatus.CheckingLevelType:
                if (!checkStarted)
                {
                    checkStarted = true;
                    checkLevelType();
                }
                break;

            case LevelUploadStatus.Uploading:
                if (!UploadStarted)
                {
                    UploadStarted = true;
                    checkHashOfLevel();
                    if (_globalHashStatus == HashEqualsStatus.HashSame)
                    {
                        StartCoroutine(UploadLevel());
                    }
                    else
                    {
                        _globalStatus = LevelUploadStatus.FailedOnMD5Check;
                    }
                }
                break;
        }
        switch (_globalHashStatus)
        {
            case HashEqualsStatus.CheckingHash:
                if (!hashCheckStarted)
                {
                    hashCheckStarted = true;
                    checkHashOfLevel();
                }
                break;
        }
    }

    private IEnumerator UploadLevel()
    {
        Debug.Log("COOKIE SET: " + NetworkManager.Instance.Cookie);
        form.headers.Add("Cookie", NetworkManager.Instance.Cookie);

        form.AddBinaryData("level[hashCode]",System.Text.Encoding.UTF8.GetBytes(ScoreController.Instance.LevelHash));
        form.AddBinaryData("level[title]", System.Text.Encoding.UTF8.GetBytes(SceneManager.Instance.levelToLoad.LevelTitle));
        form.AddBinaryData("level[description]", System.Text.Encoding.UTF8.GetBytes(SceneManager.Instance.levelToLoad.LevelDescription));
        //form.AddBinaryData( level[urlThumbnail]

        string xml = "";

        try
        {
            StreamReader rd = new StreamReader(SceneManager.Instance.levelToLoad.LeveltoLoad);
            xml = rd.ReadToEnd();
            rd.Close();
        }
        catch (Exception)
        {
            Debug.Log("Error loading xml for upload!");
            _globalStatus = LevelUploadStatus.FailedOnUpload;
        }

        if (_globalStatus != LevelUploadStatus.FailedOnUpload)
        {
            form.AddBinaryData("level[urlXML]",System.Text.Encoding.UTF8.GetBytes(xml), SceneManager.Instance.levelToLoad.LevelTitle + ".xml", "text/xml");

            request = new WWW(GlobalVars.Instance.LevelUploadUri, form.data, form.headers);
            yield return request;

            Hashtable header = new Hashtable();
            foreach (string s in request.responseHeaders.Keys)
            {
                header.Add(s, request.responseHeaders[s]);
                Debug.Log("Upload-Header: " + s + ": " + request.responseHeaders[s]);
            }

            if (header.ContainsKey("STATUS"))
            {
                if (header["STATUS"].ToString().Equals("200 OK"))
                {
                    _globalStatus = LevelUploadStatus.FinishedUploadSuccessful;
                }
                else
                {
                    _globalStatus = LevelUploadStatus.FailedOnUpload;
                }
            }
            else
            {
                _globalStatus = LevelUploadStatus.FailedOnUpload; //No Connection
            }
        }
        else
        {
            yield return false;
        }
    }

    public void StartUploadLevel()
    {
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn)
            _globalStatus = LevelUploadStatus.Uploading;
    }

    private void checkHashOfLevel()
    {
        if (ScoreController.Instance.LevelHash == ScoreController.Instance.getMD5ofFile(SceneManager.Instance.levelToLoad.LeveltoLoad))
        {
            _globalHashStatus = HashEqualsStatus.HashSame;
        }
        else
        {
            _globalHashStatus = HashEqualsStatus.HashDiffer;
        }
    }

    private void checkLevelType()
    {
        if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Custom)
        {
            _globalStatus = LevelUploadStatus.ReadyForUpload;
        }
        else
        {
            _globalStatus = LevelUploadStatus.NoCustomLevel;
        }
    }
}
