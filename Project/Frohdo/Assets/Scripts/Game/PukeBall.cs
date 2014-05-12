using UnityEngine;
using System.Collections;

public class PukeBall : MonoBehaviour {

    private Colorable colorable_;

    void Awake()
    {
        colorable_ = gameObject.GetComponent<Colorable>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag.Equals("LevelObject"))
        {
            coll.gameObject.GetComponentInChildren<PukedBehaviour>().recievePuke(colorable_.color_, gameObject);
        }
    }
}
