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
        start_length = 2.0F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            { 'A', "F(l)[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A" },
            { 'B', "F(l)[>(a2)$!(wr)\"(r2)C]!(wr)\"(r1)CZ" },
            { 'C', "F(l)[<(a2)$!(wr)\"(r2)B]!(wr)\"(r1)BZ" },
            {'Y', "" },
            {'Z', "Y" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.6F    },
            {"a0",  45.0F   },
            {"a2",  45.0F  },
            {"d",   137.5F  },
            {"lg",  1.08F   },
            {"wg",  1.109F   },
            {"wr",  0.707F  },
            {"min_w", 0.02F  },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };

        /*====== OTHER ======*/
        tropism = false;
    }
}
