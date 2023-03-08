using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(menuName = "GA/Attribut/Bark")]
public class Bark : Attribut
{
    [SerializeField]
    private Material _material;

    protected override void Awake()
    {
        _type = AttributType.BARK;
    }
}
