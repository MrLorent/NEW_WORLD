using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DiamondSquareGenerator))]
public class DiamondSquareGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DiamondSquareGenerator diamond_square_generator = (DiamondSquareGenerator)target;

        if (GUILayout.Button("Generate new map"))
        {
            diamond_square_generator.generate_map();
        }
    }
}
