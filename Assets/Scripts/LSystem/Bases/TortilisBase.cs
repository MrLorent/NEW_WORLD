using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Tortilis")]
public class TortilisBase : LSystemBase
{
    TortilisBase()
    {
        /*====== AXIOM ======*/
        start_width = 1.0F;
        start_length = 10F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a1)!(wr)\"(r1)B]|[&(a2)!(wr)\"(r2)B]";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {  'A', "F(l)[&(a1)!(wr)\"(r1)B]|[&(a2)!(wr)\"(r2)B]"  },
            {  'B', "F(l)[<(a1)!(wr)\"(r1)$B][>(a2)!(wr)\"(r2)$B]" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.8F    },
            {"a1",  20.0F   },
            {"a2",  50.0F   },
            {"lg",  1.115F  },
            {"wg",  1.35F   },
            {"wr",  0.707F  },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };

        /*==== TROPISM ====*/
        tropism = false;
    }
}
