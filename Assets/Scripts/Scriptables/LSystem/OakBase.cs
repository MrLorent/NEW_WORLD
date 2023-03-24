using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Oak")]
public class OakBase : LSystemBase
{
    OakBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.1F;
        start_length = 1.0F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")\"(" + Helpers.convert_float_to_string(start_length) + ")A";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', "F(l)+(d1)[&(a)!(wr)\"(lr)A]+(d2)[&(a)!(wr)\"(lr)A]+(d2)[&(a)!(wr)\"(lr)A]Z" },
            {'X', "" },
            {'Y', "X" },
            {'Z', "Y" }
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"d1",  30.0F   },
            {"d2",  120.0F  },
            {"a",   25.85F  },
            {"lg",  1.109F  },
            {"wg",  1.15F   },
            {"lr",  0.9F    },
            {"wr",  0.707F    },
            {"min_w",  0.01F   },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   },
        };

        /*====== TROPISM ======*/
        tropism = true;
    }

    public override float elasticity(float width)
    {
        return 15.0F;
    }

    public override float get_foliage_scale_X(int age)
    {
        return Mathf.Clamp(age * 0.75F, 1, 4);
    }

    public override float get_foliage_scale_Y(int age)
    {
        return Mathf.Clamp(age * 0.5F, 1, 4);
    }

    public override float get_foliage_scale_Z(int age)
    {
        return Mathf.Clamp(age * 0.33F, 1, 4);
    }
}