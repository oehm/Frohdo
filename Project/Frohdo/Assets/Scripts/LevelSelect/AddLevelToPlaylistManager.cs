using UnityEngine;
using System.Collections;

public class AddLevelToPlaylistManager : MonoBehaviour
{

    public enum DownloadStatus { NotDownloaded, Downloading, Downloaded, Error };

    private static AddLevelToPlaylistManager instance = null;

    private static DownloadStatus _globalStatus;

    public DownloadStatus GlobalStatus { get { return _globalStatus; } }

    private static WWWForm form;
    private static WWW request;

    bool started = false;

    int leveltoAddid = -1;

    public static AddLevelToPlaylistManager Instance
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
                    StartCoroutine(addToPlaylist());
                }
                break;

            case DownloadStatus.Downloaded:
                {
                    started = false;
                }
                break;
        }
    }

    public void addToPlaylist(int levelid)
    {

        if (_globalStatus != DownloadStatus.Downloading)
        {
            leveltoAddid = levelid;
            form = new WWWForm();
            _globalStatus = DownloadStatus.Downloading;
        }
    }

    IEnumerator addToPlaylist()
    {
        if (leveltoAddid == -1) yield break;
        Hashtable requHeaders = new Hashtable();
        requHeaders.Add("Cookie", NetworkManager.Instance.Cookie);
        //requHeaders.Add("GET", "GET levels/add_level_to_playlist?id="+ leveltoAddid + " HTTP/1.1");
        //GET /levels/add_level_to_playlist?id=51 HTTP/1.1

        //Debug.Log(GlobalVars.Instance.AddToPlaylistUrl + leveltoAddid, null, requHeaders);

        request = new WWW(GlobalVars.Instance.AddToPlaylistUrl + leveltoAddid, null, requHeaders);
        yield return request;

        Hashtable header = new Hashtable();
        foreach (string s in request.responseHeaders.Keys)
        {
            header.Add(s, request.responseHeaders[s]);
        }

        if (header.ContainsKey("STATUS"))
        {
            if (header["STATUS"].ToString().Equals("200 OK"))
            {
                _globalStatus = DownloadStatus.Downloaded;
            }
            else
            {
                Debug.Log(header["STATUS"]);
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
