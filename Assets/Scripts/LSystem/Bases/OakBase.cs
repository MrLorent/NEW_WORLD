using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Oak")]
public class OakBase : LSystemBase
{
    OakBase()
    {
        /*====== AXIOM ======*/
        axiom = "A";

        /*=== AXIOM PARAMS ===*/
        start_width = 1;
        start_length = 1;

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "F(l)+(a1)[&(o1)\"(r1)!(q1)A][+(a2)&(o1)\"(r1)!(q1)A][-(a2)&(o1)\"(r1)!(q1)A]"},
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"a1",  30.0F   },
            {"a2",  120.0F  },
            {"o1",  45.0F   },
            {"r1",  0.75F   },
            {"q1",  0.5F    },
        };

        /*====== OTHER ======*/
        tropism = false;
    }
}