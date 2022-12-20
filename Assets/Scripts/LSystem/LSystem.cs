using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LSystem : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    [SerializeField]
    private GameObject turtle_mesh;
    
    [SerializeField]
    private GameObject branch_mesh;

    [Header("L-SYSTEM PARAMETERS")]
    [SerializeField]
    private int iterations = 1;

    [SerializeField]
    private LSystemBase lsystem_base;

    /*====== PRIVATE ======*/
    private List<Instruction> axiom_instructions;
    private Dictionary<char, List<Instruction>> rules;
    private Dictionary<String, float> constants;
    private List<Instruction> pattern;

    // Start is called before the first frame update
    private void Start()
    {
        InititalizeAxiom();
        Init();
    }

    public void Init()
    {
        GeneratePattern();
        Draw();
    }

    private void InititalizeAxiom()
    {
        // TRADUCE AXIOM
        axiom_instructions = GetInstructionsFrom(lsystem_base.axiom);

        // RULES
        rules = new Dictionary<char, List<Instruction>>();
        foreach(KeyValuePair<char, string> rule in lsystem_base.rules)
        {
            rules.Add(rule.Key, GetInstructionsFrom(rule.Value));
        }

        // CONSTANTS
        constants = lsystem_base.constants;
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
        transform.DestroyChildren();

        GameObject turtle = Instantiate(
            turtle_mesh,
            Vector3.zero,
            Quaternion.identity,
            transform
        );

        Stack<TransformInfos> transform_history = new Stack<TransformInfos>();
        float current_width = lsystem_base.start_width;
        float current_length = lsystem_base.start_length;

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
                    Vector3 new_right = Vector3.Cross(turtle.transform.up, Vector3.down);
                    Quaternion delta = Quaternion.FromToRotation(turtle.transform.right, new_right);
                    delta.ToAngleAxis(out float angle, out Vector3 axis);
                    int direction = axis.y < 0 ? -1 : 1; 
                    turtle.transform.Rotate(0.0F, angle * direction, 0.0F, Space.Self);
                    break;

                case '!':
                    current_width *= constants[i._value];
                    break;
                
                case '"':
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

        turtle.transform.Destroy();

        Helpers.MergeMeshes(this.gameObject);

        transform.DestroyChildren();
    }
}
