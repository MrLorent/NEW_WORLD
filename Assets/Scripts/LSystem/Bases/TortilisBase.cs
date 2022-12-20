using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Tortilis")]
public class TortilisBase : LSystemBase
{
    TortilisBase()
    {
        /*====== AXIOM ======*/
        axiom = "F(l)[&(a1)!(wr)\"(r1)B]|[&(a2)!(wr)\"(r2)B]";

        /*=== AXIOM PARAMS ===*/
        start_width = 1;
        start_length = 10;

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "F(l)[&(a1)!(wr)\"(r1)B]|[&(a2)!(wr)\"(r2)B]"},
            {'B', "F(l)[<(a1)!(wr)\"(r1)$B][>(a2)!(wr)\"(r2)$B]"}
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.8F    },
            {"a1",  20.0F   },
            {"a2",  50.0F   },
            {"wr",  0.707F  }
        };
    }
}
