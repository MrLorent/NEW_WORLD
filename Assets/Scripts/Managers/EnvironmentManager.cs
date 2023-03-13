using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Color plain_color = new Color(0.8470588F, 0.2941176F, 0.3686274F, 1F);
    public Color desert_color = new Color(0.9058824F, 0.8F, 0.5529411F, 1F);
    public Color mountain_color = new Color(0.6039216F, 0.8980392F, 1.0F, 1F);
    public Color snow_color = new Color(1.0F, 1.0F, 1.0F, 1F);
    public Color swamp_color = new Color(0.145098F, 0.145098F, 0.3254901F, 1F);

    [SerializeField]
    private Material _terrain_material;

    [Space(10)]

    [SerializeField]
    private Transform _desert_position;
    [SerializeField]
    private Transform _swamp_position;

    
    private void Start()
    {
        // Pass the biom colors to the shader
        _terrain_material.SetColor("_Default_Color", plain_color);

        // Pass the bioms position to the shader
        _terrain_material.SetVector("_Desert_Position", new Vector4(_desert_position.position.x, _desert_position.position.y, _desert_position.position.z, 1));
        _terrain_material.SetVector("_Swamp_Position", new Vector4(_swamp_position.position.x, _swamp_position.position.y, _swamp_position.position.z, 1));
    }
}
