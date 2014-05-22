using UnityEngine;
using System.Collections;

public class LoginManager : MonoBehaviour
{

    public enum LoginStatus { LoggedOut, LoggingIn, LoggedIn, Reconnecting, Refused };

    private static LoginManager instance = null;

    private string _cookie;

    private static LoginStatus _globalStatus;

    public static LoginStatus GlobalStatus { get { return _globalStatus; } set { _globalStatus = value; } }

    public string Cookie { get { return _cookie; } }

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

    public void tryConnect()
    {
    }
}
