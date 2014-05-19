using UnityEngine;
using System.Collections;

public class Coatable : MonoBehaviour {

    public Material coatedMaterial;
    private Material normalMaterial;

    void Awake()
    {
        normalMaterial = gameObject.renderer.material;
    }
    public void coat()
    {
        gameObject.renderer.material = coatedMaterial;
        coatedMaterial.color = normalMaterial.color;
    }
    public void uncoat()
    {
        gameObject.renderer.material = normalMaterial;
        normalMaterial.color = coatedMaterial.color;
    }
}