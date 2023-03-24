using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Poplar")]
public class PoplarBase : LSystemBase
{
    PoplarBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.1F;
        start_length = 0.75F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            { 'A', "F(l)[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A" },
            { 'B', "F(l)[>(a2)$!(wr)\"(r2)C]!(wr)\"(r1)CZ" },
            { 'C', "F(l)[<(a2)$!(wr)\"(r2)B]!(wr)\"(r1)BZ" },
            {'X', "" },
            {'Y', "X" },
            {'Z', "Y" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.6F    },
            {"a0",  45.0F   },
            {"a2",  45.0F  },
            {"d",   137.5F  },
            {"lg",  1.08F   },
            {"wg",  1.109F   },
            {"wr",  0.707F  },
            {"min_w", 0.01F  },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };

        /*====== OTHER ======*/
        tropism = true;
    }

    public override float elasticity(float width)
    {
        return 5.0F;
    }

    public override float get_foliage_scale_X(int age)
    {
        return age >= 3 ? Mathf.Clamp(age * 0.75F, 1, 3.0F) : 0.0F;
    }

    public override float get_foliage_scale_Y(int age)
    {
        return age >= 2 ? Mathf.Clamp(age * 0.5F, 1, 2.75F) : 0.0F;
    }

    public override float get_foliage_scale_Z(int age)
    {
        return age >= 1 ? Mathf.Clamp(age * 0.33F, 1, 2.75F) : 0.0F;
    }
}
