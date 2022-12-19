using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystems : MonoBehaviour
{
    /*====== PUBLIC ======*/
    public static LSystems Instance;

    public LSystemBase preset_1;
    public List<LSystemBase> LSystem_presets = new List<LSystemBase>();

    private string _axiom;
    private Dictionary<char, string> _rules;
    private Dictionary<string, float> _constants;

    /*====== UNITY METHODS ======*/
    private void Awake() => Instance = this;

    private void Start()
    {
        /*======== PRESET 1 ========*/
        _axiom = "F(l)[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A";
        _rules = new Dictionary<char, string>()
        {
            {'A', "F(l)[&(a0)!(wr)\"(r2)B]+(d)!(wr)\"(r1)A"},
            {'B', "F(l)[-(a2)$!(wr)\"(r2)C]!(wr)\"(r1)C"},
            {'C', "F(l)[+(a2)$!(wr)\"(r2)B]!(wr)\"(r1)B"}
        };
        _constants = new Dictionary<string, float>()
        {
            {"r1",  0.9F    },
            {"r2",  0.7F    },
            {"a0",  45.0F   },
            {"a2",  45.0F   },
            {"d",   92.5F   },
            {"wr",  0.707F  },
        };

        preset_1 = new LSystemBase(
            _axiom,
            _rules,
            _constants
        );

        LSystem_presets.Add(preset_1);
    }

    public LSystemBase GetLSystemBase()
    {
        return LSystem_presets[Random.Range(0, LSystem_presets.Count - 1)];
    }
}
