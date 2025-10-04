Shader "Custom/PixelateCameraShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(64,1024)) = 256
        _Levels ("Color Levels", Range(2,64)) = 64
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _PixelSize;
            float _Levels;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Pixelation
                float2 uv = i.uv * _PixelSize;
                uv = floor(uv) / _PixelSize;

                fixed4 col = tex2D(_MainTex, uv);

                // Posterization: reduce color levels
                col.rgb = floor(col.rgb * _Levels) / (_Levels - 1);

                return col;
            }
            ENDCG
        }
    }
}
