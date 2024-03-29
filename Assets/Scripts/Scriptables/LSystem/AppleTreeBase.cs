using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Apple Tree")]
public class AppleTreeBase : LSystemBase
{
    AppleTreeBase()
    {
        /*====== AXIOM ======*/
        start_width = 0.1F;
        start_length = 1.0F;
        axiom = "!(" + Helpers.convert_float_to_string(start_width) + ")F(" + Helpers.convert_float_to_string(start_length) + ")[&(a1)+(o1)!(q2)\"(r1)A][&(a2)+(o2)!(q2)\"(r2)A]";

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {  'A', "F(l)[&(a1)+(o1)!(q2)\"(r1)A][&(a2)+(o2)!(q2)\"(r2)A]Z"  },
            {'X', "" },
            {'Y', "X" },
            {'Z', "Y" }
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
            {"wg",  1.15F   },
            {"q1",  Mathf.Pow(0.5F, 0.5F)    },
            {"q2",  Mathf.Pow(0.5F, 0.5F)    },
            {"min_w",  0.01F },
            {"Tx",  0.0F    },
            {"Ty",  -1.0F   },
            {"Tz",  0.0F    },
            {"e",   0.22F   }
        };

        /*==== TROPISM ====*/
        tropism = true;
    }

    public override float elasticity(float width)
    {
        return 15.0F;
    }

    public override float get_foliage_scale_X(int age)
    {
        return age >= 3 ? Mathf.Clamp(age * 0.75F, 1, 3.0F) : 0.0F;
    }

    public override float get_foliage_scale_Y(int age)
    {
        return age >= 2 ? Mathf.Clamp(age * 0.5F, 1, 3.0F) : 0.0F;
    }

    public override float get_foliage_scale_Z(int age)
    {
        return age >= 1 ? Mathf.Clamp(age * 0.33F, 1, 3.0F) : 0.0F;
    }
}