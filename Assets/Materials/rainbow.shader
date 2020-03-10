Shader "Custom/rainbow"
{
    Properties
    {
//        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
//        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                //--------------------------------------------------
                // Center cardinal coordinate system with square resolution:
                float2 uv =  (i.vertex - 0.5 * _ScreenParams.xy )/ _ScreenParams.y;
                
                //-------------------------------
                // Set up polar coordinate system:
                float r = length(uv) * 2.0; // *2 so top/bottom of screen is +-1, not +-1/2
                float t = atan2(uv.x, uv.y);
                float2 polar = float2(r, t);
            
                //-------------------------
                // Time varying pixel color
                float3 col = 0.5 + 0.5 * cos(_Time.y + polar.xyx + float3(0, 2, 4));
            
                //-----------------
                // Output to screen
                float4 fragColor = float4(col, 1.0);
                
                return fragColor;
            }
            ENDCG
        }
    }
}
