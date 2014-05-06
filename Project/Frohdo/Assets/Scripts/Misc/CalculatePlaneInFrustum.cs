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

    public static Camera getLevelCam(Vector2 levelSize, Camera playCam)
    {
        Camera cam = new Camera();
        cam.CopyFrom(playCam);

        float dist = levelSize.y / 2.0f * Mathf.Tan(playCam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        cam.aspect = levelSize.y / levelSize.x;
        cam.transform.position -= new Vector3(0, 0, dist);


        return cam;
    }

    public  static Vector2[] getPlaneSizes(Vector2 levelSize, Camera camera)
    {
        Camera cam = camera;
        cam.transform.position = new Vector3(0, 0, GlobalVars.Instance.mainCamerZ);

        Vector2 levelLeftDownCorner = new Vector2(-levelSize.x / 2, -levelSize.y / 2);

        Vector2 plane1 = new Vector2();
        Vector2 plane2 = new Vector2();
        Vector2 plane4 = new Vector2();
        Vector2 plane5 = new Vector2();

        //calulate the field of vision of the camera
        Vector3[] visonPlane = getPlane(Mathf.Abs(GlobalVars.Instance.layerZPos[3] - cam.transform.position.z), cam);
        //move the camera to downloeftCorner so it exectly sees the level corner of the main layer
        float diffx = visonPlane[0].x - levelLeftDownCorner.x;
        float diffy = visonPlane[0].y - levelLeftDownCorner.y;
        cam.transform.position -= new Vector3(diffx, diffy, 0);
        //now calc the visonplane for the different layers
        //make this automated
        visonPlane = getPlane(Mathf.Abs(GlobalVars.Instance.layerZPos[0] - cam.transform.position.z), cam);
        plane1.x = Mathf.Abs(visonPlane[0].x) * 2;
        plane1.x += diffx * GlobalVars.Instance.layerParallax[0].x * 2;
        plane1.y = Mathf.Abs(visonPlane[0].y) * 2;
        plane1.y += diffy * GlobalVars.Instance.layerParallax[0].y * 2;


        visonPlane = getPlane(Mathf.Abs(GlobalVars.Instance.layerZPos[1] - cam.transform.position.z), cam);
        plane2.x = Mathf.Abs(visonPlane[0].x) * 2;
        plane2.x += diffx * GlobalVars.Instance.layerParallax[1].x * 2;
        plane2.y = Mathf.Abs(visonPlane[0].y) * 2;
        plane2.y += diffy * GlobalVars.Instance.layerParallax[1].y * 2;

        visonPlane = getPlane(Mathf.Abs(GlobalVars.Instance.layerZPos[3] - cam.transform.position.z), cam);
        plane4.x = Mathf.Abs(visonPlane[0].x) * 2;
        plane4.x += diffx * GlobalVars.Instance.layerParallax[3].x * 2;
        plane4.y = Mathf.Abs(visonPlane[0].y) * 2;
        plane4.y += diffy * GlobalVars.Instance.layerParallax[3].y * 2;

        visonPlane = getPlane(Mathf.Abs(GlobalVars.Instance.layerZPos[4] - cam.transform.position.z), cam);
        plane5.x = Mathf.Abs(visonPlane[0].x) * 2;
        plane5.x += diffx * GlobalVars.Instance.layerParallax[4].x * 2;
        plane5.y = Mathf.Abs(visonPlane[0].y) * 2;
        plane5.y += diffy * GlobalVars.Instance.layerParallax[4].y * 2;

        Vector2[] sizes = new Vector2[5];
        sizes[0] = plane1;
        sizes[1] = plane2;
        sizes[2] = levelSize;
        sizes[3] = plane4;
        sizes[4] = plane5;

        camera.transform.position = new Vector3(0,0,GlobalVars.Instance.mainCamerZ);
        return sizes;
    }
}
