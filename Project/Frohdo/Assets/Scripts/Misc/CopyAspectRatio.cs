using UnityEngine;
using System.Collections;

public class CopyAspectRatio : MonoBehaviour {

    Camera mainCamera_;

    int lastScreenHeight = 0;
    int lastScreenWidth = 0;
    bool lastfullscreen = false;

	// Use this for initialization
	void Start () {
        mainCamera_ = ForceAspectRatio.CameraMain;
	}
	
	// Update is called once per frame
	void Update () {
        if ((Screen.height != lastScreenHeight) || (Screen.width != lastScreenWidth) || (Screen.fullScreen != lastfullscreen))
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            lastfullscreen = Screen.fullScreen;
            camera.rect = mainCamera_.rect;
            camera.aspect = mainCamera_.aspect;
        }
	}

    public void forceUpdate()
    {
        mainCamera_ = ForceAspectRatio.CameraMain;
        Update();
    }
}
