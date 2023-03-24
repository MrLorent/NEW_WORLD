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

    public virtual float elasticity(float width)
    {
        return 10.0F;
    }

    public virtual float get_foliage_scale_X(int age)
    {
        return age >= 3 ? 1.0F : 0.0F;
    }

    public virtual float get_foliage_scale_Y(int age)
    {
        return age >= 2 ? 1.0F : 0.0F;
    }

    public virtual float get_foliage_scale_Z(int age)
    {
        return age >= 1 ? 1.0F : 0.0F;
    }
}
