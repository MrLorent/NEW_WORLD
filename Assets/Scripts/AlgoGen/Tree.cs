using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    //--PRIVATE--
    [HideInInspector] public Material material;

    //--PUBLIC--
    public float fitnessScore = 1.0f;
    public Color color = new Color(1.0f, 1.0f, 1.0f);


    void Awake()
    {
        
        //material = gameObject.GetComponent<Renderer>().material;
    }

    public void SetColor(Color color)
    {
        this.color = color;
        //material.color = color;
    }
}
