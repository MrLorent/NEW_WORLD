using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Frustum : MonoBehaviour
{
    /*====== PUBLIC ======*/
    public float height = 1;
    public float top_radius = 0.9F;
    public float base_radius = 1F;
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

        draw();
    }

    // OnValidate is called at each public input change
    private void OnValidate() {
        draw();
    }

    private void draw() {
        ComputeFrustumVao();
        ComputeFrustumIbo();

        mesh.Clear();
        mesh.vertices = vao;
        mesh.triangles = ibo;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.Optimize();
    }

    private void ComputeFrustumVao()
    {
        // COMPUTE BASE VERTICES
        List<Vector3> vertices = ComputeCircleVertices(transform.position, base_radius, circle_subdivisions);
    
        // ADD TOP VERTICES
        Vector3 top_origin = transform.position + new Vector3(0, height, 0);
        vertices.AddRange(ComputeCircleVertices(top_origin, top_radius, circle_subdivisions));
        
        vao = vertices.ToArray();
    }

    private List<Vector3> ComputeCircleVertices(Vector3 origin, float radius, int nb_subdivisions) {
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

        return vao;
    }

    private void ComputeFrustumIbo() {
        List<int> indices = new List<int>();
        int nb_vertices = vao.Length;
        int nb_vertices_per_circle = nb_vertices/2;
        int nb_triangles =  nb_vertices_per_circle - 2;

        // DRAW BASE
        for(int i = 0; i < nb_triangles; ++i){
            indices.Add(0);
            indices.Add(i + 1);
            indices.Add(i + 2);
        }

        // DRAW TOP
        for(int i = nb_vertices_per_circle; i < nb_vertices_per_circle + nb_triangles; ++i){
            indices.Add(nb_vertices_per_circle);
            indices.Add(i + 2);
            indices.Add(i + 1);
        }

        // DRAW SIDES
        for(int i = 0; i < nb_vertices_per_circle; ++i){
            int next_index = (i + 1) % nb_vertices_per_circle;

            indices.Add(i);                                     // BOTTOM LEFT
            indices.Add( nb_vertices_per_circle + next_index);  // TOP RIGHT
            indices.Add(next_index);                            // BOTTOM RIGHT


            indices.Add(i);                                     // BOTTOM LEFT
            indices.Add(i + nb_vertices_per_circle);            // TOP LEFT
            indices.Add(nb_vertices_per_circle + next_index);   // TOP RIGHT
        }

        ibo = indices.ToArray();
    }
}
