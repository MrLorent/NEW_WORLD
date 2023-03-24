using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// desert Color(0.9058824F, 0.8F, 0.5529411F, 1F); #E7CC8D
// mountain Color(0.6039216F, 0.8980392F, 1.0F, 1F); #9AE5FF
// plain Color(0.8470588F, 0.2941176F, 0.3686274F, 1F); #D84B5E
// swamp Color(0.145098F, 0.145098F, 0.3254901F, 1F); #343484

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [Header("BIOMS")]
    [SerializeField]
    public Biom _BOTTOM_RIGHT;
    [SerializeField]
    private Biom _TOP_RIGHT;
    [SerializeField]
    private Biom _BOTTOM_LEFT;
    [SerializeField]
    private Biom _TOP_LEFT;

    public List<Biom> _bioms { get; private set; }

    [Header("GENERAL COLORS")]
    [SerializeField]
    private Color _snow_color = new Color(1.0F, 1.0F, 1.0F, 1F);

    [Header("TERRAIN SHADER")]
    [SerializeField]
    private Material _terrain_material;

    private void Start()
    {
        _bioms = new List<Biom>()
        { 
            _TOP_LEFT,
            _TOP_RIGHT,
            _BOTTOM_RIGHT,
            _BOTTOM_LEFT
        };
    }

    public Environment get_environment(Vector3 position)
    {
        Vector2 xz_position = new Vector2(position.x, position.z);

        Vector2 xz_desert = get_biome_position(_TOP_RIGHT);
        Vector2 xz_mountain = get_biome_position(_BOTTOM_LEFT);
        Vector2 xz_plain = get_biome_position(_BOTTOM_RIGHT);
        Vector2 xz_swamp = get_biome_position(_TOP_LEFT);

        Vector2 XZ_TOP_LEFT = xz_swamp;
        Vector2 XZ_TOP_RIGHT = xz_desert;
        Vector2 XZ_BOTTOM_RIGHT = xz_plain;
        Vector2 XZ_BOTTOM_LEFT = xz_mountain;

        if (Helpers.is_left(XZ_BOTTOM_LEFT, XZ_TOP_LEFT, xz_position))
        {
            if (Helpers.is_left(XZ_BOTTOM_RIGHT, XZ_BOTTOM_LEFT, xz_position))
            {
                return new Environment(
                    _BOTTOM_LEFT.biom_type,
                    _BOTTOM_LEFT.temperature,
                    _BOTTOM_LEFT.humidity_rate
                );
            }
            else if (Helpers.is_left(XZ_TOP_LEFT, XZ_TOP_RIGHT, xz_position))
            {
                return new Environment(
                    _TOP_LEFT.biom_type,
                    _TOP_LEFT.temperature,
                    _TOP_LEFT.humidity_rate
                );
            }
            else
            {
                Vector2 start = Helpers.get_intersection(XZ_BOTTOM_LEFT, XZ_BOTTOM_RIGHT, xz_position, xz_position + new Vector2(0, -1));
                Vector2 end = Helpers.get_intersection(XZ_TOP_LEFT, XZ_TOP_RIGHT, xz_position, xz_position + new Vector2(0, 1));
                float linear_factor = (xz_position.y - start.y) / (end.y - start.y);
                linear_factor *= linear_factor * (3.0F - 2.0F * linear_factor);

                return new Environment(
                    ((XZ_BOTTOM_LEFT - xz_position).magnitude - (XZ_TOP_LEFT - xz_position).magnitude) < 0.005 ? _BOTTOM_LEFT.biom_type : _TOP_LEFT.biom_type,
                    Mathf.Lerp(_BOTTOM_LEFT.temperature, _TOP_LEFT.temperature, linear_factor),
                    Mathf.Lerp(_BOTTOM_LEFT.humidity_rate, _TOP_LEFT.humidity_rate, linear_factor)
                );

            }
        }
        else if (Helpers.is_left(XZ_TOP_RIGHT, XZ_BOTTOM_RIGHT, xz_position))
        {
            if (Helpers.is_left(XZ_BOTTOM_RIGHT, XZ_BOTTOM_LEFT, xz_position))
            {
                return new Environment(
                    _BOTTOM_RIGHT.biom_type,
                    _BOTTOM_RIGHT.temperature,
                    _BOTTOM_RIGHT.humidity_rate
                );
            }
            else if (Helpers.is_left(XZ_TOP_LEFT, XZ_TOP_RIGHT, xz_position))
            {
                return new Environment(
                    _TOP_RIGHT.biom_type,
                    _TOP_RIGHT.temperature,
                    _TOP_RIGHT.humidity_rate
                );
            }
            else
            {
                Vector2 start = Helpers.get_intersection(XZ_BOTTOM_LEFT, XZ_BOTTOM_RIGHT, xz_position, xz_position + new Vector2(0, -1));
                Vector2 end = Helpers.get_intersection(XZ_TOP_LEFT, XZ_TOP_RIGHT, xz_position, xz_position + new Vector2(0, 1));
                float linear_factor = (xz_position.y - start.y) / (end.y - start.y);
                linear_factor *= linear_factor * (3.0F - 2.0F * linear_factor);

                return new Environment(
                    ((XZ_BOTTOM_RIGHT - xz_position).magnitude - (XZ_TOP_RIGHT - xz_position).magnitude) < 0.005 ? _BOTTOM_RIGHT.biom_type : _TOP_RIGHT.biom_type,
                    Mathf.Lerp(_BOTTOM_RIGHT.temperature, _TOP_RIGHT.temperature, linear_factor),
                    Mathf.Lerp(_BOTTOM_RIGHT.humidity_rate, _TOP_RIGHT.humidity_rate, linear_factor)
                );

            }
        }
        else
        {
            if (Helpers.is_left(XZ_BOTTOM_RIGHT, XZ_BOTTOM_LEFT, xz_position))
            {
                Vector2 start = Helpers.get_intersection(XZ_BOTTOM_LEFT, XZ_TOP_LEFT, xz_position, xz_position + new Vector2(-1, 0));
                Vector2 end = Helpers.get_intersection(XZ_BOTTOM_RIGHT, XZ_TOP_RIGHT, xz_position, xz_position + new Vector2(1, 0));
                float linear_factor = (xz_position.x - start.x) / (end.x - start.x);
                linear_factor *= linear_factor * (3.0F - 2.0F * linear_factor);

                return new Environment(
                    ((XZ_BOTTOM_LEFT - xz_position).magnitude - (XZ_BOTTOM_RIGHT - xz_position).magnitude) < 0.005 ? _BOTTOM_LEFT.biom_type : _BOTTOM_RIGHT.biom_type,
                    Mathf.Lerp(_BOTTOM_LEFT.temperature, _BOTTOM_RIGHT.temperature, linear_factor),
                    Mathf.Lerp(_BOTTOM_LEFT.humidity_rate, _BOTTOM_RIGHT.humidity_rate, linear_factor)
                );
            }
            else if(Helpers.is_left(XZ_TOP_LEFT, XZ_TOP_RIGHT, xz_position))
            {
                Vector2 start = Helpers.get_intersection(XZ_BOTTOM_LEFT, XZ_TOP_LEFT, xz_position, xz_position + new Vector2(-1, 0));
                Vector2 end = Helpers.get_intersection(XZ_BOTTOM_RIGHT, XZ_TOP_RIGHT, xz_position, xz_position + new Vector2(1, 0));
                float linear_factor = (xz_position.x - start.x) / (end.x - start.x);
                linear_factor *= linear_factor * (3.0F - 2.0F * linear_factor);

                return new Environment(
                    ((XZ_TOP_LEFT - xz_position).magnitude - (XZ_TOP_RIGHT - xz_position).magnitude) < 0.005 ? _TOP_LEFT.biom_type : _TOP_RIGHT.biom_type,
                    Mathf.Lerp(_TOP_LEFT.temperature, _TOP_RIGHT.temperature, linear_factor),
                    Mathf.Lerp(_TOP_LEFT.humidity_rate, _TOP_RIGHT.humidity_rate, linear_factor)
                );
            }
            else
            {
                Vector2 start = Helpers.get_intersection(XZ_BOTTOM_LEFT, XZ_TOP_LEFT, xz_position, xz_position + new Vector2(-1, 0));
                Vector2 end = Helpers.get_intersection(XZ_BOTTOM_RIGHT, XZ_TOP_RIGHT, xz_position, xz_position + new Vector2(1, 0));
                float linear_factor = (xz_position.x - start.x) / (end.x - start.x);
                linear_factor *= linear_factor * (3.0F - 2.0F * linear_factor);

                Vector2 closest_bottom = ((XZ_BOTTOM_LEFT - xz_position).magnitude - (XZ_BOTTOM_RIGHT - xz_position).magnitude) < 0.005 ? XZ_BOTTOM_LEFT : XZ_BOTTOM_RIGHT;
                Environment bottom_env = new Environment(
                    ((XZ_BOTTOM_LEFT - xz_position).magnitude - (XZ_BOTTOM_RIGHT - xz_position).magnitude) < 0.005 ? _BOTTOM_LEFT.biom_type : _BOTTOM_RIGHT.biom_type,
                    Mathf.Lerp(_BOTTOM_LEFT.temperature, _BOTTOM_RIGHT.temperature, linear_factor),
                    Mathf.Lerp(_BOTTOM_LEFT.humidity_rate, _BOTTOM_RIGHT.humidity_rate, linear_factor)
                );

                Vector2 closest_top = ((XZ_TOP_LEFT - xz_position).magnitude - (XZ_TOP_RIGHT - xz_position).magnitude) < 0.005 ? XZ_TOP_LEFT : XZ_TOP_RIGHT;
                Environment top_env = new Environment(
                    ((XZ_TOP_LEFT - xz_position).magnitude - (XZ_TOP_RIGHT - xz_position).magnitude) < 0.005 ? _TOP_LEFT.biom_type : _TOP_RIGHT.biom_type,
                    Mathf.Lerp(_TOP_LEFT.temperature, _TOP_RIGHT.temperature, linear_factor),
                    Mathf.Lerp(_TOP_LEFT.humidity_rate, _TOP_RIGHT.humidity_rate, linear_factor)
                );

                start = Helpers.get_intersection(XZ_BOTTOM_LEFT, XZ_BOTTOM_RIGHT, xz_position, xz_position + new Vector2(0, -1));
                end = Helpers.get_intersection(XZ_TOP_LEFT, XZ_TOP_RIGHT, xz_position, xz_position + new Vector2(0, 1));
                linear_factor = (xz_position.y - start.y) / (end.y - start.y);
                linear_factor *= linear_factor * (3.0F - 2.0F * linear_factor);

                return new Environment(
                    ((closest_bottom - xz_position).magnitude - (closest_top - xz_position).magnitude) < 0.005 ? bottom_env.biom_type : top_env.biom_type,
                    Mathf.Lerp(bottom_env.temperature, top_env.temperature, linear_factor),
                    Mathf.Lerp(bottom_env.humidity_rate, top_env.humidity_rate, linear_factor)
                );
            }

        }
    }

    public void update_shader()
    {
        // Pass the biom colors to the shader
        _terrain_material.SetColor("_TOP_LEFT_COLOR", _TOP_LEFT.ground_color);
        _terrain_material.SetColor("_TOP_RIGHT_COLOR", _TOP_RIGHT.ground_color);
        _terrain_material.SetColor("_BOTTOM_RIGHT_COLOR", _BOTTOM_RIGHT.ground_color);
        _terrain_material.SetColor("_BOTTOM_LEFT_COLOR", _BOTTOM_LEFT.ground_color);
        _terrain_material.SetColor("_SNOW_COLOR", _snow_color);

        // Pass the bioms position to the shader
        _terrain_material.SetVector("_TOP_RIGHT", new Vector4(_TOP_RIGHT.transform.position.x, _TOP_RIGHT.transform.position.y, _TOP_RIGHT.transform.position.z, 1));
        _terrain_material.SetVector("_BOTTOM_LEFT", new Vector4(_BOTTOM_LEFT.transform.position.x, _BOTTOM_LEFT.transform.position.y, _BOTTOM_LEFT.transform.position.z, 1));
        _terrain_material.SetVector("_BOTTOM_RIGHT", new Vector4(_BOTTOM_RIGHT.transform.position.x, _BOTTOM_RIGHT.transform.position.y, _BOTTOM_RIGHT.transform.position.z, 1));
        _terrain_material.SetVector("_TOP_LEFT", new Vector4(_TOP_LEFT.transform.position.x, _TOP_LEFT.transform.position.y, _TOP_LEFT.transform.position.z, 1));
    }

    public Vector2 get_biome_position(Biom biome)
    {
        switch (biome.biom_type)
        {
            case BiomType.DESERT:
                return new Vector2(_TOP_LEFT.transform.position.x, _TOP_LEFT.transform.position.z);
            case BiomType.SWAMP:
                return new Vector2(_TOP_RIGHT.transform.position.x, _TOP_RIGHT.transform.position.z);
            case BiomType.PLAIN:
                return new Vector2(_BOTTOM_LEFT.transform.position.x, _BOTTOM_LEFT.transform.position.z);
            case BiomType.MOUNTAIN:
                return new Vector2(_BOTTOM_RIGHT.transform.position.x, _BOTTOM_RIGHT.transform.position.z);
            default:
                return Vector2.zero;
        }
    }

    //Return the biom at the given position
    public Biom get_biom(Vector3 position)
    {
        Vector2 xz_position = new Vector2(position.x, position.z);
        if (xz_position.x < _TOP_LEFT.transform.position.x)
        {
            if (xz_position.y < _TOP_LEFT.transform.position.z)
            {
                return _TOP_LEFT;
            }
            else
            {
                return _BOTTOM_LEFT;
            }
        }
        else
        {
            if (xz_position.y < _TOP_RIGHT.transform.position.z)
            {
                return _TOP_RIGHT;
            }
            else
            {
                return _BOTTOM_RIGHT;
            }
        }
    }
}
