using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Poplar")]
public class PoplarBase : LSystemBase
{
    PoplarBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.2F;
        start_length = 5.0F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a0)\"(2.5)B]+(d)\"(4.0)A";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            { 'A', "!(" + Helpers.convert_float_to_string(start_width) + ")F(l)[&(a0)\"(2.5)B]+(d)\"(4.0)A" },
            { 'B', "!(" + Helpers.convert_float_to_string(start_width) + ")F(l)[>(a2)\"(2.5)$C]\"(4.0)C" },
            { 'C', "!(" + Helpers.convert_float_to_string(start_width) + ")F(l)[<(a2)\"(2.5)$B]\"(4.0)B" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"a0",  45.0F   },
            {"a2",  -45.0F   },
            {"d",   137.5F  },
            {"lr",  1.19F  },
            {"wr",  1.35F   },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };
    }
}
