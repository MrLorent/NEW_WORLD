using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    [SerializeField] private GameObject turtle_mesh;
    [SerializeField] private GameObject branch_mesh;

    [Header("L-SYSTEM BASE")]
    [SerializeField] private string axiom   = "F(l)[&(a0)!(wr)_(r2)B]+(d)!(wr)_(r1)A";
    [SerializeField] private string rule_A  = "F(l)[&(a0)!(wr)_(r2)B]+(d)!(wr)_(r1)A";
    [SerializeField] private string rule_B  = "F(l)[-(a2)$!(wr)_(r2)C]!(wr)_(r1)C";
    [SerializeField] private string rule_C  = "F(l)[+(a2)$!(wr)_(r2)B]!(wr)_(r1)B";

    [Header("L-SYSTEM PARAMETERS")]
    [SerializeField] private int iterations = 1;
    [SerializeField] private float start_width = 1;
    [SerializeField] private float start_length = 10;
    
    [Range(0f, 1f)]
    [SerializeField] private float r1 = 0.9F;

    [Range(0f, 1f)]
    [SerializeField] private float r2 = 0.7F;

    [Range(0f, 1f)]
    [SerializeField] private float wr = 0.707F;

    [Range(0f, 360f)]
    [SerializeField] private float a0 = 45F;

    [Range(0f, 360f)]
    [SerializeField] private float a2 = 45F;

    [Range(0f, 360f)]
    [SerializeField] private float d = 92.5F;


    /*====== PRIVATE ======*/
    private List<Instruction> axiom_instructions;
    private Dictionary<char, List<Instruction>> rules;
    private Dictionary<String, float> constants;
    private List<Instruction> pattern;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GeneratePattern();
        Draw();
    }

    void OnValidate()
    {
        Init();
        GeneratePattern();
        Draw();
    }

    private void Init()
    {
        // TRADUCE AXIOM
        axiom_instructions = GetInstructionsFrom(axiom);

        // RULES
        rules = new Dictionary<char, List<Instruction>>()
        {
            {'A', GetInstructionsFrom(rule_A)},
            {'B', GetInstructionsFrom(rule_B)},
            {'C', GetInstructionsFrom(rule_C)}
        };

        // CONSTANTS
        constants = new Dictionary<String, float>()
        {
            {"r1", r1},
            {"r2", r2},
            {"a0", a0},
            {"a2", a2},
            {"d", d},
            {"wr", wr},
        };
    }

    private List<Instruction> GetInstructionsFrom(string expression)
    {
        List<Instruction> instructions = new List<Instruction>();

        for(int char_idx = 0; char_idx < expression.Length; ++char_idx)
        {
            switch(expression[char_idx])
            {
                case '|':
                    instructions.Add(new Instruction('|', "180"));
                    break;

                default:
                    Instruction new_instruction = new Instruction(expression[char_idx]);
                    
                    if(char_idx+1 < expression.Length && expression[char_idx+1] == '(')
                    {
                        string string_value = "";
                        int count = char_idx + 2;

                        while(expression[count] != ')')
                        {
                            string_value += expression[count];
                            count++;
                        }

                        new_instruction._value = string_value;
                        
                        char_idx = count;
                    }
                    instructions.Add(new_instruction);
                    break;
            }
        }

        return instructions;
    }

    private void GeneratePattern() {
        List<Instruction> tmp_pattern = new List<Instruction>();
        pattern = axiom_instructions;

        for(int i = 0; i < iterations; ++i)
        {
            // For each character in our current string,
            // We check if there is an evolution rule we
            // need to apply 
            foreach(Instruction instruct in pattern)
            {
                if(rules.ContainsKey(instruct._name)){
                    tmp_pattern.AddRange(rules[instruct._name]);
                   
                }else{
                    tmp_pattern.Add(instruct);
                }
                
            }

            // We store this need evolution iteration
            // in our tree pattern
            pattern = tmp_pattern;
            tmp_pattern = new List<Instruction>();
        }
    }

    private float GetInstructionValue(String value)
    {
        return constants.ContainsKey(value) ? constants[value] : float.Parse(value, CultureInfo.InvariantCulture);
    }

    private void Draw()
    {
        DestroyChildren();

        GameObject turtle = Instantiate(
            turtle_mesh,
            Vector3.zero,
            Quaternion.identity,
            transform
        );

        Stack<TransformInfos> transform_history = new Stack<TransformInfos>();
        float current_width = start_width;
        float current_length = start_length;

        foreach (Instruction i in pattern)
        {
            switch(i._name)
            {
                case 'F':
                    float value = i._value == "l" ? current_length : GetInstructionValue(i._value);
                    Vector3 initial_position = turtle.transform.position;
                    turtle.transform.Translate(Vector3.up * value);

                    GameObject branch = Instantiate(
                        branch_mesh,
                        initial_position,
                        turtle.transform.rotation,
                        transform
                    );
                    branch.transform.localScale = new Vector3(current_width, value * 0.5F, current_width);
                    break;

                case 'A':
                    break;
                
                case 'B':
                    break;
                
                case 'C':
                    break;
                    
                case '+':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.down);
                    break;
                    
                case '-':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.up);
                    break;
                
                case '&':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.right);
                    break;
                    
                case '^':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.left);
                    break;

                case '>':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.forward);
                    break;
                    
                case '<':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.back);
                    break;

                case '|':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.up);
                    break;

                case '$':
                    turtle.transform.rotation *= Quaternion.FromToRotation(turtle.transform.up, Vector3.up);
                    break;

                case '!':
                    current_width *= constants[i._value];
                    break;
                
                case '_':
                    current_length *= constants[i._value];
                    break;
                    
                case '[':
                    // We save the current position
                    transform_history.Push(new TransformInfos()
                    {
                        position = turtle.transform.position,
                        rotation = turtle.transform.rotation,
                        length = current_length,
                        width = current_width,
                    });
                    break;
                    
                case ']':
                    // We go back to the previous position saved
                    TransformInfos ti = transform_history.Pop();
                    turtle.transform.position = ti.position;
                    turtle.transform.rotation = ti.rotation;
                    current_length = ti.length;
                    current_width = ti.width;
                    break;
                    
                default:
                    Debug.Log("Invalid L-Tree operation");
                    break;
            }
        }
    }

    private void DestroyChildren()
    {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}
