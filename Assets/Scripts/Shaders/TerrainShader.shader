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

        _Desert_Position ("Desert position", Vector) = (800, 0, 790, 1)
        _Mountain_Position ("Desert position", Vector) = (0, 0, 0, 1)
        _Plain_Position ("Desert position", Vector) = (0, 0, 0, 1)
        _Swamp_Position ("Swamp position", Vector) = (250, 0, 760, 1)
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

        fixed4 _Plain_Color;
        fixed4 _Desert_Color;
        fixed4 _Mountain_Color;
        fixed4 _Snow_Color;
        fixed4 _Swamp_Color;

        fixed3 _Desert_Position;
        fixed3 _Mountain_Position;
        fixed3 _Plain_Position;
        fixed3 _Swamp_Position;

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
            float2 xz_position = float2(position.x, position.z);
            float2 xz_desert = float2(_Desert_Position.x, _Desert_Position.z);
            float2 xz_mountain = float2(_Mountain_Position.x, _Mountain_Position.z);
            float2 xz_plain = float2(_Plain_Position.x, _Plain_Position.z);
            float2 xz_swamp = float2(_Swamp_Position.x, _Swamp_Position.z);

            float desert_dist = length(xz_desert - xz_position);
            float mountain_dist = length(xz_mountain- xz_position);
            float plain_dist = length(xz_plain - xz_position);
            float swamp_dist = length(xz_swamp - xz_position);
            
            float dist_0;
            fixed4 color_0;
            float dist_1;
            fixed4 color_1;

            
            if (desert_dist < mountain_dist)
            {
                dist_0 = desert_dist;
                color_0 = _Desert_Color;
            }
            else
            {
                dist_0 = mountain_dist;
                color_0 = _Mountain_Color;
            }

            if (plain_dist < swamp_dist)
            {
                dist_1 = plain_dist;
                color_1 = _Plain_Color;
            }
            else
            {
                dist_1 = swamp_dist;
                color_1 = _Swamp_Color;
            }

            fixed4 base_color = dist_0 < dist_1 ? color_0 : color_1;

            // if(all(base_color == _Desert_Color))
            // {
            //     if(length(desert_dist - mountain_dist) < 2.5F)
            //     {
            //         float3 z = float3(0,1,0);
            //         float3 middle_position = _Desert_Position + position * 0.5;
            //         float2 xz_middle = float2(middle_position.x, middle_position.z);
            //         float3 normal = cross(z, middle_position);
            //         float2 zx_normal = float2(normal.x, normal.z);
            //         zx_normal = normalize(zx_normal);
            //         float alpha = acos(dot(zx_normal, middle_position));
            //         float frag_to_middle_axe = length(xz_position - xz_middle) * sin(alpha);
                    
            //         float linear_middle = lerp(0, 1, 1 - frag_to_middle_axe);
            //         base_color += lerp(_Desert_Color, _Mountain_Color, linear_middle);
            //     }

            //     if(length(desert_dist - xz_plain) < 2.5F)
            //     {
                    
            //     }

            //     if(length(desert_dist - xz_swamp) < 2.5F)
            //     {
                    
            //     }
            // }
            
            return base_color;
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
