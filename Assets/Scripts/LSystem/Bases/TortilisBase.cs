using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Tortilis")]
public class TortilisBase : LSystemBase
{
    //[Header("Start Parameters")]
    [SerializeField]
    public string axiom => _axiom = "F(l)[&(a1)!(wr)\"(r1)B]|[&(a2)!(wr)\"(r2)B]";

    [SerializeField]
    public float start_width => _start_width = 1;

    [SerializeField]
    public float start_length => _start_length = 10;

    // [Header("Tropism parmeters")]
    [SerializeField]
    public bool tropism => _tropism = false;

    TortilisBase()
    {
        /*====== RULES ======*/
        _rules = new Dictionary<char, string>()
        {
            {'A', "F(l)[&(a1)!(wr)\"(r1)B]|[&(a2)!(wr)\"(r2)B]"},
            {'B', "F(l)[<(a1)!(wr)\"(r1)$B][>(a2)!(wr)\"(r2)$B]"}
        };

        /*==== CONSTANTS ====*/
        _constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.8F    },
            {"a1",  20.0F   },
            {"a2",  50.0F   },
            {"wr",  0.707F  }
        };
    }
}
