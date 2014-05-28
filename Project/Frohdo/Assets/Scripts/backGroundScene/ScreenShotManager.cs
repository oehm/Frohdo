using UnityEngine;
using System.Collections;
using System.IO;

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
	
	public Texture2D takeScreenShot(string path = null)
    {
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
        
        //sort Cameras
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
        
        //Render them
        RenderTexture.active = renderTexture;
        for (int i = 0; i < cams.Length;i++ )
        {
            CopyAspectRatio camAspect = camsSorted[i].gameObject.GetComponentInChildren<CopyAspectRatio>();
            if(camAspect)
            {
                //Force Update so Aspect ratio is correct!
                camAspect.forceUpdate();
            }
            
            
            camsSorted[i].targetTexture = renderTexture;

            camsSorted[i].Render();

            texture.ReadPixels(new Rect(0, 0, imgWidth, imgHeight), 0, 0);
            texture.Apply();

            camsSorted[i].targetTexture = null;


        }
        RenderTexture.active = null;
        
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        return texture;
    }
}
