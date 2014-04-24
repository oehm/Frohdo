using UnityEngine;
using System.Collections;

public class AddCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PolygonCollider2D collider1 = gameObject.AddComponent<PolygonCollider2D>();

        Vector2[] vertices1 = {  new Vector2(0.5f, 0.5f),
                                new Vector2(0.5f, -0.6f),
                                new Vector2(0.2f, -0.9f),
                                new Vector2(-0.2f, -0.9f),
                                new Vector2(-0.5f, -0.6f),
                                new Vector2(-0.5f, 0.5f)

                             };

        collider1.points = vertices1;

        PolygonCollider2D collider2 = gameObject.AddComponent<PolygonCollider2D>();

        Vector2[] vertices2 = { new Vector2(0.2f, -0.9f),
                                new Vector2(0.0f, -1.0f),
                                new Vector2(-0.2f, -0.9f)

                             };

        collider2.points = vertices2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
