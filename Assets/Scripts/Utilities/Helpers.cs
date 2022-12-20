using UnityEngine;

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
        MeshFilter[] mesh_filters = container.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[mesh_filters.Length];

        for (int i = 0; i < mesh_filters.Length; ++i)
        {
            combine[i].mesh = mesh_filters[i].sharedMesh;
            combine[i].transform = mesh_filters[i].transform.localToWorldMatrix;
        }


        /* Update container mesh */
        Mesh container_mesh = container.GetComponent<MeshFilter>().mesh;
        container_mesh.Clear();
        container_mesh = new Mesh();
        container_mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        container_mesh.CombineMeshes(combine);
        container.DestroyChildren();
    }
}

