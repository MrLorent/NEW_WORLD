using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Frustum : MonoBehaviour
{
    /*====== PUBLIC ======*/
    public float radius = 1;
    public int circle_subdivisions = 30;

    /*====== PRIVATE ======*/
    private Mesh mesh;
    private Vector3[] vao;
    private int[] ibo;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = "Cone";

        this.GetComponent<MeshFilter>().mesh = mesh;

        CreateFrustum();
    }

    // OnValidate is called at each public input change
    private void OnValidate() {
        CreateFrustum();
    }

    private void CreateFrustum() {
        vao = ComputeCircleVao(new Vector3(0,0,0), radius, circle_subdivisions);
        ibo = ComputeCircleIbo(circle_subdivisions);

        mesh.Clear();
        mesh.vertices = vao;
        mesh.triangles = ibo;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.Optimize();
    }

    private Vector3[] ComputeCircleVao(Vector3 origin, float radius, int nb_subdivisions) {
        List<Vector3> vao = new List<Vector3>();
        float step_angle = (2*Mathf.PI)/nb_subdivisions;

        for(int i = 0; i < nb_subdivisions; ++i){
            float current_angle = step_angle * i;
            Vector3 translate_vector = new Vector3(
                Mathf.Cos(current_angle) * radius,  // x coordinate
                0,                                  // y coordinate
                Mathf.Sin(current_angle) * radius   // z coordinate
            );

            Vector3 new_vertex = origin + translate_vector;

            vao.Add(new_vertex);
        }

        return vao.ToArray();
    }

    private int[] ComputeCircleIbo(int nb_vertices) {
        List<int> ibo = new List<int>();
        int nb_triangles = nb_vertices - 2;

        for(int i = 0; i < nb_triangles; ++i){
            ibo.Add(0);
            ibo.Add(i+1);
            ibo.Add(i+2);
        }

        return ibo.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
