Shader "Custom/URP_VertexColorSimplePBR"
{
    Properties
    {
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Roughness (Smoothness)", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline" 
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            float _Metallic;
            float _Smoothness;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 color      : COLOR;
                float2 texcoord   : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float4 color       : COLOR;
                float fogCoord     : TEXCOORD2;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(v.normalOS);
                
                o.positionHCS = vertexInput.positionCS;
                o.positionWS = vertexInput.positionWS;
                o.normalWS = normalInput.normalWS;
                o.color = v.color;
                o.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Нормализация нормали (может быть интерполирована)
                float3 normalWS = normalize(i.normalWS);
                
                // Альбедо из vertex color
                half3 albedo = i.color.rgb;
                
                // Основной свет
                Light mainLight = GetMainLight();
                
                // Освещение
                half3 lighting = LightingLambert(mainLight.color, mainLight.direction, normalWS);
                
                // Металличность и гладкость
                half metallic = _Metallic;
                half smoothness = _Smoothness;
                
                // Простая модель освещения с металличностью
                half3 diffuse = albedo * lighting;
                
                // Простой расчет отражений
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.positionWS);
                float3 reflectDir = reflect(-mainLight.direction, normalWS);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0 * smoothness + 1.0);
                half3 specular = metallic * spec * mainLight.color;
                
                // Комбинируем диффузное и зеркальное отражение
                half3 color = diffuse * (1.0 - metallic) + specular;
                
                // Добавляем окружающее освещение
                color += SampleSH(normalWS) * albedo * 0.5;
                
                // Применяем туман
                color = MixFog(color, i.fogCoord);
                
                return half4(color, 1.0);
            }

            ENDHLSL
        }
    }
}