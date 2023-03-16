using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomState
{
    DESERT,
    MOUNTAIN,
    PLAIN,
    SWAMP
};

public struct Environnment
{
    public BiomState State;
    public float temperature;
    public float humidity;
}

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [Header("PLAINS BIOM")]
    [SerializeField]
    private Transform _plain_position;
    [SerializeField]
    private float _plain_radius = 200.0F;
    [SerializeField]
    private Color _plain_ground_color = new Color(0.8470588F, 0.2941176F, 0.3686274F, 1F);

    [Space(10)]

    [Header("DESERT BIOM")]
    [SerializeField]
    private Transform _desert_position;
    [SerializeField]
    private float _desert_radius = 200.0F;
    [SerializeField]
    private Color _desert_ground_color = new Color(0.9058824F, 0.8F, 0.5529411F, 1F);

    [Space(10)]

    [Header("MOUNTAINS BIOM")]
    [SerializeField]
    private Transform _mountain_position;
    [SerializeField]
    private float _mountain_radius = 200.0F;
    [SerializeField]
    private Color _mountain_ground_color = new Color(0.6039216F, 0.8980392F, 1.0F, 1F);
    [SerializeField]
    private Color _snow_color = new Color(1.0F, 1.0F, 1.0F, 1F);

    [Space(10)]

    [Header("SWAMP BIOM")]
    [SerializeField]
    private Transform _swamp_position;
    [SerializeField]
    private float _swamp_radius = 200.0F;
    [SerializeField]
    private Color _swamp_ground_color = new Color(0.145098F, 0.145098F, 0.3254901F, 1F);

    [Space(10)]

    [Header("TERRAIN SHADER")]
    [SerializeField]
    private Material _terrain_material;

    public Environnment get_environment(Vector3 position)
    {
        return new Environnment();
    }

    public void update_shader()
    {
        // Pass the biom colors to the shader
        _terrain_material.SetColor("_Plain_Color", _plain_ground_color);
        _terrain_material.SetColor("_Desert_Color", _desert_ground_color);
        _terrain_material.SetColor("_Mountain_Color", _mountain_ground_color);
        _terrain_material.SetColor("_Snow_Color", _snow_color);
        _terrain_material.SetColor("_Swamp_Color", _swamp_ground_color);

        // Pass the bioms position to the shader
        _terrain_material.SetVector("_Desert_Position", new Vector4(_desert_position.position.x, _desert_position.position.y, _desert_position.position.z, 1));
        _terrain_material.SetFloat("_Desert_Radius", _desert_radius);

        _terrain_material.SetVector("_Mountain_Position", new Vector4(_mountain_position.position.x, _mountain_position.position.y, _mountain_position.position.z, 1));
        _terrain_material.SetFloat("_Mountain_Radius", _mountain_radius);

        _terrain_material.SetVector("_Plain_Position", new Vector4(_plain_position.position.x, _plain_position.position.y, _plain_position.position.z, 1));
        _terrain_material.SetFloat("_Plain_Radius", _plain_radius);

        _terrain_material.SetVector("_Swamp_Position", new Vector4(_swamp_position.position.x, _swamp_position.position.y, _swamp_position.position.z, 1));
        _terrain_material.SetFloat("_Swamp_Radius", _swamp_radius);

        // Update biom colliders
        _desert_position.GetComponent<SphereCollider>().radius = _desert_radius;
        _mountain_position.GetComponent<SphereCollider>().radius = _mountain_radius;
        _plain_position.GetComponent<SphereCollider>().radius = _plain_radius;
        _swamp_position.GetComponent<SphereCollider>().radius = _swamp_radius;
    }
}
