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

    public static void MergeMeshes(GameObject container)
    {
        MeshFilter[] mesh_filters = container.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[mesh_filters.Length];

        for (int i = 0; i < mesh_filters.Length; ++i)
        {
            combine[i].mesh = mesh_filters[i].sharedMesh;
            combine[i].transform = mesh_filters[i].transform.localToWorldMatrix;
            //mesh_filters[i].gameObject.SetActive(false);
        }

        /* Prepare new merge Mesh */

        /* Update container mesh */
        container.GetComponent<MeshFilter>().mesh.Clear();
        container.GetComponent<MeshFilter>().mesh = new Mesh();
        container.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        container.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        //container.SetActive(true);
    }
}

