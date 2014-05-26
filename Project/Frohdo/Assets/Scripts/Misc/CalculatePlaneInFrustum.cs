using UnityEngine;
using System.Collections;

public class CalculatePlaneInFrustum : MonoBehaviour
{
    //return val: leftDown, rightDown, rightUp, leftUp

    public static Vector2[] getPlaneSizes(Vector2 levelSize, Camera camera)
    {
        Vector2[] sizes = new Vector2[GlobalVars.Instance.LayerCount];

        //Calculate currentview and add width+height half to the layersize if wished


        float initViewHeight = 2.0f * (GlobalVars.Instance.mainCamerZ + GlobalVars.Instance.layerZPos[GlobalVars.Instance.playLayer]) * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float initViewWidth = initViewHeight * camera.aspect;

        float levelCamdistance = levelSize.y / (2.0f * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        float aspect = levelSize.x / levelSize.y;

        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            float frustumHeight = 2.0f * (levelCamdistance + GlobalVars.Instance.layerZPos[i]) * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * aspect;

            sizes[i] = new Vector2(frustumWidth, frustumHeight);

            if (i != GlobalVars.Instance.playLayer)
            {
                //sizes[i].x -= initViewWidth / 1.75f;
                sizes[i].x -= initViewWidth / 1.5f;
                sizes[i].y -= initViewHeight / 2.0f;
                //sizes[i].y -= initViewHeight / 1.75f;
            }

            sizes[i].x += sizes[i].x * GlobalVars.Instance.layerParallax[i].x;
            sizes[i].y += sizes[i].y * GlobalVars.Instance.layerParallax[i].y;

            sizes[i].x = Mathf.RoundToInt(sizes[i].x / 2.0f) * 2;
            sizes[i].y = Mathf.RoundToInt(sizes[i].y / 2.0f) * 2;
        }
        return sizes;
    }
}
