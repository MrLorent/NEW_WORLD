using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator map_generator = (MapGenerator)target;

        if (GUILayout.Button("Generate new map"))
        {
            map_generator.generate_map();
        }
    }
}
