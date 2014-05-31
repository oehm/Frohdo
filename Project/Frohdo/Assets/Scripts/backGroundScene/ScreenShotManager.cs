using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ScreenShotManager : MonoBehaviour {


    private static ScreenShotManager instance = null;

    public static ScreenShotManager Instance
    {
        get
        {
            return instance;
        }
    }


    public int imgWidth;
    public int imgHeight;

    public List<Texture2D> screens = new List<Texture2D>();
    
    private Texture2D texture;
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

    public void addScreenshot(Texture2D screen)
    {
        screens.Add(screen);
    }

    public void reset()
    {
        texture = null;
        screens.Clear();
    }
	
	public Texture2D takeScreenShot(string path = null)
    {
        bool automaticScreen = path == null ? true : false; //if path is null .. then it is a custom level screenshot and we save it to a list.
        if(path == null)
        {
            path = SceneManager.Instance.levelToLoad.thumbpath;
        }
        else
        {
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        
        texture = new Texture2D(imgWidth, imgHeight);
        renderTexture = new RenderTexture(imgWidth, imgHeight, 32);

        Camera[] cams = GameObject.Find("SceneObjects").GetComponentsInChildren<Camera>();
        Camera[] camsSorted = new Camera[cams.Length];

        //force draw of character
        GameObject character = GameObject.Find("Character");
        if(character != null)
        {
            Animator animator = character.GetComponentInChildren<Animator>();
            if(animator != null)
            {
                animator.Update(0.1f);
            }
        }
        
        //sort Cameras so later camera with least depth get rendered first
        float lastDepth = 999.0f;
        float minDepth = -99.0f;
        Camera minDepthCam = null;

        for(int i=0; i< cams.Length; i++)
        {
            foreach(Camera c in cams)
            {
                if(c.depth < lastDepth && c.depth > minDepth)
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
        for (int i = 0; i < cams.Length;i++ )
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

            texture.ReadPixels(new Rect(0, 0, imgWidth, imgHeight), 0, 0);
            texture.Apply();

            camsSorted[i].targetTexture = null;
        }
        RenderTexture.active = null;

        if (automaticScreen)
        {
            screens.Add(texture);
        }
        else
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }
        return texture;
    }

    public void saveScreenshot(Texture2D screen, string path = null)
    {
        if (path == null) path = SceneManager.Instance.levelToLoad.thumbpath;
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }
}
