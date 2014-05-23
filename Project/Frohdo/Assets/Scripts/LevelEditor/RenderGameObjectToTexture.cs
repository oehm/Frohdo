using UnityEngine;
using System.Collections;

public class RenderGameObjectToTexture : MonoBehaviour
{
    public Camera renderCam;

    private Vector3 targetPos;

    void Start()
    {
        renderCam.transparencySortMode = TransparencySortMode.Orthographic;
    }

    public Texture renderGameObjectToTexture(GameObject obj, int width, int height, string color)
    {
        targetPos = renderCam.transform.localPosition + new Vector3(0, 0, -5);
        Gridable g = obj.GetComponentsInChildren<Gridable>(true)[0];
        renderCam.orthographicSize = Mathf.Max(g.height, g.width) + 0.75f;
        GameObject toRender = Instantiate(obj, targetPos, Quaternion.identity) as GameObject;

        TextMesh textMesh = new TextMesh();

        toRender.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
        if (color != null)
        {
            Colorable colorable = toRender.GetComponentInChildren<Colorable>();
            if (colorable != null)
            {
                toRender.GetComponentInChildren<Colorable>().colorString = color ;
            }
        }

        renderCam.clearFlags = CameraClearFlags.Color;
        renderCam.backgroundColor = new Color(1, 1, 1, 0);

        renderCam.DoClear();
        
        RenderTexture renderTex = new RenderTexture(width, height, 16);
        Texture2D tex2D = new Texture2D(width, height);


        RenderTexture.active = renderTex;
        renderCam.camera.targetTexture = renderTex;

        renderCam.aspect = 1.0f;//Camera.main.aspect;
        renderCam.camera.Render();

        tex2D.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        tex2D.Apply();

        RenderTexture.active = null;
        renderCam.targetTexture = null;


        DestroyImmediate(toRender);
        return tex2D;
    }
}
