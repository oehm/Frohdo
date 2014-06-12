using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugDrawCollider : MonoBehaviour {

    PolygonCollider2D[] colliders;

    List<Color> colcolors;

    GameObject character;
        // Use this for initialization
	void Start () {
        colliders = gameObject.GetComponentsInChildren<PolygonCollider2D>();
        colcolors = new List<Color>();
        character = gameObject;
        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log(Random.value);
            colcolors.Add(new Color(Random.value, Random.value, Random.value));
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 charpos = character.transform.position;
        //Debug.Log("COlliders: " + colliders.Length);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector2[] points = colliders[i].points;
            for (int j = 0; j < points.Length; j++)
            {
                Debug.DrawLine(new Vector3(charpos.x + points[j].x, charpos.y + points[j].y, 0), new Vector3(charpos.x + (points[(j + 1) % (points.Length)].x), charpos.y + (points[(j + 1) % (points.Length)].y), 0), colcolors[i]);
            }
        }
	}
}
