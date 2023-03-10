using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Oak")]
public class OakBase : LSystemBase
{
    OakBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.3F;
        start_length = 7.5F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")A";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "!(" + Helpers.convert_float_to_string(start_width) + ")F(5)+(d1)[&(a)F(5)A]+(d2)[&(a)F(5)A]+(d2)[&(a)F(5)A]" },
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"d1",  30.0F   },
            {"d2",  120.0F  },
            {"a",   25.85F  },
            {"lg",  1.109F  },
            {"wg",  1.35F   },
            {"min_w",  0.1F   },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   },
        };

        /*====== TROPISM ======*/
        tropism = true;
    }
}