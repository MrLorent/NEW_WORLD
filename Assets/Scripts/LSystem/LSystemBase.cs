using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LSystemBase : ScriptableObject
{
    [Header("Start Parameters")]
    [SerializeField]
    public string axiom;

    [SerializeField]
    public Dictionary<char, string> rules;

    [SerializeField]
    public Dictionary<string, float> constants;

    [SerializeField]
    public float start_width;

    [SerializeField]
    public float start_length;

    [Header("Tropism parmeters")]
    [SerializeField]
    public bool tropism;
}
