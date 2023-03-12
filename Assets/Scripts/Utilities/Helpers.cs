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
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
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

    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) child.Destroy();
    }

    public static void MergeMeshes(this Transform container)
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
        container.GetComponent<MeshFilter>().mesh.Clear();
        container.GetComponent<MeshFilter>().mesh = new Mesh();
        container.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        container.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        container.DestroyChildren();
    }

    public static string convert_float_to_string(float number)
    {
        return number.ToString("F2", CultureInfo.InvariantCulture);
    }
}

