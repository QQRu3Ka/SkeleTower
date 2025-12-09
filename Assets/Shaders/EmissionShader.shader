Shader "Custom/URP_VertexColorOnlyEmission"
{
    Properties
    {
        _Intensity ("Emission Intensity", Range(0, 10)) = 1.0
        _UseVertexAlpha ("Use Vertex Alpha", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline" 
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 vertexColor : COLOR;
            };

            float _Intensity;
            float _UseVertexAlpha;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.vertexColor = v.color;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Берём цвет ТОЛЬКО из вершинного цвета
                half3 color = i.vertexColor.rgb;
                
                // Усиливаем интенсивность
                color *= _Intensity;
                
                // Используем альфа-канал для контроля прозрачности свечения
                float alpha = lerp(1.0, i.vertexColor.a, _UseVertexAlpha);
                
                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}