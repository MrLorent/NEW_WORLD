using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = "Cone";

        this.GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = GeneratesVertices();
        mesh.triangles = GeneratesTriangles();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private Vector3[] GeneratesVertices() {

        return new Vector3[]
        {
            // BOTTOM
            new Vector3(-1,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,-1),
            new Vector3(-1,0,-1),

            // TOP
            new Vector3(-1,2,1),
            new Vector3(1,2,1),
            new Vector3(1,2,-1),
            new Vector3(-1,2,-1),
        };
    }

    private int[] GeneratesTriangles() {
        return new int[]
        {
            // BOTTOM
            1,0,2,
            2,0,3,

            // TOP
            4,5,6,
            4,6,7,

        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
