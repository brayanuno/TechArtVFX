Shader "Unlit/ShaderManual"
{
    Properties
    {
        //Property Created
        _MainTexture("Main Texture", 2D) = "white" {}
        _PanningX("Animate X",float) = 0
        _PanningY("Animate Y",float) = 0
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

            sampler2D _MainTexture;
            
            float4 _MainTexture_ST;
            float _PanningX;
            float _PanningY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
                float ScaleU = _MainTexture_ST.x * _PanningX;
                float ScaleV = _MainTexture_ST.y * _PanningY;

                
                o.uv += frac(float2(ScaleU,ScaleV) * _Time.xy);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
 

                fixed4 textureColor = tex2D(_MainTexture,i.uv);
                return textureColor;
            }
            ENDCG
        }
    }
}
