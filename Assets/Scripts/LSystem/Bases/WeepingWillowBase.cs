using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Base/Weeping Willow")]
public class WeepingWillowBase : LSystemBase
{
    [SerializeField] public Vector3 tropism_direction = new Vector3(0.0F, -1.0F, 0.0F);
    [SerializeField] public float e = 0.22F;

    [Header("Rules")]
    [SerializeField]
    public string A = "F(l)F(l)[&(a)!(vr)\"(lr)A]+(d1)[&(a)!(vr)\"(lr)A]+(d2)[&(a)!(vr)\"(lr)A]";

    [Header("Constants")]
    [SerializeField] public float d1 = 94.74F;
    [SerializeField] public float d2 = 132.63F;
    [SerializeField] public float a = 18.85F;
    [SerializeField] public float lr = 0.902F;
    [SerializeField] public float vr = 0.577F;
    

    WeepingWillowBase()
    {
        /*====== AXIOM ======*/
        axiom = "F(l)+(45)A";

        /*=== AXIOM PARAMS ===*/
        start_width = 1;
        start_length = 10;

        /*====== RULES ======*/
        rules = new Dictionary<char, string>()
        {
            {'A', A },
        };

        /*==== CONSTANTS ====*/
        constants = new Dictionary<string, float>()
        {
            {"d1",  d1  },
            {"d2",  d2 },
            {"a",   a  },
            {"lr",  lr  },
            {"vr",  vr  },
            {"Tx",  tropism_direction.x    },
            {"Ty",  tropism_direction.y   },
            {"Tz",  tropism_direction.z    },
            {"e",   e   },
        };

        /*====== OTHER ======*/
        tropism = true;
    }
}