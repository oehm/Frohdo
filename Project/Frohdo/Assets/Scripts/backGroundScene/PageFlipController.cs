using UnityEngine;
using System.Collections;

public class PageFlipController : MonoBehaviour {

    private static PageFlipController instance = null;
    public static PageFlipController Instance
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
            Destroy(gameObject);
        }
        instance = this;

        isFlipping_ = false;
    }

    public GameObject pagePrefab_;

    private GameObject pageInstance_;

    bool isFlipping_;

    public void startFlip()
    {
        if (!isFlipping_)
        {
            isFlipping_ = true;

            //ForceAspectRatio.CameraMain.transform.position = new Vector3(0.0f, 0.0f, GlobalVars.Instance.mainCamerZ);
            //ForceAspectRatio.CameraMain.fieldOfView = GlobalVars.Instance.mainCamerFOV;

            pageInstance_ = GameObject.Instantiate(pagePrefab_) as GameObject;
            pageInstance_.transform.position = new Vector3(0.0f, 0.0f, -2.0f);
        }
        else
        {
            Debug.Log("cant start pageflip while already flippin");
        }
    }

    public void endFlip()
    {
        if (isFlipping_)
        {
            isFlipping_ = false;


            Destroy(pageInstance_);
        }
        else
        {
            Debug.Log("cant end pageflip when not started flippin");
        }
    }
}
