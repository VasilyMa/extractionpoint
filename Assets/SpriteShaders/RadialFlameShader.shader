Shader "Custom/RadialFlameFinal" {
    Properties {
        _FlameTex ("Flame Texture", 2D) = "white" {}   // �������� ������ �������
        _NoiseTex ("Noise Texture", 2D) = "white" {}   // �������� ���� ��� ���������� ��������
        _Distortion ("Distortion Strength", Float) = 0.1 // ���� ���������
        _Speed ("Speed", Float) = 1.0                  // �������� ��������
        _FlameCount ("Flame Count", Float) = 8.0       // ���������� ������
        _EdgeFade ("Edge Fade", Range(0, 1)) = 0.7     // ������ �������� �� �����
        _Brightness ("Brightness", Float) = 1.0         // ������� �������
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)       // �������� ���� (�������)
        _Color2 ("Color 2", Color) = (1, 1, 0, 1)       // ��������� ���� (�����)
    }
    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True"
        }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // ������������
        ZWrite Off                      // �� ���������� � ����� �������
        Cull Off                        // �������� � ����� ������

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

            // ��������� ������
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // ������� ��� ���������� ��������� ����� ���
            float randomNoise(float2 p) {
                return tex2D(_NoiseTex, p * 0.5 + _Time.y * _Speed).r;
            }

            // ����������� ������
            fixed4 frag(v2f i) : SV_Target {
                // ���������� UV-���������� (0.5 � ����� ��������)
                float2 centered = i.uv - float2(0.5, 0.5);

                // ��������� � �������� ���������� (������ � ����)
                float radius = length(centered);
                float angle = atan2(centered.y, centered.x);

                // ��������� �������� �� ����
                angle *= _FlameCount;

                // ��������� ��������� ��������� �� �������� ����
                float noise = randomNoise(centered * 2.0);
                angle += sin(radius * 10.0 + _Time.y * _Speed + noise * 5.0) * _Distortion;
                radius += sin(_Time.y + noise * 2.0) * (_Distortion * 0.1);

                // ����������� ������� � ��������� ����������
                float2 distortedUV;
                distortedUV.x = cos(angle) * radius + 0.5;
                distortedUV.y = sin(angle) * radius + 0.5;

                // ���������� ��������
                fixed4 color = tex2D(_FlameTex, distortedUV);

                // ������������ ����� ����� ������� �� ������ ������� � �������
                float timeLerp = sin(_Time.y * 2.0) * 0.5 + 0.5;
                float radiusLerp = smoothstep(0.2, 0.8, radius);
                float4 flameColor = lerp(_Color1, _Color2, timeLerp * radiusLerp);

                // ��������� ���� � ��������
                color.rgb *= flameColor.rgb;

                // ? ������������ ��������� �� ������ � ���� (0 � ������ ? 1 �� ����)
                float fade = saturate(1.0 - (radius / _EdgeFade));

                // ��������� ������ ��������� � �������
                color.a *= fade;
                color.rgb *= _Brightness;

                return color;
            }
            ENDCG
        }
    }
}
