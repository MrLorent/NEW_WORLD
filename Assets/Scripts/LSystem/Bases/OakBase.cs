using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

[CreateAssetMenu(menuName = "L-System/Base/Oak Base")]
public class OakBase : LSystemBase
{
    OakBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.3F;
        start_length = 7.5F;
        axiom = "!(" + start_width.ToString("F2", CultureInfo.InvariantCulture) + ")F(" + start_length.ToString("F2", CultureInfo.InvariantCulture) + ")A";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "!(" + start_width.ToString("F2", CultureInfo.InvariantCulture) + ")F(5)+(d1)[&(a)F(5)A]+(d2)[&(a)F(5)A]+(d2)[&(a)F(5)A]" },
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"d1",  30.0F   },
            {"d2",  120.0F  },
            {"a",   25.85F  },
            {"lr",  1.109F  },
            {"wr",  1.35F   },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   },
        };

        /*====== TROPISM ======*/
        tropism = true;
    }
}