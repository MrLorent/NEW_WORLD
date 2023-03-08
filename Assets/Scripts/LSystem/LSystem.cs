using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LSystem : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    [SerializeField]
    private GameObject turtle_mesh;
    
    [SerializeField]
    private GameObject branch_mesh;
    [SerializeField]
    private GameObject fruit_mesh;

    [Header("CONTAINERS")]
    [SerializeField]
    private GameObject trunk_container;
    [SerializeField]
    private GameObject foliage_container;
    [SerializeField]
    private GameObject fruits_container;

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
            Stack<Vector2> transform_history = new Stack<Vector2>();
            float current_width = 0;
            float current_length = 0;
            // For each character in our current string,
            // We check if there is an evolution rule we
            // need to apply
            foreach (Instruction instruct in pattern)
            {
                if (rules.ContainsKey(instruct._name))
                {
                    tmp_pattern.AddRange(rules[instruct._name]);
                }
                else
                {
                    Instruction new_instruct = new Instruction(instruct);

                    switch (instruct._name)
                    {
                        case 'F':
                            if (instruct._value != "l")
                            {
                                current_length = GetInstructionValue(instruct._value) * GetInstructionValue("lg");
                                new_instruct._value = Helpers.convert_float_to_string(current_length);
                            }
                            break;


                        case '!':
                            if(constants.ContainsKey(instruct._value))
                            {
                                current_width *= constants[instruct._value];
                                new_instruct._value = Helpers.convert_float_to_string(current_width);
                            }
                            else
                            {
                                current_width = GetInstructionValue(instruct._value) * GetInstructionValue("wg");
                                new_instruct._value = Helpers.convert_float_to_string(current_width);
                            }
                            break;

                        case '"':
                            if (constants.ContainsKey(instruct._value))
                            {
                                current_length *= constants[instruct._value];
                                new_instruct._value = Helpers.convert_float_to_string(current_length);
                            }
                            else
                            {
                                current_length = GetInstructionValue(instruct._value) * GetInstructionValue("lg");
                                new_instruct._value = Helpers.convert_float_to_string(current_length);
                            }
                            break;

                        case '[':
                            // We save the current position
                            transform_history.Push(new Vector2(
                                current_width,
                                current_length
                            ));
                            break;

                        case ']':
                            // We go back to the previous position saved
                            Vector2 ti = transform_history.Pop();
                            current_width = ti.x;
                            current_length = ti.y;
                            break;

                        default:
                            break;
                    }

                    tmp_pattern.Add(new_instruct);
                }
            }

            // We store this need evolution iteration
            // in our tree pattern
            pattern = tmp_pattern;
            tmp_pattern = new List<Instruction>();
        }
        string line = "";
        foreach(Instruction i in pattern)
        {
            line += (i._name == '[' || i._name == ']') ? i._name : (i._name + "(" + i._value + ")");
        }
        Debug.Log(line);
    }

    private float GetInstructionValue(String value)
    {
        Debug.Log(value);
        return constants.ContainsKey(value) ? constants[value] : float.Parse(value, CultureInfo.InvariantCulture);
    }

    private void Draw()
    {
        trunk_container.transform.DestroyChildren();
        foliage_container.transform.DestroyChildren();
        fruits_container.transform.DestroyChildren();

        GameObject turtle = Instantiate(
            turtle_mesh,
            Vector3.zero,
            Quaternion.identity,
            transform
        );

        Stack<TransformInfos> transform_history = new Stack<TransformInfos>();
        float current_width = 0;
        float current_length = 0;

        foreach (Instruction i in pattern)
        {
            switch (i._name)
            {
                case 'F':
                    float value = i._value == "l" ? current_length : GetInstructionValue(i._value);
                    Vector3 initial_position = turtle.transform.position;

                    if (lsystem_base.tropism)
                    {
                        Vector3 tropism = new Vector3(constants["Tx"], constants["Ty"], constants["Tz"]);
                        Vector3 rotation_axis = Vector3.Cross(turtle.transform.up, tropism);
                        float stimulus_strenght = Vector3.Cross(turtle.transform.up, tropism).magnitude;
                        float susceptability = 10.0F; // should be constants["e"]
                        turtle.transform.Rotate(
                            rotation_axis * (susceptability * stimulus_strenght),
                            Space.World
                        );
                    }

                    turtle.transform.Translate(Vector3.up * value);


                    GameObject branch = Instantiate(
                        branch_mesh,
                        initial_position,
                        turtle.transform.rotation,
                        trunk_container.transform
                    );
                    
                    branch.transform.localScale = new Vector3(current_width, value * 0.5F, current_width);
                    break;

                case 'A':
                    Instantiate(
                        fruit_mesh,
                        turtle.transform.position,
                        turtle.transform.rotation,
                        fruits_container.transform
                    );
                    break;

                case 'B':
                    Instantiate(
                        fruit_mesh,
                        turtle.transform.position,
                        turtle.transform.rotation,
                        fruits_container.transform
                    );
                    break;

                case 'C':
                    Instantiate(
                        fruit_mesh,
                        turtle.transform.position,
                        turtle.transform.rotation,
                        fruits_container.transform
                    );
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
                    int rotation_direction = axis.y < 0 ? -1 : 1;
                    turtle.transform.Rotate(0.0F, angle * rotation_direction, 0.0F, Space.Self);
                    break;

                case '!':
                    if (float.TryParse(i._value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float new_width))
                    {
                        current_width = new_width;
                    }
                    break;

                case '"':
                    if (float.TryParse(i._value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float new_length))
                    {
                        current_length = new_length;
                    }
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

        trunk_container.transform.MergeMeshes();
    }
}
