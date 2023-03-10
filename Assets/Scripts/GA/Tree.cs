using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    /*======== PUBLIC ========*/
    public Material material;
    public float fitnessScore = 1.0f;
    public Color color = new Color(1.0f, 1.0f, 1.0f);

    /*======== PRIVATE ========*/
    [Header("LSYSTEM")]
    private LSystem _lsystem;

    [Header("GA ATTRIBUTS")]
    private Trunk _trunk;
    private Bark _bark;

    private void Start()
    {
        _lsystem = GetComponent<LSystem>();

        _trunk  =   GetComponent<Trunk>();
        _bark   =   GetComponent<Bark>();

        init();
    }

    public void init()
    {
        _trunk.set_randomly();
        _lsystem.Init(_trunk.lsystem_base);
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }
}
