// CameraAspectRatio
// By Nicolas Varchavsky @ Interatica (www.interatica.com)
// Date: March 27th, 2009
// Version: 1.0

//Translated to C#  and adaped by Tobias Pretzl


using UnityEngine;
using System.Collections;

public class ForceAspectRatio : MonoBehaviour
{

    void Awake()
    {
        changeRatio();
    }

    public void changeRatio()
    {
        float sw = GlobalVars.curResX;
        float sh = GlobalVars.curResY;


        float th = sw * (GlobalVars.aspecY / GlobalVars.aspecX);

        float ptw = 1.0f;
        float pth = 1.0f;

        float tx = 0.0f;
        float ty = 0.0f;
        float half = 0.0f;

        pth = th / sh;

        if (pth > 1.0f)
        {
            float tw = sh * (GlobalVars.aspecX / GlobalVars.aspecY);
            ptw = tw / sw;

            half = (1.0f - ptw) / 2.0f;

            tx = half;
        }
        else
        {
            half = (1.0f - pth) / 2.0f;

            ty = half;
        }

        Rect r = new Rect(tx, ty, ptw, pth);
        camera.rect = r;
    }
}
