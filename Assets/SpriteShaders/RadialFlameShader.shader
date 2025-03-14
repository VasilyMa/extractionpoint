Shader "Custom/RadialFlameFinal" {
    Properties {
        _FlameTex ("Flame Texture", 2D) = "white" {}   // Текстура языков пламени
        _NoiseTex ("Noise Texture", 2D) = "white" {}   // Текстура шума для хаотичного движения
        _Distortion ("Distortion Strength", Float) = 0.1 // Сила искажения
        _Speed ("Speed", Float) = 1.0                  // Скорость движения
        _FlameCount ("Flame Count", Float) = 8.0       // Количество языков
        _EdgeFade ("Edge Fade", Range(0, 1)) = 0.7     // Начало размытия по краям
        _Brightness ("Brightness", Float) = 1.0         // Яркость пламени
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)       // Основной цвет (красный)
        _Color2 ("Color 2", Color) = (1, 1, 0, 1)       // Вторичный цвет (жёлтый)
    }
    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True"
        }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // Прозрачность
        ZWrite Off                      // Не записывать в буфер глубины
        Cull Off                        // Рисовать с обеих сторон

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _FlameTex;
            sampler2D _NoiseTex;
            float4 _FlameTex_ST;
            float _Distortion;
            float _Speed;
            float _FlameCount;
            float _EdgeFade;
            float _Brightness;
            float4 _Color1;
            float4 _Color2;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Вершинный шейдер
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Функция для хаотичного искажения через шум
            float randomNoise(float2 p) {
                return tex2D(_NoiseTex, p * 0.5 + _Time.y * _Speed).r;
            }

            // Фрагментный шейдер
            fixed4 frag(v2f i) : SV_Target {
                // Центрируем UV-координаты (0.5 — центр текстуры)
                float2 centered = i.uv - float2(0.5, 0.5);

                // Переводим в полярные координаты (радиус и угол)
                float radius = length(centered);
                float angle = atan2(centered.y, centered.x);

                // Повторяем текстуру по углу
                angle *= _FlameCount;

                // Добавляем рандомное искажение из текстуры шума
                float noise = randomNoise(centered * 2.0);
                angle += sin(radius * 10.0 + _Time.y * _Speed + noise * 5.0) * _Distortion;
                radius += sin(_Time.y + noise * 2.0) * (_Distortion * 0.1);

                // Преобразуем обратно в декартовы координаты
                float2 distortedUV;
                distortedUV.x = cos(angle) * radius + 0.5;
                distortedUV.y = sin(angle) * radius + 0.5;

                // Семплируем текстуру
                fixed4 color = tex2D(_FlameTex, distortedUV);

                // Интерполяция между двумя цветами на основе времени и радиуса
                float timeLerp = sin(_Time.y * 2.0) * 0.5 + 0.5;
                float radiusLerp = smoothstep(0.2, 0.8, radius);
                float4 flameColor = lerp(_Color1, _Color2, timeLerp * radiusLerp);

                // Добавляем цвет к текстуре
                color.rgb *= flameColor.rgb;

                // ? Исправленное затухание от центра к краю (0 в центре ? 1 на краю)
                float fade = saturate(1.0 - (radius / _EdgeFade));

                // Применяем эффект затухания и яркости
                color.a *= fade;
                color.rgb *= _Brightness;

                return color;
            }
            ENDCG
        }
    }
}
