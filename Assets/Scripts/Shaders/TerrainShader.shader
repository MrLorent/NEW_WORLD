Shader "Custom/TerrainShader"
{
    Properties
    {
        _BOTTOM_RIGHT_COLOR ("Plain Color (Default)", Color) = (1,1,1,1)
        _TOP_RIGHT_COLOR ("Desert Color", Color) = (1,1,1,1)
        _BOTTOM_LEFT_COLOR ("Mountain Color", Color) = (1,1,1,1)
        _SNOW_COLOR ("Snow Color", Color) = (1,1,1,1)
        _TOP_LEFT_COLOR ("Swamp Color", Color) = (1,1,1,1)
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

        fixed3 _TOP_LEFT;
        fixed4 _TOP_LEFT_COLOR;

        fixed3 _TOP_RIGHT;
        fixed4 _TOP_RIGHT_COLOR;

        fixed3 _BOTTOM_RIGHT;
        fixed4 _BOTTOM_RIGHT_COLOR;

        fixed3 _BOTTOM_LEFT;
        fixed4 _BOTTOM_LEFT_COLOR;

        fixed4 _SNOW_COLOR;

        half _Glossiness;
        half _Metallic;
        

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        bool is_left(float2 a, float2 b, float2 c){
            return ((b.x - a.x)*(c.y - a.y) - (b.y - a.y)*(c.x - a.x)) > 0;
        }

        float2 get_intersection(float2 A1, float2 A2, float2 B1, float2 B2)
        {
            float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
            float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;
        
            return float2(
                B1.x + (B2.x - B1.x) * mu,
                B1.y + (B2.y - B1.y) * mu
            );
        }


        // Gives the color of the given position.
        // The map is sliced into 9 parts, the four bioms at the corners,
        // And intermediate values between them like so :
        // 
        // ----------------------------------------------------
        // |                |                |                |
        // |                |                |                |
        // |    TOP LEFT    |  INTERMEDIATE  |   TOP RIGHT    |
        // |                |                |                |
        // |                |                |                |
        // ----------------------------------------------------
        // |                |                |                |
        // |                |                |                |
        // |  INTERMEDIATE  |  INTERMEDIATE  |  INTERMEDIATE  |
        // |                |                |                |
        // |                |                |                |
        // ----------------------------------------------------
        // |                |                |                |
        // |  BOTTOM LEFT   |  INTERMEDIATE  |  BOTTOM RIGHT  |
        // |                |                |                |
        // |                |                |                |
        // ----------------------------------------------------
        // 
        // It's the same algorithm that is use EnvironmentManager.cs script
        // to give the environment of a given position.
        // For more details, see the documentation in EnvironmentManager.cs.
        //
        fixed4 get_base_color(float3 position)
        {   
            fixed4 color = fixed4(1,1,1,1);

            float2 xy_position = float2(position.x, position.z);

            float2 TOP_LEFT = float2(_TOP_LEFT.x, _TOP_LEFT.z);
            float2 TOP_RIGHT = float2(_TOP_RIGHT.x, _TOP_RIGHT.z);
            float2 BOTTOM_RIGHT = float2(_BOTTOM_RIGHT.x, _BOTTOM_RIGHT.z);
            float2 BOTTOM_LEFT = float2(_BOTTOM_LEFT.x, _BOTTOM_LEFT.z);
            
            if(is_left(BOTTOM_LEFT, TOP_LEFT, xy_position)) // xy_position is on BL_TL left ?
            {
                if(is_left(BOTTOM_RIGHT, BOTTOM_LEFT, xy_position))
                {
                    return _BOTTOM_LEFT_COLOR;
                }
                else if(is_left(TOP_LEFT, TOP_RIGHT, xy_position))
                {
                    return _TOP_LEFT_COLOR;
                }
                else
                {
                    float2 start = get_intersection(BOTTOM_LEFT, BOTTOM_RIGHT, position, position + float2(0,-1));
                    float2 end = get_intersection(TOP_LEFT, TOP_RIGHT, position, position + float2(0,1));
                    float linear_factor = (xy_position.y - start.y) / (end.y - start.y);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(_BOTTOM_LEFT_COLOR, _TOP_LEFT_COLOR, linear_factor);
                }
            }
            else if(is_left(TOP_RIGHT, BOTTOM_RIGHT, xy_position))
            {
                if(is_left(BOTTOM_RIGHT, BOTTOM_LEFT, xy_position))
                {
                    return _BOTTOM_RIGHT_COLOR;
                }
                else if(is_left(TOP_LEFT, TOP_RIGHT, xy_position))
                {
                    return _TOP_RIGHT_COLOR;
                }
                else
                {
                    float2 start = get_intersection(BOTTOM_LEFT, BOTTOM_RIGHT, position, position + float2(0,-1));
                    float2 end = get_intersection(TOP_LEFT, TOP_RIGHT, position, position + float2(0,1));
                    float linear_factor = (xy_position.y - start.y) / (end.y - start.y);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(_BOTTOM_RIGHT_COLOR, _TOP_RIGHT_COLOR, linear_factor);
                }
            }
            else
            {
                if(is_left(BOTTOM_RIGHT, BOTTOM_LEFT, xy_position))
                {
                    float2 start = get_intersection(BOTTOM_LEFT, TOP_LEFT, position, position + float2(-1,0));
                    float2 end = get_intersection(BOTTOM_RIGHT, TOP_RIGHT, position, position + float2(1,0));
                    float linear_factor = (xy_position.x - start.x) / (end.x - start.x);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(_BOTTOM_LEFT_COLOR, _BOTTOM_RIGHT_COLOR, linear_factor);
                }
                else if(is_left(TOP_LEFT, TOP_RIGHT, xy_position))
                {
                    float2 start = get_intersection(BOTTOM_LEFT, TOP_LEFT, position, position + float2(-1,0));
                    float2 end = get_intersection(BOTTOM_RIGHT, TOP_RIGHT, position, position + float2(1,0));
                    float linear_factor = (xy_position.x - start.x) / (end.x - start.x);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(_TOP_LEFT_COLOR, _TOP_RIGHT_COLOR, linear_factor);
                }
                else
                {
                    float2 start = get_intersection(BOTTOM_LEFT, TOP_LEFT, position, position + float2(-1,0));
                    float2 end = get_intersection(BOTTOM_RIGHT, TOP_RIGHT, position, position + float2(1,0));
                    float linear_factor = (xy_position.x - start.x) / (end.x - start.x);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    
                    fixed4 BOTTOM_COLOR = lerp(_BOTTOM_LEFT_COLOR, _BOTTOM_RIGHT_COLOR, linear_factor);
                    fixed4 TOP_COLOR = lerp(_TOP_LEFT_COLOR, _TOP_RIGHT_COLOR, linear_factor);
                    
                    start = get_intersection(BOTTOM_LEFT, BOTTOM_RIGHT, position, position + float2(0,-1));
                    end = get_intersection(TOP_LEFT, TOP_RIGHT, position, position + float2(0,1));
                    linear_factor = (xy_position.y - start.y) / (end.y - start.y);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(BOTTOM_COLOR, TOP_COLOR, linear_factor);
                }
            }
            
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
