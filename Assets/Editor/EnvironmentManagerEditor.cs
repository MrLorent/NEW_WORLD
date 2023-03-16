using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentManager))]
public class EnvironmentManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnvironmentManager env_colors = (EnvironmentManager)target;

        if(GUILayout.Button("Update Shader"))
        {
            env_colors.update_shader();
        }
    }
}
