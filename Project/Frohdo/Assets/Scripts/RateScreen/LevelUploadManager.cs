using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LevelUploadManager : MonoBehaviour
{

    public enum LevelUploadStatus { CheckingLevelType, ReadyForUpload, NoCustomLevel, Uploading, FailedOnMD5Check, FailedOnUpload, FinishedUploadSuccessful, LevelAlreadyAvailableOnline };
    public enum HashEqualsStatus { Unchecked, HashSame, HashDiffer }

    private static LevelUploadManager instance = null;

    private string _cookie;

    private static LevelUploadStatus _globalStatus;

    private static HashEqualsStatus _globalHashStatus;

    public string UploadProgress;
    private bool everythingPreparedForUpload;

    public LevelUploadStatus GlobalStatus { get { return _globalStatus; } }

    //public HashEqualsStatus GlobalHashStatus { get { return _globalHashStatus; } }

    private static WWWForm form;
    private static WWW request;

    bool checkStarted;
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
        UploadProgress = "";
        everythingPreparedForUpload = false;

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
                    UploadProgress = "";
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
                else if(everythingPreparedForUpload)
                {
                    UploadProgress = " (" + ((float)((int)(request.uploadProgress*10000))/100) + ")";
                }
                break;
        }
    }

    private IEnumerator UploadLevel()
    {
        //form.headers["Content-Type"] = "multipart/form-data";
        //form.headers.Add("accept-charset", "UTF-8");
        //form.headers.Add("action", "/levels");
        //form.headers.Add("class", "new_level");
        //form.headers.Add("enctype", "multipart/form-data");
        //form.headers.Add("id", "new_level");
        //form.headers.Add("method", "post");
        //form.headers.Add("Cookie", "request_method=GET; " + NetworkManager.Instance.Cookie);
        //form.headers.Add("Cookie", "_pukingGame_session=VTFVQU45cEFER2dPQzlMc0hYTXhOYUtHU05jWGp4YjJnYTREQzd4aTltNTFLenhtNGxSTU9PeTlhM2wrcDI4SHBKU01vUGtISkNEQVZwK1M4YTNTdWdHWHVzc0N4b2ZkWEVGZ0hBS3c2RUVYR1AzeTh3cVN4WHRwcUsyNFpiQk10NHcvT3BWbWY2dHp6MEdMTmdFQ0RiUklkeXArY0xPM0xBS3B2VmpoVUJDczYvbHBaYlFkZk1hT21aeXBYM0MzNVVjUy9rQkpUY3FOWnNVL2lyQkJXSENSbVovMkd3Vy9QR0dsSEdmUmh4bmt4M1l3YzJmTjJDMStMK3FPMW54cnF0d1lZTTNKQnBMZktFVmR2aGRsc0YwblRjTjNwU2FxdmhocE5wMkc2RU5PTVVwZUs1OCt5NjdJN3pLc1VGYXdUM2ZjbytzUlNpb2FUZVRxR0hEV0ZCV21lSzB0Z25HN1dDTktmUkdHeEVlOVpFL3ZMZytCdmEzdlJFbUhxZlhQLS1sanRqWlo3SEF5a3JNeGgwRjhTZjR3PT0%3D--b87b046da82c51592c6bf6fd504c96e0f56382eb; path=/; HttpOnly");
        //form.AddBinaryData("level[hashCode]", System.Text.Encoding.UTF8.GetBytes(ScoreController.Instance.LevelHash));
        //form.AddBinaryData("level[title]", System.Text.Encoding.UTF8.GetBytes(SceneManager.Instance.levelToLoad.LevelTitle));
        //form.AddBinaryData("level[description]", System.Text.Encoding.UTF8.GetBytes(SceneManager.Instance.levelToLoad.LevelDescription));

        form.AddField("level[hashCode]", ScoreController.Instance.LevelHash);
        form.AddField("level[title]", SceneManager.Instance.levelToLoad.LevelTitle);
        form.AddField("level[description]", SceneManager.Instance.levelToLoad.LevelDescription + "Description");
        UploadProgress = " (Loading XML)";
        WWW xmlFile = new WWW("file:///" + SceneManager.Instance.levelToLoad.LeveltoLoad);
        yield return xmlFile;
        if (xmlFile.error != null && !xmlFile.error.Equals(""))
        {
            Debug.Log("Error loading xml for upload! -- " + xmlFile.error);
            _globalStatus = LevelUploadStatus.FailedOnUpload;
            yield break;
        }
        UploadProgress = " (Loading Thumbnail)";
        WWW thumbnail = new WWW("file:///" + SceneManager.Instance.levelToLoad.thumbpath);
        yield return xmlFile;
        if (thumbnail.error != null && !thumbnail.error.Equals(""))
        {
            Debug.Log("No thumb found!");
            _globalStatus = LevelUploadStatus.FailedOnUpload;
            yield break;
        }

        if (_globalStatus != LevelUploadStatus.FailedOnUpload)
        {            
            Hashtable requHeaders = new Hashtable();
            form.AddBinaryData("level[urlXML]", xmlFile.bytes, SceneManager.Instance.levelToLoad.LevelTitle + ".xml", "text/xml");
            form.AddBinaryData("level[urlThumbnail]", thumbnail.bytes, SceneManager.Instance.levelToLoad.LevelTitle + "_thumb.png", "image/png");

            foreach (string ke in form.headers.Keys)
            {
                requHeaders.Add(ke, form.headers[ke]);
            }
            requHeaders.Add("Cookie", NetworkManager.Instance.Cookie);
            everythingPreparedForUpload = true;
            request = new WWW(GlobalVars.Instance.LevelUploadUri, form.data, requHeaders);
            yield return request;
            NetworkManager.Instance.savenewCookie(request);

            Hashtable header = new Hashtable();
            foreach (string s in request.responseHeaders.Keys)
            {
                header.Add(s, request.responseHeaders[s]);
                //Debug.Log("Upload-Header: " + s + ": " + request.responseHeaders[s]);
            }

            if (header.ContainsKey("STATUS"))
            {
                if (header["STATUS"].ToString().Equals("200 OK"))
                {
                    _globalStatus = LevelUploadStatus.FinishedUploadSuccessful;
                }
                else if (header["STATUS"].ToString().Equals("405 Method Not Allowed"))
                {
                    _globalStatus = LevelUploadStatus.LevelAlreadyAvailableOnline;
                }
                else
                {
                    //foreach (string ke in requHeaders.Keys)
                    //{
                    //    Debug.Log("request-Header: " + ke + ": " + requHeaders[ke]);
                    //}
                    //Debug.Log("DONE!");
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
            yield break;
        }
        everythingPreparedForUpload = false;
    }

    public void StartUploadLevel()
    {
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn && _globalStatus == LevelUploadStatus.ReadyForUpload)
            _globalStatus = LevelUploadStatus.Uploading;
    }

    private void checkHashOfLevel()
    {
        UploadProgress = " (Checking hash)";
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
