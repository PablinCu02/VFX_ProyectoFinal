Shader "Custom/GastricAcid"
{
    Properties
    {
        _MainTex ("Acid Texture (Base)", 2D) = "white"{}
        _NoiseTex ("Bubbles/Noise Texture", 2D) = "white"{}
        _Color ("Acid Color", Color) = (0.2, 0.8, 0.1, 0.6) // Verde ácido transparente
        
        [Space(10)]
        _Viscosity ("Viscosity (Distortion)", Range(1, 20)) = 10
        _FlowSpeed ("Flow Speed", Range(0, 2)) = 0.5
        
        [Space(10)]
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1
        _WaveFrequency ("Wave Frequency", Range(0, 5)) = 1
        _WaveAmplitude ("Wave Amplitude", Range(0, 0.5)) = 0.1
    }
    SubShader
    {       
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha // Permite transparencia

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
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Viscosity;
            float _FlowSpeed;
            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;

            v2f vert (appdata v)
            {
                v2f o;
                
                // Efecto de ondulación (Oleaje del ácido)
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                v.vertex.y += sin((worldPos.z + (_Time.y * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;
                v.vertex.y += cos((worldPos.x + (_Time.y * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {       
                // Distorsión para simular viscosidad
                float2 uvNoise = i.uv + (_Time.y * _FlowSpeed * 0.1);
                half distortion = tex2D(_NoiseTex, uvNoise).r;
                
                i.uv.x += (distortion - 0.5) / _Viscosity;
                i.uv.y += (distortion - 0.5) / _Viscosity;

                // Color base + textura
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // Brillo de "burbujas" usando el ruido
                col.rgb += pow(distortion, 4) * 0.5; // Crea puntos brillantes
                
                return col;
            }
            ENDCG
        }
    }
}