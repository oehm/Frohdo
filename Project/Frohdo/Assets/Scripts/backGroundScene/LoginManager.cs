using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class LoginManager : MonoBehaviour
{

    public enum LoginStatus { LoggedOut, LoggingIn, LoggedIn, Reconnecting, Refused };

    private static LoginManager instance = null;

    private string _cookie;

    private static LoginStatus _globalStatus;

    public static LoginStatus GlobalStatus { get { return _globalStatus; } }

    public string Cookie { get { return _cookie; } }

    private string user, pass;

    public string User { get { return user; } }

    private static WWWForm form;
    private static WWW request;

    String url = "";

    bool started = false;

    private int reconnects = 0;

    public static LoginManager Instance
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
        _globalStatus = LoginStatus.LoggedOut;
    }

    void Update()
    {
        switch (_globalStatus)
        {
            case LoginStatus.LoggedIn:
                break;

            case LoginStatus.LoggingIn:
                if (!started)
                {
                    _cookie = "";
                    started = true;
                    StartCoroutine(LoginAndGetCookie());
                }
                break;

            case LoginStatus.Reconnecting:
                if (!started)
                {
                    _cookie = "";
                    started = true;
                    StartCoroutine(LoginAndGetCookie());
                }
                break;


            case LoginStatus.Refused:
                break;


        }
    }

    public void tryConnect(string user, string pass)
    {
        if (_globalStatus == LoginStatus.LoggedOut)
        {
            _globalStatus = LoginStatus.LoggingIn;
            form = new WWWForm();
            this.user = user;
            this.pass = pass;
            form.AddField("user[email]", user);
            form.AddField("user[password]", pass);
            form.AddField("user[remember_me]", 0);
            form.AddField("commit", "Sign in");
        }
    }

    private string ByteArrayToString(byte[] arr)
    {
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        return enc.GetString(arr);
    }

    IEnumerator MakeRequest()
    {
        request = new WWW(url, form.data, form.headers);
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

            }
        }
        else
        {
            reconnects = 0;
            _globalStatus = LoginStatus.Reconnecting; //No Connection
        }

        savenewCookie();
    }

    IEnumerator LoginAndGetCookie()
    {
        request = new WWW("http://community.mediacube.at/users/sign_in", form.data, form.headers);
        yield return request;
        Hashtable header = new Hashtable();
        foreach (string s in request.responseHeaders.Keys)
        {
            header.Add(s, request.responseHeaders[s]);
        }

        if (header.ContainsKey("STATUS"))
        {
            if (header["STATUS"].ToString().Equals("HTTP/1.1 302 Found"))
            {
                request = new WWW("http://community.mediacube.at/", null, header);
                yield return request;
            }
            else
            {
                Debug.Log("Logindata incorrect");
                LogOut();
            }
        }else{
            Debug.Log("No Connection to Server!");
            if(_globalStatus != LoginStatus.Reconnecting){
                _globalStatus = LoginStatus.Refused;
            }else{
                if(reconnects < 4){
                    reconnects++;
                    started = false;
                }
            }
        }
        if(_globalStatus != LoginStatus.Reconnecting ||(_globalStatus == LoginStatus.Reconnecting && reconnects >= 4)) savenewCookie();
    }

    void savenewCookie()
    {
        started = false;
        if (String.IsNullOrEmpty(request.error))
        {
            if (request.responseHeaders.ContainsKey("SET-COOKIE"))
            {
                String[] data = request.responseHeaders["SET-COOKIE"].Split(";"[0]);
                if (data.Length > 0)
                {
                    _cookie = data[0];
                    Debug.Log("newCookie: " + _cookie);
                    _globalStatus = LoginStatus.LoggedIn;
                    return;
                }
            }
            else
            {
                Debug.Log("OH NO WE DIDNT GET A NEW COOKIE!!!! NOOOOOOOOOOOOOOOOOOO");
            }
        }
        else
        {
            Debug.Log(request.error);
        }
        _globalStatus = LoginStatus.LoggedOut;
    }

    public void LogOut()
    {
        _cookie = "";
        pass = "";
        _globalStatus = LoginStatus.LoggedOut;
        started = false;
    }
}
