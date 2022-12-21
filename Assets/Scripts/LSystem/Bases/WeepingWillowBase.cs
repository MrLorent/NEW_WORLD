using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Weeping Willow")]
public class WeepingWillowBase : LSystemBase
{
    WeepingWillowBase()
    {
        /*====== AXIOM ======*/
        axiom = "F(l)+(45)A";

        /*=== AXIOM PARAMS ===*/
        start_width = 1;
        start_length = 10;

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "F(l)F(l)[&(a)!(vr)\"(lr)A]+(d1)[&(a)!(vr)\"(lr)A]+(d2)[&(a)!(vr)\"(lr)A]"},
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"d1",  94.74F  },
            {"d2",  132.63F },
            {"a",   18.85F  },
            {"lr",  0.902F  },
            {"vr",  0.577F  },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   },
        };

        /*====== OTHER ======*/
        tropism = true;
    }
}