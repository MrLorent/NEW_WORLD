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
        start_length = 10;

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "F(l)+(a1)[&(o1)\"(r1)!(q1)A]+(a2)[&(o2)\"(r2)!(q2)A]+(a2)[&(o2)\"(r2)!(q2)A]"},
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"a1",  30.0F   },
            {"a2",  120.0F  },
            {"o1",  45.0F    },
            {"o2",  45.0F    },
            {"r1",  0.7F   },
            {"r2",  0.7F   },
            {"q1",  0.5F    },
            {"q2",  0.5F    },
            {"e",   0.4F    },
        };

        /*====== OTHER ======*/
        tropism = false;
    }
}