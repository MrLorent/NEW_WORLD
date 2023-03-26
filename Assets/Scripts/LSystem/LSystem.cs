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
    private FoliageShape foliage_shape;

    [Header("CONTAINERS")]
    [SerializeField]
    private Transform trunk_container;
    [SerializeField]
    private Transform foliage_container;

    [Header("L-SYSTEM PARAMETERS")]
    [SerializeField]
    public int iterations = 1;

    [SerializeField]
    private LSystemBase lsystem_base = null;

    /*====== PRIVATE ======*/
    private List<Instruction> axiom_instructions;
    private float min_width;
    private Dictionary<char, List<Instruction>> rules;
    private Dictionary<String, float> constants;
    private List<Instruction> pattern;


    private void Start()
    {
        if (lsystem_base != null && foliage_shape != null) Init();
    }

    public void Init()
    {
        InititalizeAxiom();
        Draw();
    }

    public void Init(LSystemBase LS_base, FoliageShape fs)
    {
        lsystem_base = LS_base;
        foliage_shape = fs;
        InititalizeAxiom();
        Draw();
    }

    /// <summary>
    /// Traduce L-System strings axiom, rules and constants from LSystem_base object
    /// into code usable objects
    /// </summary>
    ///
    private void InititalizeAxiom()
    {
        // TRADUCE AXIOM
        axiom_instructions = GetInstructionsFrom(lsystem_base.axiom);

        // START PARAMETERS
        min_width = lsystem_base.constants["min_w"];

        // RULES
        rules = new Dictionary<char, List<Instruction>>();
        foreach(KeyValuePair<char, string> rule in lsystem_base.rules)
        {
            rules.Add(rule.Key, GetInstructionsFrom(rule.Value));
        }

        // CONSTANTS
        constants = lsystem_base.constants;
    }

    /// <summary>
    /// Traduce L-System strings axiom or rule from LSystem_base object
    /// into code usable "instruction" struct
    /// </summary>
    ///
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


    /// <summary>
    /// Generate the L-System series of instructions current LSystem axiom
    /// for the number of iterations given by Tree.cs script
    /// </summary>
    ///
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
                            // Deal with instanciation values in axiom
                            if (instruct._value != "l")
                            {
                                current_length = GetInstructionValue(instruct._value) * GetInstructionValue("lg");
                                new_instruct._value = Helpers.convert_float_to_string(current_length);
                            }
                            break;


                        case '!':
                            // Shrink width
                            if(constants.ContainsKey(instruct._value))
                            {
                                current_width *= constants[instruct._value];
                                current_width = current_width < min_width ? min_width : current_width;
                                new_instruct._value = Helpers.convert_float_to_string(current_width);
                            }
                            // Grow width
                            else
                            {
                                current_width = GetInstructionValue(instruct._value) * GetInstructionValue("wg");
                                new_instruct._value = Helpers.convert_float_to_string(current_width);
                            }
                            break;

                        case '"':
                            // Shrink length
                            if (constants.ContainsKey(instruct._value))
                            {
                                current_length *= constants[instruct._value];
                                new_instruct._value = Helpers.convert_float_to_string(current_length);
                            }
                            // Grow length
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

            // We store this new evolution iteration
            // in our tree pattern and prepare for a next one
            pattern = tmp_pattern;
            tmp_pattern = new List<Instruction>();
        }
    }

    /// <summary>
    /// Print the final L-System strings series of instructions
    /// </summary>
    ///
    private void print_pattern()
    {
        string line = "";
        foreach (Instruction i in pattern)
        {
            line += (i._name == '[' || i._name == ']') ? i._name : (i._name + "(" + i._value + ")");
        }
        Debug.Log(line);
    }

    /// <summary>
    /// Determine if instruction value is a derteminated constant
    /// of LSystem, or a given float value and return the right value
    /// </summary>
    ///
    private float GetInstructionValue(String value)
    {
        return constants.ContainsKey(value) ? constants[value] : float.Parse(value, CultureInfo.InvariantCulture);
    }


    /// <summary>
    /// Draw the Plant/Tree in Unity by reading the pattern
    /// generated by GeneratePattern() method
    /// </summary>
    ///
    public void Draw()
    {
        GeneratePattern();

        // Clean unity scene containers
        trunk_container.DestroyChildren();
        foliage_container.DestroyChildren();
        
        // Create the turtle use to draw LSystem
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
                // Forward instruction
                case 'F':
                    float value = i._value == "l" ? current_length : GetInstructionValue(i._value);
                    Vector3 initial_position = turtle.transform.position;

                    if (lsystem_base.tropism)
                    {
                        Vector3 tropism = new Vector3(constants["Tx"], constants["Ty"], constants["Tz"]); // Tropism direction
                        Vector3 rotation_axis = Vector3.Cross(turtle.transform.up, tropism);
                        float stimulus_strenght = rotation_axis.magnitude;
                        float elasticity = lsystem_base.elasticity(current_width); // Elasticity of the branch defined by current LSystemBase
                        turtle.transform.Rotate(
                            rotation_axis.normalized * (elasticity * stimulus_strenght),
                            Space.World
                        );
                    }

                    turtle.transform.Translate(Vector3.up * value);


                    GameObject branch = Instantiate(
                        branch_mesh,
                        initial_position,
                        turtle.transform.rotation,
                        trunk_container
                    );
                    
                    branch.transform.localScale = new Vector3(current_width, value * 0.5F, current_width);
                    break;

                // Rules markers
                case 'A':
                    break;

                case 'B':
                    break;

                case 'C':
                    break;

                case 'D':
                    break;

                // Old foliage marker
                case 'X':
                    float scale_x = lsystem_base.get_foliage_scale_X(iterations);

                    if(scale_x > 0)
                    {
                        GameObject foliage_x = Instantiate(
                            foliage_shape.foliage_prefab,
                            turtle.transform.position,
                            foliage_shape.get_orientation(turtle.transform.rotation),
                            foliage_container
                        );

                        // Scale foliage according to its "age"
                        foliage_x.transform.localScale = new Vector3(scale_x, scale_x, scale_x);
                    }
                    break;

                // Middle age foliage marker
                case 'Y':
                    float scale_y = lsystem_base.get_foliage_scale_Y(iterations);

                    if (scale_y > 0)
                    {
                        GameObject foliage_y = Instantiate(
                            foliage_shape.foliage_prefab,
                            turtle.transform.position,
                            foliage_shape.get_orientation(turtle.transform.rotation),
                            foliage_container
                        );

                        // Scale foliage according to its "age"
                        foliage_y.transform.localScale = new Vector3(scale_y, scale_y, scale_y);
                    }
                    break;

                // Young foliage marker
                case 'Z':
                    float scale_z = lsystem_base.get_foliage_scale_Z(iterations);

                    if (scale_z > 0)
                    {
                        GameObject foliage_z = Instantiate(
                            foliage_shape.foliage_prefab,
                            turtle.transform.position,
                            foliage_shape.get_orientation(turtle.transform.rotation),
                            foliage_container
                        );

                        // Scale foliage according to its "age"
                        foliage_z.transform.localScale = new Vector3(scale_z, scale_z, scale_z);
                    }
                    break;

                // Clockwise Y axis rotation instruction
                case '+':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.down);
                    break;

                // Counterclockwise Y axis rotation instruction
                case '-':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.up);
                    break;

                // Clockwise X axis rotation instruction
                case '&':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.right);
                    break;

                // Counterclockwise X axis rotation instruction
                case '^':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.left);
                    break;

                // Clockwise Z axis rotation instruction
                case '>':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.forward);
                    break;

                // Counterclockwise Z axis rotation instruction
                case '<':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.back);
                    break;

                // 180 degres Y axis rotation instruction
                case '|':
                    turtle.transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.up);
                    break;

                // Make turtle right vector parallele to world ground plan instruction
                case '$':
                    Vector3 new_right = Vector3.Cross(turtle.transform.up, Vector3.down);
                    Quaternion delta = Quaternion.FromToRotation(turtle.transform.right, new_right);
                    delta.ToAngleAxis(out float angle, out Vector3 axis);
                    int rotation_direction = axis.y < 0 ? -1 : 1;
                    turtle.transform.Rotate(0.0F, angle * rotation_direction, 0.0F, Space.Self);
                    break;

                // Width set instruction
                case '!':
                    if (float.TryParse(i._value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float new_width))
                    {
                        current_width = new_width;
                    }
                    break;

                // Length set instruction
                case '"':
                    if (float.TryParse(i._value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float new_length))
                    {
                        current_length = new_length;
                    }
                    break;

                // Save current transform instruction
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

                // Go back to previous transform saved instruction
                case ']':
                    TransformInfos ti = transform_history.Pop();
                    turtle.transform.position = ti.position;
                    turtle.transform.rotation = ti.rotation;
                    current_length = ti.length;
                    current_width = ti.width;
                    break;

                default:
                    Debug.Log("Invalid L-System transform : " + i._name);
                    break;
            }
        }

        // Remove turtle
        turtle.transform.Destroy();

        // Merge Unity meshes for optimisation
        trunk_container.merge_children_meshes();
        foliage_container.merge_children_meshes();
    }
}
