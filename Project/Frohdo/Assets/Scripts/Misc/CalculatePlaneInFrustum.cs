using UnityEngine;
using System.Collections;

public class CalculatePlaneInFrustum : MonoBehaviour
{

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

    public static Camera getLevelCam(Vector2 levelSize, Camera playCam)
    {
        Camera cam = new Camera();
        cam.CopyFrom(playCam);

        float dist = levelSize.y / 2.0f * Mathf.Tan(playCam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        cam.aspect = levelSize.y / levelSize.x;
        cam.transform.position -= new Vector3(0,0,dist);


        return cam;
    }
}
