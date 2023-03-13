Shader "Custom/TerrainShader"
{
    Properties
    {
        _Default_Color ("Plain Color (Default)", Color) = (0.8470588,0.2941176,0.3686274,1)
        _Desert_Color ("Desert Color", Color) = (0.9058824,0.8,0.5529411,1)
        _Mountain_Color ("Mountain Color", Color) = (0.6039216,0.8980392,1,1)
        _Snow_Color ("Snow Color", Color) = (1,1,1,1)
        _Swamp_Color ("Swamp Color", Color) = (0.145098,0.145098,0.3254901,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.25
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _Desert_Position ("Desert position", Vector) = (800, 0, 790, 1)
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

        fixed4 _Default_Color;
        fixed4 _Desert_Color;
        fixed4 _Mountain_Color;
        fixed4 _Snow_Color;
        fixed4 _Swamp_Color;

        fixed3 _Desert_Position;
        fixed3 _Swamp_Position;

        half _Glossiness;
        half _Metallic;
        

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 final_color = _Default_Color;

            if(IN.worldPos[1] > 15.0F)
            {
                final_color = _Mountain_Color;
            }
            else
            {
                if(length(IN.worldPos - _Swamp_Position) < 450.0F)
                {
                    final_color = _Swamp_Color;
                }
                else if(length(IN.worldPos - _Desert_Position) < 450.0F)
                {
                    final_color = _Desert_Color;
                }
            }

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * final_color;
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
