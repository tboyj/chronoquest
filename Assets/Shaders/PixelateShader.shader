Shader "Custom/PixelateShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(16,512)) = 128
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Surface shader with Standard lighting + shadows
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        float _PixelSize;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Apply tiling/offset automatically
            float2 uv = IN.uv_MainTex;

            // Pixelation logic
            uv *= _PixelSize;
            uv = floor(uv) / _PixelSize;

            fixed4 c = tex2D(_MainTex, uv);

            o.Albedo = c.rgb;
            o.Metallic = 0.0;
            o.Smoothness = 0.0;
            o.Alpha = c.a;
        }
        ENDCG
    }

    FallBack "Standard"
}
