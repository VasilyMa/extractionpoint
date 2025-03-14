Shader "Custom/ToonGoldWithLine"
{
    Properties
    {
        [Header(Base Color)]
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        [HDR][MainColor]_BaseColor("Base Color", Color) = (1, 0.84, 0, 1)
        _ColorIntensity("Color Intensity", Range(0, 5)) = 1.0

        [Header(Shine Line)]
        _LineWidth("Line Width", Range(0.0001, 1)) = 0.1
        _LineSharpness("Line Sharpness", Range(0.0001, 1)) = 0.1
        _LineIntensity("Line Intensity", Range(0, 1)) = 0.8
        _LineLength("Line Length", Range(0.1, 20)) = 1.0
        _LineAngle("Line Angle (degrees)", Range(-180, 180)) = 45.0
        _LineOffsetX("Line Offset X", Range(-2, 2)) = 0.0
        _LineOffsetY("Line Offset Y", Range(-2, 2)) = 0.0

        [Header(Outline)]
        _OutlineWidth("Outline Width", Range(0, 10)) = 1
        _OutlineColor("Outline Color", Color) = (0.5, 0.5, 0.5, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "IgnoreProjector" = "True"
        }

        LOD 100

        // ======== Основной шейдер ========
        Pass
        {
            Name "ToonLit"
            Tags { "LightMode" = "UniversalForwardOnly" }

            Blend One Zero
            ZWrite On
            Cull Off
            ZTest LEqual

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _BaseMap;
            float4 _BaseColor;
            float _ColorIntensity;
            float _LineWidth;
            float _LineSharpness;
            float _LineIntensity;
            float _LineLength;
            float _LineAngle;
            float _LineOffsetX;
            float _LineOffsetY;

            // ВЕРШИННЫЙ ШЕЙДЕР
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = normalize(mul(v.normal, (float3x3) unity_WorldToObject));
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            // ФРАГМЕНТНЫЙ ШЕЙДЕР
            float4 frag(v2f i) : SV_Target
            {
                // Основной цвет с учётом интенсивности
                float4 baseColor = tex2D(_BaseMap, i.uv) * _BaseColor * _ColorIntensity;

                // Получаем угол в радианах
                float angleRad = radians(_LineAngle);

                // Матрица поворота для линии
                float2x2 rotationMatrix = float2x2(
                    cos(angleRad), -sin(angleRad),
                    sin(angleRad), cos(angleRad)
                );

                // Поворачиваем вектор взгляда в зависимости от угла
                float2 rotatedViewDir = mul(rotationMatrix, i.viewDir.xy);

                // ?? Добавляем сдвиг по осям X и Y
                rotatedViewDir.x += _LineOffsetX;
                rotatedViewDir.y += _LineOffsetY;

                // Учитываем длину линии
                float lineMask = abs(rotatedViewDir.x) * _LineLength;

                // Создаём эффект линии через smoothstep
                float shineLine = smoothstep(_LineWidth, _LineWidth + _LineSharpness, lineMask);

                // Инвертируем линию (toon-стиль)
                shineLine = 1.0 - shineLine;

                // Добавляем интенсивность линии
                float3 shine = shineLine * _LineIntensity;

                // Финальный результат (Toon Base + Shine)
                float3 finalColor = baseColor.rgb + shine;

                return float4(finalColor, baseColor.a);
            }
            ENDCG
        }

        // ======== Обводка ========
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "CustomPass" }

            Blend One Zero
            ZWrite On
            Cull Front
            ZTest LEqual

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            float _OutlineWidth;
            float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 normal = normalize(mul(v.normal, (float3x3) unity_WorldToObject));
                float4 pos = v.vertex;

                // Двигаем вершины вдоль нормали для эффекта обводки
                pos.xyz += normal * _OutlineWidth * 0.01;
                o.vertex = UnityObjectToClipPos(pos);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
