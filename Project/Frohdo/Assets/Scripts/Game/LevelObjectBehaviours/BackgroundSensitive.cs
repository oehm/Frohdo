using UnityEngine;
using System.Collections;

public class BackgroundSensitive : MonoBehaviour {

    public bool AffectCollider{get;set;}

    private Colorable colorable_;
    private Collider2D[] colliders_;

    private bool enabled_ = false;

	// Use this for initialization
	void Start () {
        colorable_ = gameObject.GetComponentInChildren<Colorable>();
        colliders_ = gameObject.GetComponentsInChildren<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (AffectCollider)
        {
            foreach (Collider2D collider in colliders_)
            {
                collider.enabled = !colorable_.colorString.Equals(SceneManager.Instance.background.GetComponent<Colorable>().colorString);
            }
        }
	}
}
