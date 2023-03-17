using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomType
{
    DESERT,
    MOUNTAIN,
    PLAIN,
    SWAMP
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
    private BiomType biom_type;

    [SerializeField]
    [Range(-50.0f, 50.0f)]
    private float temperature;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float humidity_rate;

    public Environment get_caracteristics()
    {
        return new Environment(biom_type, temperature, humidity_rate);
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
