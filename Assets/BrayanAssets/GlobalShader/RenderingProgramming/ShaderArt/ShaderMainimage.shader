Shader "Unlit/Mainimage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _speedAnim ("SpeedAnim", float) = 1.0
        [HDR] _Color ("Color", Color) = (1,1,1,1)

        _tileGrad ("Tileable", float) = 8.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            float _speedAnim ;
            float _tileGrad;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }
            void MainImage() 
            {

            }
            
            fixed3 palette( float t) {
                float3 a = float3(0.5, 0.5 ,0.5);
                float3 b = float3(0.5, 0.5,0.5);
                float3 c = float3(1.0,1.0,1.0);
                float3 d = float3(0.263,0.416,0.557);
                
                return a + b * cos(6.28318 * (c * t + d)) ;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //The UVS     
                float2 uv = i.uv;
                uv *= _tileGrad;
                uv = frac(uv);
                uv -= .5;
                // uv -= 1;

                float grad = length(uv);

                grad = sin(grad * 12 + 3 * (_Time * _speedAnim ))/.9;

                grad -= .5;
                grad = abs(grad);

                grad = .1/ grad;
                // grad = saturate(grad);

                grad = smoothstep(0.0, 1, grad);

                fixed3 col = palette(grad + _Time * 40) ;

                col *= grad;
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
               

                // fixed4 col =  d * _Color;

                return float4(col,1.0);
            }


            
            ENDCG
        }
    }
}
