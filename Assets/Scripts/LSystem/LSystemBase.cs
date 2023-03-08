using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LSystemBase : ScriptableObject
{
    [Header("Start Parameters")]
    public string axiom;

    public Dictionary<char, string> rules;

    public Dictionary<string, float> constants;

    public float start_width;

    public float start_length;

    [Header("Tropism parmeters")]
    public bool tropism;
}
