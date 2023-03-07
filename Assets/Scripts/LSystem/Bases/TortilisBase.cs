using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

[CreateAssetMenu(menuName = "L-System/Base/Tortilis")]
public class TortilisBase : LSystemBase
{
    TortilisBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.2F;
        start_length = 7.5F;
        axiom = "!(" + start_width.ToString("F2", CultureInfo.InvariantCulture) + ")F(" + start_length.ToString("F2", CultureInfo.InvariantCulture) + ")[&(a1)\"(5.0)B]|[&(a2)\"(4.2)B]";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {  'A', "!(" + start_width.ToString("F2", CultureInfo.InvariantCulture) + ")F(l)[&(a1)\"(5.0)B]|[&(a2)\"(4.2)B]"  },
            {  'B', "!(" + start_width.ToString("F2", CultureInfo.InvariantCulture) + ")F(l)[<(a1)\"(5.0)$B][>(a2)\"(4.2)$B]" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"a1",  20.0F   },
            {"a2",  50.0F   },
            {"lr",  1.115F  },
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
