using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LSystem1 : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    [SerializeField] private GameObject branch;

    [Header("L-SYSTEM BASE")]
    [SerializeField] private string axiom   = "!(wr)F(l)[&(a0)!(wr)!(r2)B]+(d)!(wr)!(r1)A";
    [SerializeField] private string rule_A  = "F(l)[&(a0)!(wr)!(r2)B]+(d)!(wr)!(r1)A";
    [SerializeField] private string rule_B  = "F(l)[-(a2)$!(wr)!(r2)C]!(wr)!(r1)C";
    [SerializeField] private string rule_C  = "F(l)[+(a2)$!(wr)!(r2)B]!(wr)!(r1)B";

    [Header("L-SYSTEM PARAMETERS")]
    [SerializeField] private int iterations = 1;
    
    [Range(0f, 1f)]
    [SerializeField] private float r1 = 0.9F;

    [Range(0f, 1f)]
    [SerializeField] private float r2 = 0.7F;

    [Range(0f, 1f)]
    [SerializeField] private float wr = 0.707F;

    [Range(0f, 360f)]
    [SerializeField] private float a0 = 45F;

    [Range(0f, 360f)]
    [SerializeField] private float a2 = 0.9F;

    [Range(0f, 360f)]
    [SerializeField] private float d = 92.5F;


    /*====== PRIVATE ======*/
    private List<Instruction> axiom_instructions = new List<Instruction>();
    private Dictionary<char, List<Instruction>> rules = new Dictionary<char, List<Instruction>>();
    private Dictionary<String, float> constants;
    private List<Instruction> pattern = new List<Instruction>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject test = Instantiate(branch);
        test.transform.parent = transform;

    }

    void OnValidate()
    {
        Debug.Log("refresh");
        Draw();
    }

    void Draw()
    {
        DestroyChildren();
    }

    void DestroyChildren()
    {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}
