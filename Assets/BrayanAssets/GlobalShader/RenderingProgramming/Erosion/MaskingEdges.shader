Shader "Unlit/MaskingEdges"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src Factor",float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DestFactor("Dst Factor",float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation",float) = 0

        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask",2D) = "white" {}
        _Erosion ("Erosion",float) = 0

        _EdgeThickness ("EdgeThickness", Range(0,.3)) = 0.0
        [HDR] _ColorEdge("EdgeColor",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend [_SrcFactor] [_DestFactor] 
        BlendOp [_Opp]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            //From mesh get info
            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MaskTex;
            float4 _MaskTex_ST;

            float _Erosion;

            float _EdgeThickness;
            
            float3 _ColorEdge;
   
            //Vertex - Sampling every vertexs
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex);
                return o;
            }

            //Fragment - Sampling every Texture
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv.xy);

                fixed4 mask = tex2D(_MaskTex, i.uv.zw);

                // Contrast the mask
                float MaskContrasted = pow(mask.r ,.7);

                //Reveal Anim
                float revealAnim = sin(_Time.y * 2) * 0.5 + 0.5;

                //Calculating ErosionEdge
                float erodeTop = step(mask.r,_Erosion + _EdgeThickness);
                float erodeBottom = step(mask.r,_Erosion - _EdgeThickness);
                float EdgeAmount = erodeTop - erodeBottom;


                // finalColor
                float3 finalColor = lerp(col.rgb , _ColorEdge , EdgeAmount);

                // finalAlpha
                float finalAlpha = col.a * erodeTop;

                
                return fixed4(finalColor,finalAlpha);
  
            }



            ENDCG
        }
    }
}
