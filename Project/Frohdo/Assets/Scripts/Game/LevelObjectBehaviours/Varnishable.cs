using UnityEngine;
using System.Collections;

public class Varnishable : MonoBehaviour {

    public Material varnishedMaterial;
    private Material normalMaterial;

    void Awake()
    {
        normalMaterial = gameObject.renderer.material;
    }
    public void varnish()
    {
        gameObject.renderer.material = varnishedMaterial;
        varnishedMaterial.color = normalMaterial.color;
    }
    public void unvarnish()
    {
        gameObject.renderer.material = normalMaterial;
        normalMaterial.color = varnishedMaterial.color;
    }
}