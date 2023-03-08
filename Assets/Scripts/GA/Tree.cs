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
    [Header("GA ATTRIBUTS")]
    [SerializeField]
    private Trunk _trunk;

    [SerializeField]
    private Bark _bark;

    public void SetColor(Color color)
    {
        this.color = color;
    }
}
