using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// desert Color(0.9058824F, 0.8F, 0.5529411F, 1F); #E7CC8D
// mountain Color(0.6039216F, 0.8980392F, 1.0F, 1F); #9AE5FF
// plain Color(0.8470588F, 0.2941176F, 0.3686274F, 1F); #D84B5E
// swamp Color(0.145098F, 0.145098F, 0.3254901F, 1F); #343484

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [Header("PLAINS BIOM")]
    [SerializeField]
    private Biom _BOTTOM_RIGHT;

    [Space(10)]

    [Header("DESERT BIOM")]
    [SerializeField]
    private Biom _TOP_RIGHT;
    
    [Space(10)]

    [Header("MOUNTAINS BIOM")]
    [SerializeField]
    private Biom _BOTTOM_LEFT;

    [Space(10)]

    [Header("SWAMP BIOM")]
    [SerializeField]
    private Biom _TOP_LEFT;
    
    [Space(10)]

    [Header("TERRAIN SHADER")]
    [SerializeField]
    private Color _snow_color = new Color(1.0F, 1.0F, 1.0F, 1F);

    [SerializeField]
    private Material _terrain_material;

    public Environment get_environment(Vector3 position)
    {
        Vector2 xz_position = new Vector2(position.x, position.z);

        Vector2 xz_desert = new Vector2(_TOP_RIGHT.transform.position.x, _TOP_RIGHT.transform.position.z);
        Vector2 xz_mountain = new Vector2(_BOTTOM_LEFT.transform.position.x, _BOTTOM_LEFT.transform.position.z);
        Vector2 xz_plain = new Vector2(_BOTTOM_RIGHT.transform.position.x, _BOTTOM_RIGHT.transform.position.z);
        Vector2 xz_swamp = new Vector2(_TOP_LEFT.transform.position.x, _TOP_LEFT.transform.position.z);

        Vector2 TOP_LEFT = xz_swamp;
        Vector2 TOP_RIGHT = xz_desert;
        Vector2 BOTTOM_RIGHT = xz_plain;
        Vector2 BOTTOM_LEFT = xz_mountain;

        /*if (Helpers.is_left(BOTTOM_LEFT, TOP_LEFT, xz_position))
        {
            if (Helpers.is_left(BOTTOM_RIGHT, BOTTOM_LEFT, xz_position))
            {
                return new Environment(


                );
            }
        }
        else if ((Helpers.is_left(xz_desert, xz_plain, xz_position))
        {

        }
        else
        {

        }*/

        return new Environment();
    }

    public void update_shader()
    {
        // Pass the biom colors to the shader
        _terrain_material.SetColor("_Plain_Color", _BOTTOM_RIGHT.ground_color);
        _terrain_material.SetColor("_Desert_Color", _TOP_RIGHT.ground_color);
        _terrain_material.SetColor("_Mountain_Color", _BOTTOM_LEFT.ground_color);
        _terrain_material.SetColor("_Swamp_Color", _TOP_LEFT.ground_color);
        _terrain_material.SetColor("_Snow_Color", _snow_color);

        // Pass the bioms position to the shader
        _terrain_material.SetVector("_TOP_RIGHT", new Vector4(_TOP_RIGHT.transform.position.x, _TOP_RIGHT.transform.position.y, _TOP_RIGHT.transform.position.z, 1));
        _terrain_material.SetVector("_BOTTOM_LEFT", new Vector4(_BOTTOM_LEFT.transform.position.x, _BOTTOM_LEFT.transform.position.y, _BOTTOM_LEFT.transform.position.z, 1));
        _terrain_material.SetVector("_BOTTOM_RIGHT", new Vector4(_BOTTOM_RIGHT.transform.position.x, _BOTTOM_RIGHT.transform.position.y, _BOTTOM_RIGHT.transform.position.z, 1));
        _terrain_material.SetVector("_TOP_LEFT", new Vector4(_TOP_LEFT.transform.position.x, _TOP_LEFT.transform.position.y, _TOP_LEFT.transform.position.z, 1));
    }
}
