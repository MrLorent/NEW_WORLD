using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Destroy the gameObject linked to this transform.
    /// Use it like so:
    /// <code>
    /// transform.Destroy();
    /// </code>
    /// </summary>
    ///
    public static void Destroy(this Transform t)
    {
        if (Application.isPlaying)
        {
            GameObject.Destroy(t.gameObject);
        }
        else
        {
            GameObject.DestroyImmediate(t.gameObject);
        }
    }

    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    ///
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) child.Destroy();
    }

    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.merge_children_meshes();
    /// </code>
    /// </summary>
    ///
    public static void merge_children_meshes(this Transform container)
    {
        List<MeshFilter> mesh_filters = new List<MeshFilter>(container.GetComponentsInChildren<MeshFilter>());
        CombineInstance[] combine = new CombineInstance[mesh_filters.Count];

        for (int i = 0; i < mesh_filters.Count; ++i)
        {
            if (mesh_filters[i].transform == container) continue;

            combine[i].subMeshIndex = 0;
            combine[i].mesh = mesh_filters[i].sharedMesh;
            combine[i].transform = mesh_filters[i].transform.localToWorldMatrix;
        
        }

        /* Update container mesh */
        Mesh new_mesh = new Mesh();
        new_mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        new_mesh.CombineMeshes(combine);

        if(container.GetComponent<MeshFilter>().sharedMesh != null) container.GetComponent<MeshFilter>().sharedMesh.Clear();
        container.GetComponent<MeshFilter>().sharedMesh = new_mesh;
        container.DestroyChildren();
    }

    public static string convert_float_to_string(float number)
    {
        return number.ToString("F2", CultureInfo.InvariantCulture);
    }
}

