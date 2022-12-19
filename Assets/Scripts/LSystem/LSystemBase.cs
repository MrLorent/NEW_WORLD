using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemBase
{
    public string axiom;
    public Dictionary<char, string> rules;
    public Dictionary<string, float> constants;

    public LSystemBase()
    {
        axiom = null;
        rules = null;
        constants = null;
    }

    public LSystemBase(string a, Dictionary<char, string> r, Dictionary<string, float> c)
    {
        axiom = a;
        rules = r;
        constants = c;
    }
}
