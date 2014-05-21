using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Security.Cryptography;

public class CheckAndUpdateFiles : MonoBehaviour {

	// Use this for initialization
    public SceneDestroyer destroyer;
    public GUIStyle gstyle;

    string localDir = "";
    List<string> toPatch = new List<string>();
    int status = 2; //0 = scanning, 1 = has to be Patched!, 2 = All OK;
    string currentFile = "";

	void Start () {

        string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        if (UnityEngine.Debug.isDebugBuild) localDir = appPath + @"\..\..\test_Data";
        else localDir = appPath + @"\..\";

        if (!localDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            localDir += Path.DirectorySeparatorChar;
        }

        Uri loc = new Uri(localDir);
        localDir = new Uri(loc.AbsoluteUri).LocalPath;

        WWWForm myform = new WWWForm();
        Debug.Log(myform.headers["Content-Type"]);

        ThreadStart ts = new ThreadStart(CheckFiles);
        Thread th = new Thread(ts);
        //th.Start();
	}

    void OnGUI()
    {
        switch (status)
        {
            case 0: GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 30, 400, 60), "SCANNING DATA...\n" + currentFile, gstyle); break;
            case 1: GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 30, 400, 60), "SOME DATA HAVE TO BE PATCHED!" + currentFile, gstyle);
                if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 30, 200, 60), "Patch now"))
                {
                    string parameter = "";

                    foreach(string s in toPatch)
                    {
                        parameter += s + " ";
                    }
                    Debug.Log(localDir + @"\..\Patcher.exe");
                    System.Diagnostics.Process.Start(localDir + @"..\Patcher.exe", parameter);
                    Application.Quit();
                } break;
        }

    }

	// Update is called once per frame
	void Update () {
        switch (status)
        {
            case 2:
                SoundController.Instance.startBackgroundSoundLoop();
                SceneManager.Instance.loadScene(SceneManager.Scene.Login);
                break;
        }
	}

    void CheckFiles()
    {

        List<string> musthave = new List<string>();
        List<string> currentlyhave = new List<string>();

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(getRemoteXMLFileList());

        XmlNodeList xnList = doc.SelectNodes("/files/file");
        foreach (XmlNode xn in xnList)
        {
            musthave.Add(xn.InnerText);
            //Debug.Log("Should have: " + xn.InnerText);
        }

        if (!Directory.Exists(localDir)) Directory.CreateDirectory(localDir);

        Debug.Log(localDir);

        DirectoryInfo dirInfo = new DirectoryInfo(localDir);

        FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            currentlyhave.Add(GetRelativePath(files[i].FullName, localDir));
            //Debug.Log("Have: " + GetRelativePath(files[i].FullName, localDir));
        }

        foreach (string s in musthave)
        {
            if (!s.Equals("output_log.txt")) //exclude output_log.txt
            {
                if (currentlyhave.Contains(s))
                {
                    if (!ScoreController.Instance.getMD5ofFile(localDir + s).Equals(getRemoteMD5(s)))
                    {
                        Debug.Log("Files are not the same " + s);
                        toPatch.Add(s);
                    }
                }
                else
                {
                    toPatch.Add(s);
                }
            }
        }

        if (toPatch.Count == 0) status = 2;
        else
        {
            foreach (string s in toPatch)
            {
                Debug.Log(s);
            }
            status = 1;
        }
    }

    private string getRemoteMD5(string s) //gets SHA from File on Server via Script!!
    {
        return ScoreController.Instance.getMD5ofFile(@"C:\Users\Dominic\Desktop\test_Data\" + s);
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

    private string getRemoteXMLFileList()
    {
        string xml = "";

        xml += "<files>";

        xml += "<file>test.txt</file>";

        xml += "</files>";

        return xml;
    }
}
