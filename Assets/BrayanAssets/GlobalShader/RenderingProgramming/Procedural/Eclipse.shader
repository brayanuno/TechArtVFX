Shader "Unlit/Eclipse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tiling ("Float",float) = 5
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Tiling;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvT = frac(i.uv * float2(_Tiling,_Tiling));
                float testing = 1-(length((uvT  * 2 - 1) /(.5,.5)));
                float derivative = saturate(testing / abs(ddx(testing)) + abs(ddy(testing))) ;

                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                
                return derivative;
                
            }
            ENDCG
        }
    }
}
