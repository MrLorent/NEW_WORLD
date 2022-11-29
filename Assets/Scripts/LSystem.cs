using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

public class TransformInfos {
    public Vector3 position;
    public Quaternion rotation;
}

[ExecuteInEditMode]
public class LSystem : MonoBehaviour
{
    /*====== PUBLIC ======*/
    [Header("PEFABS")]
    [SerializeField] private GameObject Branch;

    [Header("TREE PARAMETERS")]
    [SerializeField] private int number_of_iterations = 2;
    [SerializeField] private float length = 10;
    [SerializeField] private float x_angle = 30;
    [SerializeField] private float y_angle = 120;
    [SerializeField] private float z_angle = 30;

    /*====== PRIVATE ======*/
    private Stack<TransformInfos> transformStack;
    private Dictionary<char, string> rules;
    private const string AXIOM = "X";

    private GameObject TreeParent;
    private GameObject Tree = null;
    private string currentString = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        TreeParent = new GameObject("Tree");
        TreeParent.transform.parent = transform;
        transformStack = new Stack<TransformInfos>();
        rules = new Dictionary<char, string>
        {
            {'X', "[F[&FX][+&FX][-&FX]]"},
            {'F', "FF"}
        };

        Generate();
    }

    private void OnValidate() {
        Generate();    
    }

    private void Generate()
    {
        Destroy(Tree);

        Tree = Instantiate(TreeParent);

        currentString = AXIOM;

        StringBuilder sb = new StringBuilder();

        for(int i = 0; i < number_of_iterations; ++i)
        {
            // For each character in our current string,
            // We check if there is an evolution rule we
            // need to apply 
            foreach(char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            // We store this need evolution iteration
            // in our tree pattern
            currentString = sb.ToString();
            sb = new StringBuilder();
        }

        foreach (char c in currentString)
        {
            switch(c)
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * length);

                    GameObject treeSegment = Instantiate(Branch);
                    treeSegment.transform.parent = Tree.transform;
                    treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    break;

                case 'X':
                    break;
                    
                case '+':
                    transform.Rotate(Vector3.up * -y_angle);
                    break;
                    
                case '-':
                    transform.Rotate(Vector3.up * y_angle);
                    break;
                
                case '&':
                    transform.Rotate(Vector3.right * x_angle);
                    break;
                    
                case '^':
                    transform.Rotate(Vector3.right * -x_angle);
                    break;

                case '>':
                    transform.Rotate(Vector3.forward * z_angle);
                    break;
                    
                case '<':
                    transform.Rotate(Vector3.forward * -z_angle);
                    break;

                case '|':
                    transform.Rotate(Vector3.up * 180);
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
                    throw new InvalidOperationException("Invalid L-Tree operation");
            }
        }
    }
    
}
