using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Apple Tree")]
public class AppleTreeBase : LSystemBase
{
    AppleTreeBase()
    {
        /*====== ID ======*/
        id = "apple_tree";
        name = "Apple Tree";

        /*====== AXIOM ======*/
        start_width = 1.0F;
        start_length = 7.5F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a1)+(o1)!(q2)\"(r1)A][&(a2)+(o2)!(q2)\"(r2)A]";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {  'A', "F(l)[&(a1)+(o1)!(q2)\"(r1)A][&(a2)+(o2)!(q2)\"(r2)A]"  },
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"r1",  0.8F    },
            {"r2",  0.8F    },
            {"a1",  30.0F   },
            {"a2",  -30.0F   },
            {"o1",  137.0F  },
            {"o2",  137.0F  },
            {"lg",  1.109F    },
            {"wg",  1.109F   },
            {"q1",  Mathf.Pow(0.5F, 0.5F)    },
            {"q2",  Mathf.Pow(0.5F, 0.5F)    },
            {"min_w",  0.1F },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };

        /*==== TROPISM ====*/
        tropism = false;
    }
}
