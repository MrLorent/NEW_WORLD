using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LSystemBase : ScriptableObject
{
    [Header("Start Parameters")]
    public string _axiom;

    public Dictionary<char, string> _rules;

    public Dictionary<string, float> _constants;

    public float _start_width;

    public float _start_length;

    [Header("Tropism parmeters")]
    public bool _tropism;
}
