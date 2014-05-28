using UnityEngine;
using System.Collections;

public class PukeBall : MonoBehaviour {

    private Colorable colorable_;
    private GameObject look_;

    void Awake()
    {
        colorable_ = gameObject.GetComponentInChildren<Colorable>();
        look_ = gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    void Update()
    {
        float rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg + 180.0f;

        look_.transform.rotation = Quaternion.AngleAxis(rotation, new Vector3(0.0f, 0.0f, 1.0f));
//        transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, rigidbody2D.velocity.x, rigidbody2D.velocity.y));
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag.Equals("LevelObject"))
        {
            coll.gameObject.GetComponentInChildren<Pukeable>().recievePuke(colorable_.colorString, gameObject);
        }
    }
}
