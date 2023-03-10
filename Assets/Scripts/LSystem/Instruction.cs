using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Instruction {
    public char _name;
    public string _value;

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

    public Instruction(Instruction i)
    {
        _name = i._name;
        _value = i._value;
    }
}
