Shader "Unlit/FlowMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlowTex ("FlowTexture", 2D) = "white" {}
        _UVTex ("UV Texture", 2D) = "white" {}
        _FlowSpeed("Flow Speed", Vector) = (0,0,0,0)
        [HDR] _ColorFlow("ColorFlow", Color) = (1,1,1,1)
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
                float4 uv : TEXCOORD0;

                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _FlowTex;
            sampler2D _UVTex;
            
            float4 _FlowSpeed;
            float4 _ColorFlow;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the UV
                fixed4 uv = tex2D(_UVTex, i.uv.xy);
                //panning UV
                uv.rg += frac(_Time.y * _FlowSpeed.xy);
                //sample the flow
                fixed4 flow = tex2D(_FlowTex, uv.rg) ;

                // Pannning Texture
                i.uv.xy += frac(_Time.y * 3);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv.xy) ;

                fixed4 ColoredFlow = flow * _ColorFlow;

                fixed4 finalColor = lerp(col , ColoredFlow , uv.a * flow.r) ;
        
                return finalColor;
            }
            ENDCG
        }
    }
}
