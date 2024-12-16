Shader "Unlit/Abso"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("ColorLight" , Color) = (1,1,1,1) 
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
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float3 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {   
                // float3 = i.normal;
                // half3 col = i.normalWS.xyz;
                // sample the texture
                // float grad = abs(dot(i.normal,normalize(_WorldSpaceLightPos0.xyz)));
                
                float grad = abs(dot( _WorldSpaceLightPos0 , i.normal));
                float3 col = grad * _Color;
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog

                return float4(col ,1);
            }
            ENDCG
        }
    }
}
