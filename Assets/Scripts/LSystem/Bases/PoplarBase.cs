using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Poplar")]
public class PoplarBase : LSystemBase
{
    //[Header("Start Parameters")]
    [SerializeField]
    public string axiom => _axiom = "F(l)[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A";

    [SerializeField]
    public float start_width => _start_width = 1;

    [SerializeField]
    public float start_length => _start_length = 1;

    // [Header("Tropism parmeters")]
    [SerializeField]
    public bool tropism => _tropism = false;

    PoplarBase()
    {
        /*====== RULES ======*/
        _rules = new Dictionary<char, string>()
        {
            {'A', "F(l)[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A"},
            {'B', "F(l)[+(15)>(a2)$!(wr)\"(r2)C]!(wr)\"(r1)C"},
            {'C', "F(l)[-(15)<(a2)$!(wr)\"(r2)B]!(wr)\"(r1)B"}
        };

        /*==== CONSTANTS ====*/
        _constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.8F    },
            {"a0",  45.0F   },
            {"a2",  45.0F   },
            {"d",   137.5F  },
            {"wr",  0.707F  }
        };
    }
}
