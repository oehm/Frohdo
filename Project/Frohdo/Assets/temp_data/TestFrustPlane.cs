using UnityEngine;
using System.Collections;

public class TestFrustPlane : MonoBehaviour
{

    public Camera camLevel;
    public Camera cam;

    public float dist1 = 10.0f;
    public float dist2 = 20.0f;

    private Vector3[] verts;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update() {

        verts = CalculatePlaneInFrustum.getPlane(dist2, camLevel);

        Debug.DrawLine(verts[0], verts[1]);
        Debug.DrawLine(verts[1], verts[2]);
        Debug.DrawLine(verts[2], verts[3]);
        Debug.DrawLine(verts[3], verts[0]);

        verts = CalculatePlaneInFrustum.getPlane(dist1, cam);

        Debug.DrawLine(verts[0], verts[1]);
        Debug.DrawLine(verts[1], verts[2]);
        Debug.DrawLine(verts[2], verts[3]);
        Debug.DrawLine(verts[3], verts[0]);
	}
}
