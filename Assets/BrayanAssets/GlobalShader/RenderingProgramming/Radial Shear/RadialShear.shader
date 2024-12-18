Shader "Unlit/RadialShear"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SpeedAnim("PanningSpeed", float) = 10.0
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

            float _SpeedAnim;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {   
                i.uv -= float2(.5,.5);
                float distance = dot(i.uv,i.uv);
                float strength = distance * 10;


                float negate = -i.uv.r;
                float2 appe = float2(i.uv.g,negate) ;
                
                
                float2 DistortedUV = strength * appe  ;
                
                i.uv = _Time * 5;
                DistortedUV += i.uv + 0;
                float2 MovingDistorted = DistortedUV ;


                // sample the texture
                fixed4 col = tex2D(_MainTex, MovingDistorted);
                
                
                return float4(DistortedUV.r,DistortedUV.g,0,1);
                // return col;
            }
            ENDCG
        }
    }
}
