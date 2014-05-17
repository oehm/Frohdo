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
    }

    public void unvarnish()
    {
        gameObject.renderer.material = normalMaterial;
    }
}
