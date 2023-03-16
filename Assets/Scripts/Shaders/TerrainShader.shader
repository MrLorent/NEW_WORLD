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

        fixed4 get_base_color(float3 position)
        {   
            fixed4 color = fixed4(1,1,1,1);

            float2 xy_position = float2(position.x, position.z);

            float2 TOP_LEFT = float2(_Swamp_Position.x, _Swamp_Position.z);
            float TL_RADIUS = _Swamp_Radius;

            float2 TOP_RIGHT = float2(_Desert_Position.x, _Desert_Position.z);
            float TR_RADIUS = _Desert_Radius;

            float2 BOTTOM_RIGHT = float2(_Plain_Position.x, _Plain_Position.z);
            float BR_RADIUS = _Plain_Radius;

            float2 BOTTOM_LEFT = float2(_Mountain_Position.x, _Mountain_Position.z);
            float BL_RADIUS = _Mountain_Radius;

            if(is_left(BOTTOM_LEFT, TOP_LEFT, xy_position)) // xy_position is on BL_TL left ?
            {
                if(is_left(BOTTOM_RIGHT, BOTTOM_LEFT, xy_position))
                {
                    return _Mountain_Color;
                }
                else if(is_left(TOP_LEFT, TOP_RIGHT, xy_position))
                {
                    return _Swamp_Color;
                }
                else
                {
                    float2 start = get_intersection(BOTTOM_LEFT, BOTTOM_RIGHT, position, position + float2(0,-1));
                    float2 end = get_intersection(TOP_LEFT, TOP_RIGHT, position, position + float2(0,1));
                    float linear_factor = (xy_position.y - start.y) / (end.y - start.y);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(_Mountain_Color, _Swamp_Color, linear_factor);
                }
            }
            else if(is_left(TOP_RIGHT, BOTTOM_RIGHT, xy_position))
            {
                if(is_left(BOTTOM_RIGHT, BOTTOM_LEFT, xy_position))
                {
                    return _Plain_Color;
                }
                else if(is_left(TOP_LEFT, TOP_RIGHT, xy_position))
                {
                    return _Desert_Color;
                }
                else
                {
                    float2 start = get_intersection(BOTTOM_LEFT, BOTTOM_RIGHT, position, position + float2(0,-1));
                    float2 end = get_intersection(TOP_LEFT, TOP_RIGHT, position, position + float2(0,1));
                    float linear_factor = (xy_position.y - start.y) / (end.y - start.y);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(_Plain_Color, _Desert_Color, linear_factor);
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
                    return lerp(_Mountain_Color, _Plain_Color, linear_factor);
                }
                else if(is_left(TOP_LEFT, TOP_RIGHT, xy_position))
                {
                    float2 start = get_intersection(BOTTOM_LEFT, TOP_LEFT, position, position + float2(-1,0));
                    float2 end = get_intersection(BOTTOM_RIGHT, TOP_RIGHT, position, position + float2(1,0));
                    float linear_factor = (xy_position.x - start.x) / (end.x - start.x);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    return lerp(_Swamp_Color, _Desert_Color, linear_factor);
                }
                else
                {
                    float2 start = get_intersection(BOTTOM_LEFT, TOP_LEFT, position, position + float2(-1,0));
                    float2 end = get_intersection(BOTTOM_RIGHT, TOP_RIGHT, position, position + float2(1,0));
                    float linear_factor = (xy_position.x - start.x) / (end.x - start.x);
                    linear_factor = linear_factor * linear_factor * (3.0F - 2.0F * linear_factor);
                    
                    fixed4 BOTTOM_COLOR = lerp(_Mountain_Color, _Plain_Color, linear_factor);
                    fixed4 TOP_COLOR = lerp(_Swamp_Color, _Desert_Color, linear_factor);
                    
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
