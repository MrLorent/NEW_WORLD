using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biom : MonoBehaviour
{
    public string name;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Biom other_biom))
        {
            Debug.Log( name + " Biom is colliding with " + other_biom.name);
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
