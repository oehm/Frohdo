using UnityEngine;
using System.Collections;

public class Colorable : MonoBehaviour {

    public string colorString
    {
        get { return color_; }
        set
        {
            try
            {
                colorObject_ = LevelObjectController.Instance.GetColor(value);
            }
            catch
            {
                Debug.Log("LevelObject can not be coloured in: " + value);
                return;
            }
            gameObject.GetComponentInChildren<Renderer>().material.color = colorObject_;
            color_ = value;
        }
    }

    private string color_;
    private Color colorObject_;

    private Color previousColorObject_;
    private float animationTimeCount_;

    public void Start()
    {
        animationTimeCount_ = GlobalVars.Instance.animationTime;
    }

    public void colorIn(string color)
    {

        previousColorObject_ = colorObject_;
        try{
            colorObject_ = LevelObjectController.Instance.GetColor(color);
        }
        catch
        {
            Debug.Log("LevelObject can not be coloured in: " + color);
            return;
        }
        //gameObject.GetComponent<Renderer>().material.color = colorO;
        animationTimeCount_ = 0.0f;
        color_ = color;
    }

    void Update()
    {
        if (animationTimeCount_ < GlobalVars.Instance.animationTime)
        {
            animationTimeCount_ += Time.deltaTime;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(previousColorObject_, colorObject_, animationTimeCount_ / GlobalVars.Instance.animationTime);
        }
        //else if (animationTimeCount_ > GlobalVars.Instance.animationTime)
        //{
        //    animationTimeCount_ = GlobalVars.Instance.animationTime;
        //    gameObject.GetComponent<Renderer>().material.color = colorObject_;
        //}
    }
}
