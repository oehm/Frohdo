﻿using UnityEngine;
using System.Collections;

public class RenderGameObjectToTexture : MonoBehaviour
{
    public Camera renderCam;

    private Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        targetPos = renderCam.transform.position + new Vector3(0, 0, -7.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Texture renderGameObjectToTexture(GameObject obj, int width, int height)
    {

        GameObject toRender = (GameObject)Instantiate(obj, targetPos, Quaternion.identity);
        toRender.transform.Rotate(Vector3.up, 180.0f);

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