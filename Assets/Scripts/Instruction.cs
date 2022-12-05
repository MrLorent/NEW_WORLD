using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Instruction {
    public char _name;
    public String _value;

    public Instruction(char name)
    {
        _name = name;
        _value = null;
    }

    public Instruction(char name, string value)
    {
        _name = name;
        _value = value;
    }
}
