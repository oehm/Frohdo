using UnityEngine;
using System.Collections;

public class Vanishable : MonoBehaviour {

    public bool AffectCollider{get;set;}

    private Colorable colorable_;
    private Collider2D[] colliders_;

    private bool lastFrameEqualBG_;

    private float animationTimeCount_;
    bool fadingIn_;

	// Use this for initialization
	void Start () {
        colorable_ = gameObject.GetComponentInChildren<Colorable>();
        colliders_ = gameObject.GetComponentsInChildren<Collider2D>();

        animationTimeCount_ = GlobalVars.Instance.animationTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
        bool equalBG =  colorable_.colorString.Equals(SceneManager.Instance.background.GetComponentInChildren<Colorable>().colorString);

        if (equalBG != lastFrameEqualBG_)
        {

            animationTimeCount_ = 0.0f;
            fadingIn_ = !equalBG;

            if (AffectCollider)
            {
                foreach (Collider2D collider in colliders_)
                {
                    collider.enabled = !equalBG;
                }
            }

        }

        if (animationTimeCount_ < GlobalVars.Instance.animationTime)
        {
            animationTimeCount_ += Time.deltaTime;

            Color color = gameObject.GetComponentInChildren<Renderer>().material.color;
            if (fadingIn_)
            {

                color.a = 0.5f + animationTimeCount_ / GlobalVars.Instance.animationTime / 2;
            }
            else
            {
                color.a = 1.0f - animationTimeCount_ / GlobalVars.Instance.animationTime / 2;
            }
            gameObject.GetComponentInChildren<Renderer>().material.color = color;
        }
        else if (animationTimeCount_ > GlobalVars.Instance.animationTime)
        {
            animationTimeCount_ = GlobalVars.Instance.animationTime;
            Color color = gameObject.GetComponentInChildren<Renderer>().material.color;
            if (fadingIn_)
            {
                color.a = 1.0f;
            }
            else
            {
                color.a = 0.5f;
            }
            gameObject.GetComponentInChildren<Renderer>().material.color = color;
        }

        lastFrameEqualBG_ = equalBG;
    }
}
