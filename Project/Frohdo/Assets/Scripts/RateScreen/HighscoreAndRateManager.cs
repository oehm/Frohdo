using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class HighscoreAndRateManager : MonoBehaviour
{

    public enum RatingStatus { Unloaded, Unrated, Uploading, Rated, LevelNotFoundOnline, LoadingCurData, Uploaded, Error }

    private static HighscoreAndRateManager instance = null;

    private static RatingStatus _globalStatus;

    public RatingStatus GlobalStatus { get { return _globalStatus; } }

    private static WWWForm form;
    private static WWW request;

    private string Levelhash;
    private int rating;
    private int difficultyrating;
    private int timescore, pukescore;
    
    private bool uploadstarted;
    private bool loadStarted;

    public int Rating { get { return rating; } }
    public int DifficultyRating { get { return difficultyrating; } }

    public static HighscoreAndRateManager Instance
    {
        get
        {
            return instance;
        }
    }
    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_globalStatus)
        {
            case RatingStatus.Uploading:
                if (!uploadstarted)
                {
                    uploadstarted = true;
                    StartCoroutine(uploadRatingAndHighscore());
                }
                break;

            case RatingStatus.LoadingCurData:
                if (!loadStarted)
                {
                    loadStarted = true;
                    StartCoroutine(getCurRating());
                }
                break;
        }
    }

    IEnumerator uploadRatingAndHighscore()
    {
        Hashtable requHeaders = new Hashtable();
        requHeaders.Add("Cookie", NetworkManager.Instance.Cookie);
        form = new WWWForm();
        form.AddField("hashCode", Levelhash);
        form.AddField("highscore_time", timescore);
        form.AddField("highscore_points", pukescore);

        if(rating != -1 && difficultyrating != -1)
        {
            form.AddField("rating", rating);
            form.AddField("difficulty", difficultyrating);
        }

        request = new WWW(GlobalVars.Instance.RateLevelAndUploadHighscoreUrl, form.data, requHeaders);
        yield return request;

        Hashtable header = new Hashtable();
        foreach (string s in request.responseHeaders.Keys)
        {
            header.Add(s, request.responseHeaders[s]);
        }

        if (header.ContainsKey("STATUS"))
        {
            Debug.Log(header["STATUS"]);
            if (header["STATUS"].ToString().Equals("200 OK"))
            {
                NetworkManager.Instance.savenewCookie(request);
                _globalStatus = RatingStatus.Uploaded;
                yield break;
            }
            else
            {
                Debug.Log("Some errors occured!");
                _globalStatus = RatingStatus.Error;
                yield break;
            }
        }
        else
        {
            Debug.Log("No Connection to Server!");
            _globalStatus = RatingStatus.Error;
            yield break;
        }
    }

    IEnumerator getCurRating()
    {
        Hashtable requHeaders = new Hashtable();
        requHeaders.Add("Cookie", NetworkManager.Instance.Cookie);

        request = new WWW(GlobalVars.Instance.GetCurRatingforLevelUrl + @"?hashCode=" + Levelhash, null, requHeaders);
        yield return request;

        Hashtable header = new Hashtable();
        foreach (string s in request.responseHeaders.Keys)
        {
            header.Add(s, request.responseHeaders[s]);
        }

        if (header.ContainsKey("STATUS"))
        {
            Debug.Log(header["STATUS"]);
            if (header["STATUS"].ToString().Equals("200 OK"))
            {
                NetworkManager.Instance.savenewCookie(request);
                parseXML(request.text);
                yield break;
            }
            else
            {
                Debug.Log("Some errors occured!");
                _globalStatus = RatingStatus.LevelNotFoundOnline;
                yield break;
            }
        }
        else
        {
            Debug.Log("No Connection to Server!");
            _globalStatus = RatingStatus.LevelNotFoundOnline;
            yield break;
        }
    }

    public void SetNewData(int time, int pukes, int rating = -1, int difficultyrating = -1)
    {
        this.timescore = time;
        this.pukescore = pukes;
        if(rating != -1) this.rating = rating;
        if(difficultyrating != -1) this.difficultyrating = difficultyrating;
        _globalStatus = RatingStatus.Uploading;
    }

    public void loadCurData(string hash)
    {
        this.Levelhash = hash;
        _globalStatus = RatingStatus.LoadingCurData;
    }

    void Reset()
    {
        _globalStatus = RatingStatus.Unloaded;
        uploadstarted = false;
        loadStarted = false;
        form = new WWWForm();
        Levelhash = null;
        difficultyrating = -1;
        rating = -1;
    }

    private void parseXML(string XML)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(XML);
        foreach (XmlNode n in doc.GetElementsByTagName("level"))
        {
            foreach (XmlNode cn in n.ChildNodes)
            {
                if (cn.Name.Equals("rating"))
                {
                    if (cn.InnerText.Trim().Equals(""))
                    {
                        rating = -1;
                    }
                    else
                    {
                        rating = Convert.ToInt32(cn.InnerText);
                    }
                }
                if (cn.Name.Equals("difficulty"))
                {
                    if (cn.InnerText.Trim().Equals(""))
                    {
                        difficultyrating = -1;
                    }
                    else
                    {
                        difficultyrating = Convert.ToInt32(cn.InnerText);
                    }
                }
            }
            if (rating == -1 || difficultyrating == -1)
            {
                _globalStatus = RatingStatus.Unrated;
                return;
            }
            else
            {
                _globalStatus = RatingStatus.Rated;
                return;
            }
        }
    }
}
