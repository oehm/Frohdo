using UnityEngine;
using System.Collections;

public class Colorable : MonoBehaviour {

    public string colorString{ get{ return color_;}}

    private string color_;

    public void colorIn(string color)
    {
        Color colorO;
        try{
             colorO = LevelObjectController.Instance.GetColor(color);
        }
        catch
        {
            Debug.Log("LevelObject can not be coloured in: " + color);
            return;
        }
        gameObject.GetComponent<Renderer>().material.color = colorO;
        color_ = color;
    }
}
