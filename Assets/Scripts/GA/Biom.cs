using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomType
{
    DESERT = 0,
    MOUNTAIN = 1,
    PLAIN = 2,
    SWAMP = 3
};

public struct Environment
{
    public BiomType biom_type;
    public float temperature;
    public float humidity_rate;

    public Environment(BiomType type, float t, float h)
    {
        biom_type = type;
        temperature = t;
        humidity_rate = h;
    }
}

public class Biom : MonoBehaviour
{
    [Header("ALGO GEN PARAMS")]
    [SerializeField]
    public BiomType biom_type;

    [SerializeField]
    [Range(-50.0f, 50.0f)]
    public float temperature;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float humidity_rate;

    [Header("COLORS")]
    public Color ground_color;

    private float radius;
    
    private void Start()
    {
        radius = GetComponent<SphereCollider>().radius;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Biom other_biom))
        {
            Debug.Log( name + " Biom is colliding with " + other_biom.name);
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
