Shader "Custom/HealthBarShader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0, 1, 0, 1)
        _BackgroundColor ("Background Color", Color) = (1, 0, 0, 1)
        _Progress ("Progress", Range(0, 1)) = 1
        _AnimationSpeed ("Animation Speed", Range(0.1, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainColor;
            float4 _BackgroundColor;
            float _Progress;
            float _AnimationSpeed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float smoothValue(float current, float target, float speed, float deltaTime)
            {
                return lerp(current, target, 1 - exp(-speed * deltaTime));
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Time is a built-in variable in Unity
                float deltaTime = _Time.y; // _Time.y is the time elapsed since the last frame
                static float currentProgress = 0.0;

                // Smoothly interpolate progress
                currentProgress = smoothValue(currentProgress, _Progress, _AnimationSpeed, deltaTime);

                // Select color based on the current progress
                if (i.uv.x < currentProgress)
                    return _MainColor;
                else
                    return _BackgroundColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
