using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Oak")]
public class OakBase : LSystemBase
{
    OakBase()
    {
        /*====== ID ======*/
        id = "oak";
        name = "oak";

        /*====== AXIOM ======*/
        start_width = 1.0F;
        start_length = 10.0F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")A";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "F(l)+(d1)[&(a)!(wr)\"(lr)A]+(d2)[&(a)!(wr)\"(lr)A]+(d2)[&(a)!(wr)\"(lr)A]" },
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"d1",  30.0F   },
            {"d2",  120.0F  },
            {"a",   25.85F  },
            {"lg",  1.109F  },
            {"wg",  1.2F   },
            {"lr",  0.9F    },
            {"wr",  0.707F    },
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