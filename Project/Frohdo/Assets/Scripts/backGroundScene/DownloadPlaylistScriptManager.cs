using UnityEngine;
using System.Collections;

public class DownloadPlaylistScriptManager : MonoBehaviour
{

    public enum DownloadStatus { NotDownloaded, Downloading, Downloaded, Error };

    private static DownloadPlaylistScriptManager instance = null;

    private static DownloadStatus _globalStatus;

    public DownloadStatus GlobalStatus { get { return _globalStatus; } }

    public string XML;

    private static WWWForm form;
    private static WWW request;

    bool started = false;

    public static DownloadPlaylistScriptManager Instance
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

    public void startLoadingLevelList()
    {
        if (_globalStatus != DownloadStatus.Downloading)
        {
            form = new WWWForm();
            _globalStatus = DownloadStatus.Downloading;
        }
    }

    IEnumerator download()
    {
        Hashtable requHeaders = new Hashtable();
        requHeaders.Add("Cookie", NetworkManager.Instance.Cookie);

        request = new WWW(GlobalVars.Instance.playlistUrl, form.data, requHeaders);
        yield return request;
        Hashtable header = new Hashtable();
        foreach (string s in request.responseHeaders.Keys)
        {
            header.Add(s, request.responseHeaders[s]);
            //Debug.Log(s + request.responseHeaders[s]);
        }

        if (header.ContainsKey("STATUS"))
        {
            if (header["STATUS"].ToString().Equals("200 OK"))
            {
                //Debug.Log(request.text);
                XML = request.text;
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
        if(NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn) NetworkManager.Instance.savenewCookie(request);
        _globalStatus = DownloadStatus.Downloaded;
    }

    public void reset()
    {
        _globalStatus = DownloadStatus.NotDownloaded;
        started = false;
    }
}
