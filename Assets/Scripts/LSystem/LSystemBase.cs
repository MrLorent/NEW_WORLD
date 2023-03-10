using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LSystemBase : ScriptableObject
{
    [Header("Name")]
    protected string name;

    protected string id; 

    [Header("Start Parameters")]
    public string axiom;

    public Dictionary<char, string> rules;

    public Dictionary<string, float> constants;

    public float start_width;

    public float start_length;

    [Header("Tropism parmeters")]
    public bool tropism;

    public virtual float elasticity(float width)
    {
        return 10.0F;
    }
}
