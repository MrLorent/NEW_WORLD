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

    public static void Destroy(GameObject gameObject)
    {
        if (Application.isPlaying)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }

    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) Destroy(child.gameObject);
    }

    public static void MergeMeshes(GameObject container)
    {
        MeshFilter[] mesh_filters = container.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[mesh_filters.Length];

        for (int i = 0; i < mesh_filters.Length; ++i)
        {
            combine[i].mesh = mesh_filters[i].sharedMesh;
            combine[i].transform = mesh_filters[i].transform.localToWorldMatrix;
            mesh_filters[i].gameObject.SetActive(false);
        }

        container.GetComponent<MeshFilter>().mesh.Clear();
        container.GetComponent<MeshFilter>().mesh = new Mesh();
        container.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        container.SetActive(true);
    }
}

