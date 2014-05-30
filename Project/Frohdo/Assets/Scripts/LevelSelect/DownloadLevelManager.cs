using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class DownloadLevelManager : MonoBehaviour
{

    public enum DownloadStatus { NotDownloaded, Downloading, Downloaded, Error };

    private static DownloadLevelManager instance = null;

    private static DownloadStatus _globalStatus;

    public DownloadStatus GlobalStatus { get { return _globalStatus; } }

    private string LeveltoDownloadpath;
    private string thumbdownloadpath;
    private Texture2D thumbnail;
    private string mapname;

    private static WWW request;

    bool started = false;

    public static DownloadLevelManager Instance
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
        _globalStatus = DownloadStatus.NotDownloaded;
    }

    void Update()
    {
        switch (_globalStatus)
        {
            case DownloadStatus.Downloading:
                if (!started)
                {
                    started = true;
                    StartCoroutine(download());
                }
                break;

            case DownloadStatus.Downloaded:
                {
                    started = false;
                }
                break;
        }
    }

    public void startDownload(string path, string thumbdownloadpath, Texture2D thumbnail, string mapname)
    {
        if (_globalStatus != DownloadStatus.Downloading)
        {
            this.thumbdownloadpath = thumbdownloadpath;
            this.thumbnail = thumbnail;
            this.mapname = mapname;
            LeveltoDownloadpath = path;
            _globalStatus = DownloadStatus.Downloading;
        }
    }

    IEnumerator download()
    {
        Hashtable requHeaders = new Hashtable();
        requHeaders.Add("Cookie", NetworkManager.Instance.Cookie);
        //Debug.Log(GlobalVars.Instance.CommunityBasePath + LeveltoDownloadpath);
        request = new WWW(GlobalVars.Instance.CommunityBasePath + LeveltoDownloadpath ,null, requHeaders);
        yield return request;

        Hashtable header = new Hashtable();
        foreach (string s in request.responseHeaders.Keys)
        {
            header.Add(s, request.responseHeaders[s]);
        }

        if (header.ContainsKey("STATUS"))
        {
            //Debug.Log(header["STATUS"]);
            if (header["STATUS"].ToString().Equals("HTTP/1.1 200 OK"))
            {
                if (thumbnail == null)
                {
                    //NetworkManager.Instance.savenewCookie(request);
                    request = new WWW(GlobalVars.Instance.CommunityBasePath + thumbdownloadpath);
                    yield return request;
                    header.Clear();
                    foreach (string s in request.responseHeaders.Keys)
                    {
                        header.Add(s, request.responseHeaders[s]);
                    }

                    if (request.error == null)
                        thumbnail = request.texture;
                    else
                    {
                        _globalStatus = DownloadStatus.Error;
                        yield break;
                    }
                }
                byte[] bytes = thumbnail.EncodeToPNG();
                //Debug.Log(mapname);
                //Debug.Log(Application.dataPath + @"\Levels\downloaded\" + mapname + @"\" + " --- created!");

                if (!Directory.Exists(Application.dataPath + @"\Levels\downloaded\" + mapname + @"\")) Directory.CreateDirectory(Application.dataPath + @"\Levels\downloaded\" + mapname + @"\");

                File.WriteAllBytes(Application.dataPath + @"\Levels\downloaded\" + mapname + @"\" + mapname + @"_thumb.png", bytes);
                System.IO.File.WriteAllText(Application.dataPath + @"\Levels\downloaded\" + mapname + @"\" + mapname + @".xml", request.text);
            }
            else
            {
                Debug.Log("Some errors occured!");
                _globalStatus = DownloadStatus.Error;
                yield break;
            }
        }
        else
        {
            Debug.Log("No Connection to Server!");
            _globalStatus = DownloadStatus.Error;
            yield break;
        }
        //if(NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn) NetworkManager.Instance.savenewCookie(request);
        _globalStatus = DownloadStatus.Downloaded;
    }

    public void reset()
    {
        _globalStatus = DownloadStatus.NotDownloaded;
        started = false;
    }
}
