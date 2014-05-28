using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class PatcherManager : MonoBehaviour
{

    public enum PatcherStatus { Unchecked, Patching, Patched, Error, Unpatched };

    private static PatcherManager instance = null;

    private PatcherStatus _globalStatus;

    public PatcherStatus GlobalStatus { get { return _globalStatus; } }

    private static WWW request;

    bool started = false;

    string url = "http://community.mediacube.at/patchers.json";

    string localDir = "";

    private List<Hashtable> toPatch = new List<Hashtable>();

    private string curCheckedFile = "";

    public string CurCheckedFile { get { return curCheckedFile; } }

    public static PatcherManager Instance
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
        _globalStatus = PatcherStatus.Unchecked;

        string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        if (UnityEngine.Debug.isDebugBuild) localDir = appPath + @"\..\..\test_Data";
        else localDir = appPath + @"\..\";

        if (!localDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            localDir += Path.DirectorySeparatorChar;
        }

        Uri loc = new Uri(localDir);
        localDir = new Uri(loc.AbsoluteUri).LocalPath;

        started = false;
    }

    void Update()
    {
        switch (_globalStatus)
        {
            case PatcherStatus.Patching:
                if (!started)
                {
                    started = true;
                    StartCoroutine(CheckFiles());
                }
                break;
        }
    }

    public void checkForPatch()
    {
        if (UnityEngine.Debug.isDebugBuild)
        {
            Debug.Log("PATCHER INACTIVE IN DEBUG MODUS!");
            _globalStatus = PatcherStatus.Patched;
        }
        else _globalStatus = PatcherStatus.Patching;
    }

    IEnumerator CheckFiles()
    {
        request = new WWW(url);
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
                GeneratePatchInformation();
            }
            else
            {
                Debug.Log("Something went wrong in Patcher script!");
                Debug.Log("Error Code: " + header["STATUS"]);
                _globalStatus = PatcherStatus.Error;
            }
        }
        else
        {
            Debug.Log("No Connection to Server!");
            _globalStatus = PatcherStatus.Error;
        }
    }

    private void GeneratePatchInformation()
    {
        List<string> currentlyhave = new List<string>();

        List<Hashtable> remotefiles = getRemoteFileList();

        if (!Directory.Exists(localDir)) Directory.CreateDirectory(localDir);

        //Debug.Log(localDir);

        DirectoryInfo dirInfo = new DirectoryInfo(localDir);

        FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            currentlyhave.Add(files[i].Name);
            //Debug.Log("Filename: " + files[i].Name);
        }

        foreach (Hashtable s in remotefiles)
        {
            curCheckedFile = s["name"].ToString();
            if (!s.Equals("output_log.txt")) //exclude output_log.txt
            {
                if (currentlyhave.Contains(s["name"].ToString()))
                {
                    if (!ScoreController.Instance.getMD5ofFile(localDir + s).Equals(s["hashCode"].ToString()))
                    {
                        //Debug.Log("Files are not the same " + s["FileName"]);
                        toPatch.Add(s);
                    }
                }
                else
                {
                    toPatch.Add(s);
                }
            }
        }

        if (toPatch.Count == 0) _globalStatus = PatcherStatus.Patched;
        else
        {
            foreach (Hashtable s in toPatch)
            {
                Debug.Log("toPatch: " + s["name"]);
            }
            _globalStatus = PatcherStatus.Unpatched;
        }
    }

    private List<Hashtable> getRemoteFileList()
    {
        List<Hashtable> files = new List<Hashtable>();

        string json = request.text.Trim();
        json = json.Replace("[", ""); json = json.Replace("]", "");
        json = json.Replace("},{", "~");

        if (json.Length == 0) return files;
        string[] split = json.Split('~');

        Debug.Log(json + " " + split.Length + " " + split[0]);

        foreach (string s in split)
        {
            Hashtable table = new Hashtable();
            string sn = s.Replace("{", "");
            string[] ssplit = sn.Split(',');
            foreach (string str in ssplit)
            {
                string strn = str.Replace("\":", "~");
                strn = strn.Replace("\"", "");
                string[] entry = strn.Split('~');
                table.Add(entry[0], entry[1]);
            }
            files.Add(table);
        }
        return files;
    }

    string GetRelativePath(string filespec, string folder)
    {
        Uri pathUri = new Uri(filespec);
        // Folders must end in a slash
        if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            folder += Path.DirectorySeparatorChar;
        }
        Uri folderUri = new Uri(folder);
        return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
    }

    public void commitPatch()
    {
        string parameter = "";

        foreach (Hashtable s in toPatch)
        {
            parameter += "\"" + s["urlFiles"] + "\" \"" + s["urlLocalDir"] + s["name"] + "\" ";
        }
        Debug.Log(localDir + @"\..\Patcher.exe");
        Debug.Log(parameter);
        System.Diagnostics.Process.Start(localDir + @"..\Patcher.exe", parameter);
        Application.Quit();
    }
}
