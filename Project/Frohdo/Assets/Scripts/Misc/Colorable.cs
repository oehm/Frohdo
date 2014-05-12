using UnityEngine;
using System.Collections;

public class Colorable : MonoBehaviour {
    public string color_;

    public void colorIn(string color)
    {
        gameObject.GetComponent<Renderer>().material.color = LevelObjectController.Instance.GetColor(color);
        color_ = color;
    }
}
