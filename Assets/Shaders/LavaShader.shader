Shader "Custom/URP_Emission"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0,1)
        _EmissionStrength ("Emission Strength", Range(0, 10)) = 1
        
        // Новые свойства для управления насыщенностью
        _Saturation ("Saturation", Range(0, 2)) = 1.0
        _AlbedoSaturation ("Albedo Saturation", Range(0, 2)) = 1.0
        _EmissionSaturation ("Emission Saturation", Range(0, 2)) = 1.0
        _EnableSaturation ("Enable Saturation", Range(0, 1)) = 1.0
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
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                float fogFactor : TEXCOORD3;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _EmissionColor;
            float _EmissionStrength;
            
            // Новые переменные для насыщенности
            float _Saturation;
            float _AlbedoSaturation;
            float _EmissionSaturation;
            float _EnableSaturation;

            // Функция для коррекции насыщенности
            half3 AdjustSaturation(half3 color, float saturation)
            {
                // Рассчитываем яркость (luminance)
                half luminance = dot(color, half3(0.299, 0.587, 0.114));
                
                // Интерполируем между монохромным (яркость) и насыщенным цветом
                half3 saturatedColor = lerp(half3(luminance, luminance, luminance), color, saturation);
                
                return saturatedColor;
            }

            Varyings vert(Attributes v)
            {
                Varyings o;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(v.normalOS);
                
                o.positionHCS = vertexInput.positionCS;
                o.positionWS = vertexInput.positionWS;
                o.normalWS = normalInput.normalWS;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Алиас для удобства
                float2 uv = i.uv;
                float3 normalWS = normalize(i.normalWS);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.positionWS);
                
                // Основной цвет из текстуры
                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                
                // Применяем насыщенность к альбедо (если включено)
                half3 finalAlbedo = albedo.rgb;
                if (_EnableSaturation > 0.5)
                {
                    // Применяем общую насыщенность и отдельную для альбедо
                    float albedoSat = _Saturation * _AlbedoSaturation;
                    finalAlbedo = AdjustSaturation(finalAlbedo, albedoSat);
                }
                
                // Освещение (простая модель)
                Light mainLight = GetMainLight();
                float NdotL = max(dot(normalWS, mainLight.direction), 0.0);
                half3 lighting = mainLight.color * NdotL;
                
                // Добавляем ambient освещение
                half3 ambient = SampleSH(normalWS);
                
                // Комбинируем освещение с альбедо
                half3 color = finalAlbedo * (lighting + ambient);
                
                // Добавляем свечение
                half3 emission = _EmissionColor.rgb * _EmissionStrength;
                
                // Применяем насыщенность к свечению (если включено)
                if (_EnableSaturation > 0.5)
                {
                    // Применяем общую насыщенность и отдельную для эмиссии
                    float emissionSat = _Saturation * _EmissionSaturation;
                    emission = AdjustSaturation(emission, emissionSat);
                }
                
                color += emission;
                
                // Применяем туман
                color = MixFog(color, i.fogFactor);
                
                return half4(color, albedo.a);
            }
            ENDHLSL
        }
        
        // Добавляем пасс для отображения в UV-редакторе
        Pass
        {
            Name "DepthOnly"
            Tags {"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings DepthOnlyVertex(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 DepthOnlyFragment(Varyings i) : SV_TARGET
            {
                return 0;
            }
            ENDHLSL
        }
    }
    
    // Fallback для совместимости
    FallBack "Universal Render Pipeline/Lit"
}