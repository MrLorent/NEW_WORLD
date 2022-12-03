using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Globalization;

public class TransformInfos {
    public Vector3 position;
    public Quaternion rotation;
}

public class Instruction {
    public char _name;
    public float _value;

    public Instruction(char name)
    {
        _name = name;
        _value = 0.0F;
    }

    public Instruction(char name, float value)
    {
        _name = name;
        _value = value;
    }
}

public class LSystemParams {
    public String _axiom;
    public Dictionary<char, string> _rules;
    public Dictionary<char, float> _constants;

    public LSystemParams(String axiom, Dictionary<char, string> rules, Dictionary<char, float> constants)
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
    [SerializeField] private int number_of_iterations = 2;
    

    /*====== PRIVATE ======*/
    private Stack<TransformInfos> transformStack;
    private LSystemParams tree_params;
    private List<Instruction> axiom;
    private Dictionary<char, List<Instruction>> rules;
    private List<Instruction> pattern = new List<Instruction>();
    private GameObject Tree = null;

    // Start is called before the first frame update
    void Start()
    {
        String string_axiom = "X";
        Dictionary<char, string> string_rules = new Dictionary<char, string>
        {
            {'X', "[F(5)[&(30)F(5)X][+(120)&(30)F(5)X][-(120)&(30)F(5)X]]"},
            {'F', "F(5)F(5)"}
        };
        // String string_axiom = "!(1)F(200)>(45)A";
        // Dictionary<char, string> string_rules = new Dictionary<char, string>
        // {
        //     {'A', "[!(1.732)F(50)[&(18.95)F(50)A]>(94.74)[&(18.95)F(50)A]>(132.63)[&(18.95))F(50)A]"},
        //     {'F', "F(1.109)"},
        //     {'!', "!(1.732)"},
        // };
        Dictionary<char, float> constants = new Dictionary<char, float>();

        tree_params = new LSystemParams(
            string_axiom,
            string_rules,
            constants
        );

        transformStack = new Stack<TransformInfos>();
        axiom = TraduceStringExpression(tree_params._axiom);
        rules = new Dictionary<char, List<Instruction>>();
        
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
                    instructions.Add(new Instruction('|', 180));
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

                        new_instruction._value = float.Parse(string_value, CultureInfo.InvariantCulture);
                        
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

    private void Draw()
    {
        Destroy(Tree);
        Tree = new GameObject("Tree");

        foreach (Instruction i in pattern)
        {
            switch(i._name)
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * i._value);

                    GameObject treeSegment = Instantiate(
                        branch,
                        initialPosition,
                        transform.rotation,
                        Tree.transform
                    );
                    treeSegment.transform.localScale = new Vector3(1, i._value * 0.5F, 1);
                    
                    break;

                case 'X':
                    break;
                    
                case '+':
                    transform.rotation *= Quaternion.AngleAxis(i._value, Vector3.down);
                    break;
                    
                case '-':
                    transform.rotation *= Quaternion.AngleAxis(i._value, Vector3.up);
                    break;
                
                case '&':
                    transform.rotation *= Quaternion.AngleAxis(i._value, Vector3.right);
                    break;
                    
                case '^':
                    transform.rotation *= Quaternion.AngleAxis(i._value, Vector3.left);
                    break;

                case '>':
                    transform.rotation *= Quaternion.AngleAxis(i._value, Vector3.forward);
                    break;
                    
                case '<':
                    transform.rotation *= Quaternion.AngleAxis(i._value, Vector3.back);
                    break;

                case '|':
                    transform.rotation *= Quaternion.AngleAxis(i._value, Vector3.up);
                    break;
                    
                case '[':
                    // We save the current position
                    transformStack.Push(new TransformInfos()
                    {
                        position = transform.position,
                        rotation = transform.rotation,
                    });
                    break;
                    
                case ']':
                    // We go back to the previous position saved
                    TransformInfos ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;
                    
                default:
                    Debug.Log("Invalid L-Tree operation");
                    break;
            }
        }
    }
    
}
