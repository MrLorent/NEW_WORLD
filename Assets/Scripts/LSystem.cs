using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

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

public class LSystem : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    [SerializeField] private GameObject branch;

    [Header("TREE PARAMETERS")]
    [SerializeField] private int number_of_iterations = 2;
    

    /*====== PRIVATE ======*/
    private Stack<TransformInfos> transformStack;
    private Dictionary<char, string> string_rules;
    private Dictionary<char, List<Instruction>> rules;
    private const string AXIOM = "X";
    private List<Instruction> pattern = new List<Instruction>();

    private GameObject Tree = null;
    private string currentString = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        transformStack = new Stack<TransformInfos>();
        string_rules = new Dictionary<char, string>
        {
            {'X', "[F(5)[&(30)F(5)X][+(120)&(30)F(5)X][-(120)&(30)F(5)X]]"},
            {'F', "F(5)F(5)"}
        };
        rules = new Dictionary<char, List<Instruction>>();
        
        TraduceRules();
        GeneratePattern();
        Draw();
    }

    private void OnValidate() {
        Draw();
    }

    private void TraduceRules() {
        foreach(KeyValuePair<char,string> rule in string_rules)
        {
            String string_rule = string_rules[rule.Key];
            List<Instruction> instructions = new List<Instruction>();

            for(int char_idx = 0; char_idx < string_rule.Length; ++char_idx)
            {
                switch(char_idx)
                {
                    case '|':
                        instructions.Add(new Instruction('|', 180));
                        break;

                    default:
                        Instruction new_instruction = new Instruction(string_rule[char_idx]);
                        
                        if(char_idx+1 < string_rule.Length && string_rule[char_idx+1] == '(')
                        {
                            String string_value = "";
                            int count = char_idx + 2;

                            while(string_rule[count] != ')')
                            {
                                string_value += string_rule[count];
                                count++;
                            }

                            new_instruction._value = float.Parse(string_value);
                            
                            char_idx = count;
                        }
                        instructions.Add(new_instruction);
                        break;
                }
            }
            rules.Add(rule.Key, instructions);
        }
    }

    private void GeneratePattern() {
        List<Instruction> tmp_pattern = new List<Instruction>();
        pattern.Add(new Instruction('X'));

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
                        transform.position,
                        transform.rotation,
                        Tree.transform
                    );
                    treeSegment.transform.localScale = new Vector3(1, i._value, 1);
                    
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
                        rotation = transform.rotation
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
