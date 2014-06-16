using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class DownloadHighscoreManager : MonoBehaviour {

    public enum HighscoreType { LocalTime, LocalPuke, OnlineTime, OnlinePuke, None };
    public enum DownloadStatus { NotDownloaded, Downloading, Downloaded, Error, NotFoundOnServer };

    private static DownloadHighscoreManager instance = null;

    private static DownloadStatus _globalStatus;

    public DownloadStatus GlobalStatus { get { return _globalStatus; } }

    private static WWW request;

    bool started = false;

    List<Highscore> highscores;

    HighscoreType typeToLoad;

    string hash;

    public static DownloadHighscoreManager Instance
    {
        get
        {
            return instance;
        }
    }

	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        _globalStatus = DownloadStatus.NotDownloaded;
	}
	
	// Update is called once per frame
    void Update()
    {
        switch (_globalStatus)
        {
            case DownloadStatus.Downloading:
                if (!started)
                {
                    started = true;
                    StartCoroutine("download");
                }
                break;

            case DownloadStatus.Downloaded:
                {
                    started = false;
                }
                break;
        }
    }

    public void startDownload(string hash, ref List<Highscore> highscores, HighscoreType type)
    {
        reset();
        highscores = new List<Highscore>();
        this.highscores = highscores;
        this.hash = hash;
        typeToLoad = type;
        _globalStatus = DownloadStatus.Downloading;
    }

    IEnumerator download()
    {
        //yield return new WaitForSeconds(0.01f);
        Hashtable requHeaders = new Hashtable();
        requHeaders.Add("Cookie", NetworkManager.Instance.Cookie);
        ////Debug.Log(GlobalVars.Instance.CommunityBasePath + LeveltoDownloadpath);
        request = new WWW(GlobalVars.Instance.GetHighscoreUrl + @"?hashCode=" + hash + "&amount_entries=" + GlobalVars.Instance.highscorestoload, null, requHeaders);
        yield return request;

        Hashtable header = new Hashtable();
        foreach (string s in request.responseHeaders.Keys)
        {
            header.Add(s, request.responseHeaders[s]);
        }

        if (header.ContainsKey("STATUS"))
        {
            //Debug.Log(header["STATUS"]);
            if (header["STATUS"].ToString().Equals("200 OK"))
            {
                NetworkManager.Instance.savenewCookie(request);
                parseHighscoreXML(request.text);
                //Debug.Log(request.text);
            }
            else
            {
                //Debug.Log("Some errors occured!");
                _globalStatus = DownloadStatus.NotFoundOnServer;
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

    private void parseHighscoreXML(string XML)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(XML);

        if (typeToLoad == HighscoreType.OnlineTime)
        {
            foreach (XmlNode n in doc.GetElementsByTagName("toplist_time"))
            {
                int rank = -1;
                string user = "";
                int highscore_time = -1;
                foreach (XmlNode cn in n.ChildNodes)
                {
                    if (cn.Name.Equals("rank")) rank = Convert.ToInt32(cn.InnerText);
                    if (cn.Name.Equals("highscore_time")) highscore_time = Convert.ToInt32(cn.InnerText);
                    if (cn.Name.Equals("user_nick")) user = cn.InnerText;
                }
                highscores.Add(new Highscore(rank, user, highscore_time));
            }

            foreach (XmlNode n in doc.GetElementsByTagName("toplist_time_own"))
            {
                bool ranked = true;

                int rank = -1;
                int highscore_time = -1;

                foreach (XmlNode cn in n.ChildNodes)
                {
                    if (cn.Name.Equals("rank"))
                    {
                        if (cn.InnerText.Equals("no entry"))
                        {
                            ranked = false;
                            break;
                        }
                        else
                        {
                            rank = Convert.ToInt32(cn.InnerText);
                        }
                    }
                    if (cn.Name.Equals("highscore_time")) highscore_time = Convert.ToInt32(cn.InnerText);
                }
                if (ranked)
                {
                    bool ownUnderTopX = false;
                    foreach (Highscore score in highscores)
                    {
                        if (score.rank == rank)
                        {
                            score.ownHighscore = true;
                            ownUnderTopX = true;
                        }
                    }
                    if (!ownUnderTopX)
                    {
                        highscores.RemoveAt(highscores.Count - 1);
                        highscores.Add(new Highscore(rank, "OWN", highscore_time, true));
                    }
                }
            }
        }
        if (typeToLoad == HighscoreType.OnlinePuke)
        {
            foreach (XmlNode n in doc.GetElementsByTagName("toplist_pukes"))
            {
                int rank = -1;
                string user = "";
                int highscore_pukes = -1;
                foreach (XmlNode cn in n.ChildNodes)
                {
                    if (cn.Name.Equals("rank")) rank = Convert.ToInt32(cn.InnerText);
                    if (cn.Name.Equals("highscore_pukes")) highscore_pukes = Convert.ToInt32(cn.InnerText);
                    if (cn.Name.Equals("user_nick")) user = cn.InnerText;
                }
                highscores.Add(new Highscore(rank, user, highscore_pukes));
            }

            foreach (XmlNode n in doc.GetElementsByTagName("toplist_pukes_own"))
            {
                bool ranked = true;

                int rank = -1;
                int highscore_pukes = -1;

                foreach (XmlNode cn in n.ChildNodes)
                {
                    if (cn.Name.Equals("rank"))
                    {
                        if (cn.InnerText.Equals("no entry"))
                        {
                            ranked = false;
                            break;
                        }
                        else
                        {
                            rank = Convert.ToInt32(cn.InnerText);
                        }
                    }
                    if (cn.Name.Equals("highscore_pukes")) highscore_pukes = Convert.ToInt32(cn.InnerText);
                }
                if (ranked)
                {
                    bool ownUnderTopX = false;
                    foreach (Highscore score in highscores)
                    {
                        if (score.rank == rank)
                        {
                            score.ownHighscore = true;
                            ownUnderTopX = true;
                        }
                    }
                    if (!ownUnderTopX)
                    {
                        highscores.RemoveAt(highscores.Count - 1);
                        highscores.Add(new Highscore(rank, "OWN", highscore_pukes, true));
                    }
                }
            }
        }
    }

    public void reset()
    {
        StopCoroutine("download");
        _globalStatus = DownloadStatus.NotDownloaded;
        started = false;
    }
}
