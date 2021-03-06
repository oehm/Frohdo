﻿// CameraAspectRatio
// By Nicolas Varchavsky @ Interatica (www.interatica.com)
// Date: March 27th, 2009
// Version: 1.0

//Translated to C#  and adaped by Tobias Pretzl


using UnityEngine;
using System.Collections;

public class ForceAspectRatio : MonoBehaviour
{
    int lastScreenHeight  = 0;
    int lastScreenWidth  = 0;
    bool lastfullscreen = false;
    public float _wantedAspectRatio = 16.0f/9.0f; // (16:9)
    // public float _wantedAspectRatio = 1.3333333f; // 4:3
    public bool landscapeModeOnly = false;
    static public bool _landscapeModeOnly = true;
    static float wantedAspectRatio;
    public static Camera CameraMain { get { return cam; } }
    static Camera cam;
    static Camera backgroundCam;

	void Awake () {
        activate();
	}

    public static void resetZAxis()
    {
        //Debug.Log("reset" + GlobalVars.Instance.mainCamerZ);
        cam.transform.position = new Vector3(0, 0, -10);
        cam.fieldOfView = 60.0f;
    }

    public void activate()
    {
        //Debug.Log("Activate");
        //Screen.fullScreen = Screen.fullScreen;
        _landscapeModeOnly = landscapeModeOnly;
        cam = camera;
        if (!cam)
        {
            cam = Camera.main;
            //Debug.Log ("Setting the main camera " + cam.name);
        }
        else
        {
            //Debug.Log ("Setting the main camera " + cam.name);
        }

        if (!cam)
        {
            //Debug.LogError ("No camera available");
            return;
        }
        wantedAspectRatio = _wantedAspectRatio;
        SetCamera();

        cam.transparencySortMode = TransparencySortMode.Orthographic;
    }

	void FixedUpdate()
	{
		// check the screen height and witdh
        //Debug.LogError(cam.aspect);
		if ((Screen.height != lastScreenHeight) || (Screen.width != lastScreenWidth) || (Screen.fullScreen != lastfullscreen)) 
		{
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;
			lastfullscreen = Screen.fullScreen;
            //Debug.LogError("aspect changed" + cam.aspect + " _ " + 16.0f/9.0f);
            StartCoroutine(SetCamera());
		}
	}

    IEnumerator SetCamera() {
        yield return new WaitForFixedUpdate();
		float currentAspectRatio = 0.0f;
        //if(Screen.orientation == ScreenOrientation.LandscapeRight ||
        //    Screen.orientation == ScreenOrientation.LandscapeLeft) {
        //    Debug.Log ("Landscape detected...");
        //    currentAspectRatio = (float)Screen.width / Screen.height;
        //}
        //else {
        //    Debug.Log ("Portrait detected...?");
        //    if(Screen.height >  Screen.width && _landscapeModeOnly) {
        //        currentAspectRatio = (float)Screen.height / Screen.width;
        //    }
        //    else {
        //        currentAspectRatio = (float)Screen.width / Screen.height;
        //    }
        //}
        currentAspectRatio = (float)Screen.width / Screen.height;
        // If the current aspect ratio is already approximately equal to the desired aspect ratio,
        // use a full-screen Rect (in case it was set to something else previously)

		// Debug.Log ("currentAspectRatio = " + currentAspectRatio + ", wantedAspectRatio = " + wantedAspectRatio);

        //if ((!Screen.fullScreen) && !currentAspectRatio.ToString().Equals(wantedAspectRatio.ToString()))
        //{
        //    //cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        //    //if (backgroundCam)
        //    //{
        //    //    Destroy(backgroundCam.gameObject);
        //    //}
        //    //return;
        //}
        // Pillarbox
        if (currentAspectRatio > wantedAspectRatio) {
            //Debug.LogError("Pillar");
            //Debug.LogError(currentAspectRatio);
            float inset = 1.0f - wantedAspectRatio/currentAspectRatio;
            cam.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);        
        }
        // Letterbox
        else {
            //Debug.LogError("Letter");
            float inset = 1.0f - currentAspectRatio/wantedAspectRatio;
            cam.rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
        }
        if (!backgroundCam) {
            // Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
            backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).camera;
            backgroundCam.depth = -3;
            // backgroundCam.clearFlags = CameraClearFlags.SolidColor;
            backgroundCam.clearFlags = CameraClearFlags.Skybox;
            backgroundCam.backgroundColor = Color.black;
            backgroundCam.cullingMask = 0;
        }
    }

	public static int screenHeight {
		get {
			return (int)(Screen.height * cam.rect.height);
		}
	}

	public static int screenWidth {
		get {
			return (int)(Screen.width * cam.rect.width);
		}
	}

	public static int xOffset {
		get {
			return (int)(Screen.width * cam.rect.x);
		}
	}

	public static int yOffset {
		get {
			return (int)(Screen.height * cam.rect.y);
		}
	}

	public static Rect screenRect {
		get {
			return new Rect(cam.rect.x * Screen.width, cam.rect.y * Screen.height, cam.rect.width * Screen.width, cam.rect.height * Screen.height);
		}
	}

	public static Vector3 mousePosition {
		get {
			Vector3 mousePos = Input.mousePosition;
			mousePos.y -= (int)(cam.rect.y * Screen.height);
			mousePos.x -= (int)(cam.rect.x * Screen.width);
			return mousePos;
		}
	}

	public static Vector2 guiMousePosition {
		get {
			Vector2 mousePos = Event.current.mousePosition;
			mousePos.y = Mathf.Clamp(mousePos.y, cam.rect.y * Screen.height, cam.rect.y * Screen.height + cam.rect.height * Screen.height);
			mousePos.x = Mathf.Clamp(mousePos.x, cam.rect.x * Screen.width, cam.rect.x * Screen.width + cam.rect.width * Screen.width);
			return mousePos;
		}
	}
}
