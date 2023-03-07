using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Oak")]
public class OakBase : LSystemBase
{
    //[Header("Start Parameters")]
    [SerializeField]
    public string axiom => _axiom = "A";

    [SerializeField]
    public float start_width => _start_width = 1;

    [SerializeField]
    public float start_length => _start_length = 1;

    // [Header("Tropism parmeters")]
    [SerializeField]
    public bool tropism => _tropism = false;

    OakBase()
    {
        /*====== RULES ======*/
        _rules = new Dictionary<char, string>()
        {
            {'A', "F(l)+(a1)[&(o1)\"(r1)!(q1)A][+(a2)&(o1)\"(r1)!(q1)A][-(a2)&(o1)\"(r1)!(q1)A]"},
        };

        /*==== CONSTANTS ====*/
        _constants = new Dictionary<string, float>()
        {
            {"a1",  30.0F   },
            {"a2",  120.0F  },
            {"o1",  45.0F   },
            {"r1",  0.75F   },
            {"q1",  0.5F    },
        };
    }
}