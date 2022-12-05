using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class LSystemParams {
    public String _axiom;
    public Dictionary<char, string> _rules;
    public Dictionary<String, float> _constants;

    public LSystemParams(String axiom, Dictionary<char, string> rules, Dictionary<String, float> constants)
    {
        _axiom = axiom;
        _rules = rules;
        _constants = constants;
    }
}

public class LSystem : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    [SerializeField] private GameObject branch;

    [Header("TREE PARAMETERS")]
    [SerializeField] private int number_of_iterations = 1;
    

    /*====== PRIVATE ======*/
    private LSystemParams tree_params;
    private List<Instruction> axiom = new List<Instruction>();
    private Dictionary<char, List<Instruction>> rules = new Dictionary<char, List<Instruction>>();
    private Dictionary<String, float> constants;
    private List<Instruction> pattern = new List<Instruction>();
    private Stack<TransformInfos> transformStack = new Stack<TransformInfos>();
    private GameObject Tree = null;

    // Start is called before the first frame update
    void Start()
    {
        String string_axiom = "!(wr)F(l)[&(a0)!(wr)!(r2)B]+(d)!(wr)!(r1)A";
        Dictionary<char, string> string_rules = new Dictionary<char, string>
        {
            {'A', "F(l)[&(a0)!(wr)!(r2)B]+(d)!(wr)!(r1)A"},
            {'B', "F(l)[-(a2)$!(wr)!(r2)C]!(wr)!(r1)C"},
            {'C', "F(l)[+(a2)$!(wr)!(r2)B]!(wr)!(r1)B"}
        };
        constants = new Dictionary<String, float>()
        {
            {"r1", 0.9F},
            {"r2", 0.7F},
            {"a0", 45.0F},
            {"a2", 45.0F},
            {"d", 92.5F},
            {"wr", 0.707F},
        };

        tree_params = new LSystemParams(
            string_axiom,
            string_rules,
            constants
        );

        axiom = TraduceStringExpression(tree_params._axiom);
        TraduceRules(tree_params._rules, ref rules);

        GeneratePattern();
        Draw();
    }

    private void OnValidate() {
        Draw();
    }

    private List<Instruction> TraduceStringExpression(String string_expression)
    {
        List<Instruction> instructions = new List<Instruction>();

        for(int char_idx = 0; char_idx < string_expression.Length; ++char_idx)
        {
            switch(string_expression[char_idx])
            {
                case '|':
                    instructions.Add(new Instruction('|', "180"));
                    break;

                default:
                    Instruction new_instruction = new Instruction(string_expression[char_idx]);
                    
                    if(char_idx+1 < string_expression.Length && string_expression[char_idx+1] == '(')
                    {
                        String string_value = "";
                        int count = char_idx + 2;

                        while(string_expression[count] != ')')
                        {
                            string_value += string_expression[count];
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

    private void TraduceRules(Dictionary<char, String> string_rules, ref Dictionary<char, List<Instruction>> algorithmic_rules) {
        foreach(KeyValuePair<char,string> rule in string_rules)
        {
            String string_rule = string_rules[rule.Key];
            List<Instruction> instructions = TraduceStringExpression(string_rule);
            algorithmic_rules.Add(rule.Key, instructions);
        }
    }

    private void GeneratePattern() {
        List<Instruction> tmp_pattern = new List<Instruction>();
        pattern.AddRange(axiom);

        for(int i = 0; i < number_of_iterations; ++i)
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
        Destroy(Tree);
        Tree = new GameObject("Tree");
        float length = 10;
        float width = 1;

        foreach (Instruction i in pattern)
        {
            switch(i._name)
            {
                case 'F':
                    float value = i._value == "l" ? length : GetInstructionValue(i._value);
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * value);

                    GameObject treeSegment = Instantiate(
                        branch,
                        initialPosition,
                        transform.rotation,
                        Tree.transform
                    );
                    treeSegment.transform.localScale = new Vector3(width, value * 0.5F, width);
                    
                    break;

                case 'A':
                    break;
                
                case 'B':
                    break;
                
                case 'C':
                    break;
                    
                case '+':
                    transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.down);
                    break;
                    
                case '-':
                    transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.up);
                    break;
                
                case '&':
                    transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.right);
                    break;
                    
                case '^':
                    transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.left);
                    break;

                case '>':
                    transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.forward);
                    break;
                    
                case '<':
                    transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.back);
                    break;

                case '|':
                    transform.rotation *= Quaternion.AngleAxis(GetInstructionValue(i._value), Vector3.up);
                    break;

                case '$':
                    
                    transform.rotation *= Quaternion.FromToRotation(transform.up, Vector3.up);
                    break;

                case '!':
                    switch(i._value)
                    {
                        case "wr":
                            width *= constants["wr"];
                            break;
                        
                        case "r1":
                            length *= constants["r1"];
                            break;

                        case "r2":
                            length *= constants["r2"];
                            break;
                    }
                    break;
                    
                case '[':
                    // We save the current position
                    transformStack.Push(new TransformInfos()
                    {
                        position = transform.position,
                        rotation = transform.rotation,
                        length = length,
                        width = width,
                    });
                    break;
                    
                case ']':
                    // We go back to the previous position saved
                    TransformInfos ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    length = ti.length;
                    width = ti.width;
                    break;
                    
                default:
                    Debug.Log("Invalid L-Tree operation");
                    break;
            }
        }
    }
    
}
