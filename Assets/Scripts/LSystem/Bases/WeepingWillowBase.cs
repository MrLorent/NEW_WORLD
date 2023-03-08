using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Weeping Willow")]
public class WeepingWillowBase : LSystemBase
{
    WeepingWillowBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.2F;
        start_length = 7.5F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a1)>(o1)\"(2.5)A][&(a2)>(o2)\"(2.8)A]";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {  'A', "!(" + Helpers.convert_float_to_string(start_width) + ")F(l)[&(a1)>(o1)\"(2.5)A][&(a2)>(o2)\"(2.8)A]"  },
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"a1",  27.0F   },
            {"a2",  -68.0F  },
            {"o1",  0.0F    },
            {"o2",  0.0F    },
            {"lr",  1.2F  },
            {"wr",  1.35F   },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };

        /*==== TROPISM ====*/
        tropism = false;
    }
}
