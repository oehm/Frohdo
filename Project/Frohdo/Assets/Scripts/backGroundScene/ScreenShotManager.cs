using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class ScreenShotManager : MonoBehaviour {


    private static ScreenShotManager instance = null;

    public static ScreenShotManager Instance
    {
        get
        {
            return instance;
        }
    }
    public GameObject camera;


    public int imgWidth;
    public int imgHeight;

    public int screenshotcount;
    private int manuallyaddedScreens;

    private List<RenderTexture> screens = new List<RenderTexture>();
    private Hashtable renderedscreens = new Hashtable();
    private List<int> alreadysavedscreens = new List<int>();
    
    //private Texture2D texture;
    private RenderTexture renderTexture;


    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public Texture2D getScreenShot(int index)
    {
        if (!renderedscreens.ContainsKey(index))
        {
            //Debug.Log("rendered screen on Position: " + index);
            RenderTexture tmp = screens[index-manuallyaddedScreens];
            Texture2D texture = new Texture2D(tmp.width, tmp.height);

            RenderTexture.active = tmp;

            texture.ReadPixels(new Rect(0, 0, imgWidth, imgHeight), 0, 0);
            texture.Apply();

            RenderTexture.active = null;
            renderedscreens.Add(index, texture);
        }
        return (Texture2D)renderedscreens[index];
    }

    public void addCurrentThumb(Texture2D screen)
    {
        renderedscreens.Add(screenshotcount,screen);
        screenshotcount++;
        manuallyaddedScreens++;
    }

    public void reset()
    {
        StopCoroutine("ScreenShotCoroutine");
        screens.Clear();
        renderedscreens.Clear();
        alreadysavedscreens.Clear();
        screenshotcount = 0;
        manuallyaddedScreens = 0;
    }
	
	public void takeScreenShot(bool delay, string path = null)
    {
        ScreenShotCoroutineObj o = new ScreenShotCoroutineObj(delay, path);
        StartCoroutine("ScreenShotCoroutine", o);
    }
    public bool isThumbnail(int index)//returns if given texture is thumbnail of current level (if the thumb got added with addCurrentHumb function)
    {
        return index < manuallyaddedScreens;
    }

    private IEnumerator ScreenShotCoroutine(ScreenShotCoroutineObj o)
    {
        yield return new WaitForSeconds(0.1f);
        bool automaticScreen = o.path == null ? true : false; //if path is null .. then it is a custom level screenshot and we save it to a list.
        if (o.path == null)
        {
            o.path = SceneManager.Instance.levelToLoad.thumbpath;
        }
        else
        {
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(o.path));
        }

        if (automaticScreen && o.delay) yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f)); //delay on automatic screenshots.. for better pictures. ;)
            try
            {    
        
            //texture = new Texture2D(imgWidth, imgHeight);
            renderTexture = new RenderTexture(imgWidth, imgHeight, 32);

            Camera[] cams = camera.GetComponentsInChildren<Camera>();
            Camera[] camsSorted = new Camera[cams.Length];

            //force draw of character
            GameObject character = GameObject.Find("Character");
            if (character != null)
            {
                Animator animator = character.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    animator.Update(0.1f);
                }
            }

            //sort Cameras so later camera with least depth get rendered first
            float lastDepth = 999.0f;
            float minDepth = -99.0f;
            Camera minDepthCam = null;

            for (int i = 0; i < cams.Length; i++)
            {
                foreach (Camera c in cams)
                {
                    if (c.depth < lastDepth && c.depth > minDepth)
                    {
                        lastDepth = c.depth;
                        minDepthCam = c;
                    }
                }
                minDepth = minDepthCam.depth;
                camsSorted[i] = minDepthCam;
                lastDepth = 999.0f;
            }

            //Render all cams in the same texture. Take the sorted cameras so the kind of z-order is right
            RenderTexture.active = renderTexture;
            for (int i = 0; i < cams.Length; i++)
            {
                CopyAspectRatio camAspect = camsSorted[i].gameObject.GetComponentInChildren<CopyAspectRatio>();
                if (camAspect)
                {
                    //Force Update so Aspect ratio is correct!
                    camAspect.forceUpdate();
                }

                camsSorted[i].targetTexture = renderTexture;

                //set camera rect back to normal for rendering into texture
                Rect oldRect = camsSorted[i].rect;
                camsSorted[i].rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

                camsSorted[i].Render();
                //set camerta rect back to old value
                camsSorted[i].rect = oldRect;

                camsSorted[i].targetTexture = null;
            }
            RenderTexture.active = null;

            if (automaticScreen)
            {
                screens.Add(renderTexture);
                screenshotcount++;
            }
            else
            {
                saveScreenshot(renderTexture, o.path);
            }
        }
        catch (NullReferenceException) { }
    }

    public void saveScreenshot(RenderTexture screen, string path = null)
    {
        if (path == null) path = SceneManager.Instance.levelToLoad.thumbpath;

        Texture2D texture = new Texture2D(imgWidth, imgHeight);
        RenderTexture.active = screen;

        texture.ReadPixels(new Rect(0, 0, imgWidth, imgHeight), 0, 0);
        texture.Apply();

        RenderTexture.active = null;
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

    public void saveScreenshot(int index, string path = null)
    {
        if (path == null) path = SceneManager.Instance.levelToLoad.thumbpath;
        byte[] bytes = getScreenShot(index).EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        alreadysavedscreens.Add(index);
    }

    public string getScreenShotFolderPath()
    {
        System.DateTime time = System.DateTime.Now;
        string timeAndDate = time.DayOfYear.ToString() + "_" + time.Year.ToString() + "_" + time.Hour.ToString() + "_" + time.Minute.ToString() + "_" + time.Second.ToString();
        return Application.dataPath + "/Screenshots/ScreenShot_" + timeAndDate + ".png";
    }

    public bool GotThumbAlreadySaved(int index)
    {
        return alreadysavedscreens.Contains(index);
    }
}

class ScreenShotCoroutineObj
{
    public bool delay;
    public string path = null;
    public ScreenShotCoroutineObj(bool delay, string path = null)
    {
        this.delay = delay;
        this.path = path;
    }
}
