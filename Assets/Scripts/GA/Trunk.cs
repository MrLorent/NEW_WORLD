using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GA/Attribut/Trunk")]
public class Trunk : Attribut
{
    [SerializeField]
    private TrunksList _trunks_list;

    [SerializeField]
    private LSystemBase _LSystem_base;

    protected override void Awake()
    {
        base.Awake();
        _type = AttributType.TRUNK;

        _LSystem_base = _trunks_list.options[(int)(((_stats.temperature + _stats.humidity) / 2) * _trunks_list.options.Count / 100)];
    }
}
