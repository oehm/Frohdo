using UnityEngine;
using System.Collections;

public class WideBox : LevelElements {

    private GameObject pre;
    public Material mat;

    void Start()
    {
        Mesh mMesh = new Mesh();
        Vector3[] veterx = new Vector3[4];
        veterx[0] = new Vector3(-0.5f, -0.5f, 0);
        veterx[1] = new Vector3(-0.5f, 0.5f, 0);
        veterx[2] = new Vector3(1.5f, 0.5f, 0);
        veterx[3] = new Vector3(1.5f, -0.5f, 0);

        int[] indices = new int[6];
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 3;
        indices[4] = 0;
        indices[5] = 2;

        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(-0.5f, -0.5f);
        uvs[1] = new Vector2(-0.5f, 1.5f);
        uvs[2] = new Vector2(0.5f, 1.5f);
        uvs[3] = new Vector2(-0.5f, -0.5f);

        mMesh.vertices = veterx;
        mMesh.SetIndices(indices, MeshTopology.Triangles, 0);



        mMesh.uv = uvs;

        pre = new GameObject();

        pre.AddComponent<MeshFilter>();
        pre.GetComponent<MeshFilter>().mesh = mMesh;

        pre.AddComponent<MeshRenderer>();
        pre.GetComponent<MeshRenderer>().material = mat;
        pre.GetComponent<MeshRenderer>().enabled = false;

        pre.AddComponent<MeshCollider>();
        pre.GetComponent<MeshCollider>().enabled = false;

        prefab = pre;
        
        base.Init();
        quadMat[0][0] = true;
        quadMat[1][0] = true;
    }
}
