using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Weeping Willow")]
public class WeepingWillowBase : LSystemBase
{
    WeepingWillowBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.1F;
        start_length = 1.0F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a1)+(d1)!(wr)\"(lr)A][&(a2)+(d2)!(wr)\"(lr)A]";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "F(l)[&(a1)+(d1)!(wr)\"(lr)A][&(a2)+(d2)!(wr)\"(lr)A]Z" },
            {'Y', "" },
            {'Z', "Y" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"a1",   30.0F   },
            {"a2",   -10F   },
            {"d1",  137.0F  },
            {"d2",  -90.0F  },
            {"lg",  1.109F  },
            {"wg",  1.109F  },
            {"wr",   0.707F   },
            {"lr",   0.9F   },
            {"min_w",  0.01F },
            {"Tx",  -0.02F  },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.27F   },
        };

        /*====== TROPISM ======*/
        tropism = true;
    }

    public override float elasticity(float width)
    {
        return 2.0F / width;
    }
}
