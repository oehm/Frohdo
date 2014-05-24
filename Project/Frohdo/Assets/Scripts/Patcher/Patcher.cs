using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Security.Cryptography;

public class Patcher : MonoBehaviour {

	// Use this for initialization
    public SceneDestroyer destroyer;
    public GUIStyle gstyle;

	void Start () {
        Debug.Log(PatcherManager.Instance);
        PatcherManager.Instance.checkForPatch();
	}

    void OnGUI()
    {
        switch (PatcherManager.Instance.GlobalStatus)
        {
            case PatcherManager.PatcherStatus.Patching: GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 30, 400, 60), "SCANNING DATA...\n" + PatcherManager.Instance.CurCheckedFile, gstyle); break;
            case PatcherManager.PatcherStatus.Unpatched: GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 30, 400, 60), "SOME DATA HAVE TO BE PATCHED!", gstyle);
                if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 30, 200, 60), "Patch now"))
                {
                    PatcherManager.Instance.commitPatch();
                } break;
        }

    }

	// Update is called once per frame
	void Update () {
        switch (PatcherManager.Instance.GlobalStatus)
        {
            case PatcherManager.PatcherStatus.Patched:
                SoundController.Instance.startBackgroundSoundLoop();
                SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
                break;
        }
	}
}
