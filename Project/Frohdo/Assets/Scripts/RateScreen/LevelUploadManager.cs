using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LevelUploadManager : MonoBehaviour
{

    public enum LevelUploadStatus { CheckingLevelType, ReadyForUpload, NoCustomLevel, Uploading, FailedOnMD5Check, FailedOnUpload, FinishedUploadSuccessful };
    public enum HashEqualsStatus { Unchecked, HashSame, HashDiffer }

    private static LevelUploadManager instance = null;

    private string _cookie;

    private static LevelUploadStatus _globalStatus;

    private static HashEqualsStatus _globalHashStatus;

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
    }

    private IEnumerator UploadLevel()
    {
        //form.headers["Content-Type"] = "multipart/form-data";
        form.headers.Add("accept-charset", "UTF-8");
        //form.headers.Add("action", "/levels");
        //form.headers.Add("class", "new_level");
        //form.headers.Add("enctype", "multipart/form-data");
        //form.headers.Add("id", "new_level");
        //form.headers.Add("method", "post");
        form.headers.Add("Cookie", "request_method=GET; " + NetworkManager.Instance.Cookie);

        //form.AddBinaryData("level[hashCode]", System.Text.Encoding.UTF8.GetBytes(ScoreController.Instance.LevelHash));
        //form.AddBinaryData("level[title]", System.Text.Encoding.UTF8.GetBytes(SceneManager.Instance.levelToLoad.LevelTitle));
        //form.AddBinaryData("level[description]", System.Text.Encoding.UTF8.GetBytes(SceneManager.Instance.levelToLoad.LevelDescription));

        form.AddField("level[hashCode]", ScoreController.Instance.LevelHash);
        form.AddField("level[title]", SceneManager.Instance.levelToLoad.LevelTitle);
        form.AddField("level[description]", SceneManager.Instance.levelToLoad.LevelDescription);

        WWW xmlFile = new WWW("file:///" + SceneManager.Instance.levelToLoad.LeveltoLoad);
        yield return xmlFile;
        if (xmlFile.error != null && !xmlFile.error.Equals(""))
        {
            Debug.Log("Error loading xml for upload! -- " + xmlFile.error);
            _globalStatus = LevelUploadStatus.FailedOnUpload;
            yield break;
        }

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

            form.AddBinaryData("level[urlXML]", xmlFile.bytes, SceneManager.Instance.levelToLoad.LevelTitle + ".xml", "text/xml");
            form.AddBinaryData("level[urlThumbnail]", thumbnail.bytes, SceneManager.Instance.levelToLoad.LevelTitle + "_thumb.jpg", "image/jpeg");

            form.AddField("commit", "Create");
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
            yield break;
        }
    }

    public void StartUploadLevel()
    {
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn && _globalStatus == LevelUploadStatus.ReadyForUpload)
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
