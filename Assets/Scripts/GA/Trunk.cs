using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GA/Attribut/Trunk")]
public class Trunk : Attribut
{
    [SerializeField]
    static private TrunksList _trunks_list;

    [SerializeField]
    public LSystemBase lsystem_base { get; private set; }

    private void Awake()
    {
        _trunks_list = AttributsLists.Instance.trunks_list;
        _type = AttributType.TRUNK;
    }

    public override void set_randomly()
    {
        lsystem_base = _trunks_list.options[UnityEngine.Random.Range(0, _trunks_list.options.Count - 1)];
    }
}
