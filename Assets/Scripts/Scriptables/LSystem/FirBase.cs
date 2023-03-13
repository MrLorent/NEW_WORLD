using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Fir")]
public class FirBase : LSystemBase
{
    FirBase()
    {
        /*====== AXIOM ======*/
        start_width = 1F;
        start_length = 10.0F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a0)!(wr)\"(r2)C]+(d1)[&(a0)!(wr)\"(r2)C]+(d2)[&(a0)!(wr)\"(r2)C]+(d3)!(wr)\"(r1)B";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            { 'A', "F(l)[&(a0)!(wr)\"(r2)C]+(d1)[&(a0)!(wr)\"(r2)C]+(d2)[&(a0)!(wr)\"(r2)C]+(d3)!(wr)\"(r1)B" },
            { 'B', "F(l)[&(a0)!(wr)\"(r2)C]+(d2)[&(a0)!(wr)\"(r2)C]+(d1)[&(a0)!(wr)\"(r2)C]+(d3)!(wr)\"(r1)A" },
            { 'C', "^(a3)F(l)[>(a2)$!(wr)\"(r2)D]!(wr)\"(r1)DZ" },
            { 'D', "^(a3)F(l)[<(a2)$!(wr)\"(r2)C]!(wr)\"(r1)CZ" },
            {'Y', "" },
            {'Z', "Y" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.6F    },
            {"a0",  80.0F   },
            {"a2",  75.0F  },
            {"a3",  5.0F  },
            {"d1",   109.5F  },
            {"d2",   137.5F  },
            {"d3",   60.0F  },
            {"lg",  1.08F   },
            {"wg",  1.109F   },
            {"wr",  0.707F  },
            {"min_w", 0.1F  },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };

        /*====== OTHER ======*/
        tropism = false;
    }
}
