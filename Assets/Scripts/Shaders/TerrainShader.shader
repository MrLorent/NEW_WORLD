Shader "Custom/TerrainShader"
{
    Properties
    {
        _Plain_Color ("Plain Color (Default)", Color) = (1,1,1,1)
        _Desert_Color ("Desert Color", Color) = (1,1,1,1)
        _Mountain_Color ("Mountain Color", Color) = (1,1,1,1)
        _Snow_Color ("Snow Color", Color) = (1,1,1,1)
        _Swamp_Color ("Swamp Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.25
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        fixed3 _Desert_Position;
        float _Desert_Radius;
        fixed4 _Desert_Color;

        fixed3 _Mountain_Position;
        float _Mountain_Radius;
        fixed4 _Mountain_Color;
        fixed4 _Snow_Color;

        fixed3 _Plain_Position;
        float _Plain_Radius;
        fixed4 _Plain_Color;

        fixed3 _Swamp_Position;
        float _Swamp_Radius;
        fixed4 _Swamp_Color;

        half _Glossiness;
        half _Metallic;
        

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        fixed4 get_base_color(float3 position)
        {
            float desert_dist = length(_Desert_Position - position);
            float mountain_dist = length(_Mountain_Position - position);
            float plain_dist = length(_Plain_Position - position);
            float swamp_dist = length(_Swamp_Position - position);
            
            fixed4 color = fixed4(1,1,1,1);

            if(position.x < _Swamp_Position.x)
            {
                if(position.z < _Mountain_Position.z)
                {
                    return _Mountain_Color;
                }
                else if( position.z > _Swamp_Position.z)
                {
                    return _Swamp_Color;
                }
                else
                {
                    return lerp(_Mountain_Color, _Swamp_Color, (position.z - _Mountain_Position.z) / (_Swamp_Position.z - _Mountain_Position.z));
                }
            }
            else if(position.x > _Plain_Position.x)
            {
                if(position.z < _Plain_Position.z)
                {
                    return _Plain_Color;
                }
                else if( position.z > _Desert_Position.z)
                {
                    return _Desert_Color;
                }
                else
                {
                    return lerp(_Plain_Color, _Desert_Color, (position.z - _Plain_Position.z) / (_Desert_Position.z - _Plain_Position.z));
                }
            }
            else
            {
                if(position.z < (_Mountain_Position.z + _Plain_Position.z) * 0.5)
                {
                    return lerp(_Mountain_Color, _Plain_Color, (position.x - _Mountain_Position.x) / (_Plain_Position.x - _Mountain_Position.x));
                }
                else if(position.z > _Swamp_Position.z)
                {
                    return lerp(_Swamp_Color, _Desert_Color, (position.x - _Swamp_Position.x) / (_Desert_Position.x - _Swamp_Position.x));
                }
                else
                {
                    fixed4 TOP_HORIZONTAL_COLOR = lerp(_Swamp_Color, _Desert_Color, (position.x - _Swamp_Position.x) / (_Desert_Position.x - _Swamp_Position.x));
                    fixed4 BOT_HORIZONTAL_COLOR = lerp(_Mountain_Color, _Plain_Color, (position.x - _Mountain_Position.x) / (_Plain_Position.x - _Mountain_Position.x));
                    return lerp(BOT_HORIZONTAL_COLOR, TOP_HORIZONTAL_COLOR, (position.z - _Mountain_Position.z) / (_Swamp_Position.z - _Mountain_Position.z));
                }
            }

            // if (desert_dist < _Desert_Radius)
            // {
            //     return _Desert_Color;
            // }

            // if (mountain_dist < _Mountain_Radius)
            // {
            //     return _Mountain_Color;
            // }

            // if (plain_dist < _Plain_Radius)
            // {
            //     return _Plain_Color;
            // }

            // if (swamp_dist < _Swamp_Radius)
            // {
            //     return _Swamp_Color;
            // }

            // float dist_0;
            // fixed4 color_0;
            // float dist_1;
            // fixed4 color_1;

            // if (desert_dist < mountain_dist)
            // {
            //     dist_0 = desert_dist;
            //     color_0 = _Desert_Color;
            // }
            // else
            // {
            //     dist_0 = mountain_dist;
            //     color_0 = _Mountain_Color;
            // }

            // if (plain_dist < swamp_dist)
            // {
            //     dist_1 = plain_dist;
            //     color_1 = _Plain_Color;
            // }
            // else
            // {
            //     dist_1 = swamp_dist;
            //     color_1 = _Swamp_Color;
            // }

            // if((dist_1 - dist_0 * 1.5F) > 0 || (dist_0 - dist_1 * 1.5F) > 0)
            // {
            //     return dist_0 < dist_1 ? color_0 : color_1;
            // }
            
            return color;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 base_color = get_base_color(IN.worldPos);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * base_color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
