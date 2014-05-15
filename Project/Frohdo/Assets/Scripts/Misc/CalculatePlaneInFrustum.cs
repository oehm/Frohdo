using UnityEngine;
using System.Collections;

public class CalculatePlaneInFrustum : MonoBehaviour
{
    //return val: leftDown, rightDown, rightUp, leftUp
    public static Vector3[] getPlane(float distance, Camera cam)
    {
        Vector3[] corners = new Vector3[4];

        float frustumHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * cam.aspect;

        Vector3 center = cam.transform.position + cam.transform.forward * distance;
        corners[0] = center + new Vector3(-frustumWidth / 2, -frustumHeight / 2, 0.0f);
        corners[1] = center + new Vector3(frustumWidth / 2, -frustumHeight / 2, 0.0f);
        corners[2] = center + new Vector3(frustumWidth / 2, frustumHeight / 2, 0.0f);
        corners[3] = center + new Vector3(-frustumWidth / 2, frustumHeight / 2, 0.0f);

        return corners;
    }

    public static Vector2[] getPlaneSizes(Vector2 levelSize, Camera camera)
    {
        Vector3 oldCamPos = camera.transform.position;
        camera.transform.position = new Vector3(0, 0, GlobalVars.Instance.mainCamerZ);

        Vector2 levelLeftDownCorner = new Vector2(-levelSize.x / 2, -levelSize.y / 2);

        //calulate the field of vision of the camera
        Vector3[] visonPlane = getPlane(Mathf.Abs(GlobalVars.Instance.layerZPos[GlobalVars.Instance.playLayer] - camera.transform.position.z), camera);
        //move the camera to downloeftCorner so it exectly sees the level corner of the main layer
        //float diffx = Mathf.Abs(levelLeftDownCorner.x) - Mathf.Abs(visonPlane[0].x);
        //float diffy = Mathf.Abs(levelLeftDownCorner.y) - Mathf.Abs(visonPlane[0].y);

        float diffx = levelLeftDownCorner.x;
        float diffy = levelLeftDownCorner.y;
        camera.transform.position += new Vector3(diffx, diffy, 0);
        //now calc the visonplane for the different layers
        Vector2[] sizes = new Vector2[GlobalVars.Instance.LayerCount];

        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            if (i == GlobalVars.Instance.playLayer)
            {
                levelSize.x = (int)(levelSize.x / 2) * 2;
                levelSize.y = (int)(levelSize.y / 2) * 2;
                sizes[i] = levelSize;
                continue;
            }

            Vector2 plane = new Vector2();
            visonPlane = getPlane(Mathf.Abs(GlobalVars.Instance.layerZPos[i] - camera.transform.position.z), camera);
            plane.x = Mathf.Abs(visonPlane[0].x) * 2;
            plane.x += diffx * GlobalVars.Instance.layerParallax[i].x * 2;
            plane.y = Mathf.Abs(visonPlane[0].y) * 2;
            plane.y += diffy * GlobalVars.Instance.layerParallax[i].y * 2;

            plane.x = (int)(plane.x/2)*2;
            plane.y = (int)(plane.y/2)*2;

            sizes[i] = plane;
        }
        camera.transform.position = oldCamPos;
        return sizes;
    }
}
