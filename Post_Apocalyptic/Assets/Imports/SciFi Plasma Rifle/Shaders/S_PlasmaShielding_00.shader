Shader "SciFi Weapons/Plasma Shielding"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [NoScaleOffset]_BumpMap ("Normal Map", 2D) = "normal" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        [NoScaleOffset]_MetallicGlossMap ("Metallic Gloss Map", 2D) = "white" {}
        [NoScaleOffset]_EmissionMap ("Emission", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        [Header(FX Settings)]
        _FXMap("FX Texture", 2D) = "black" {}
        _FXNormal ("FX Normal", 2D) = "normal" {}
        _FXNormalStrength("FX Normal Strength",float) = 0.0
        _FXEmissionColor ("FX Emission Color", Color) = (1,1,1,1)
        _FlowParams("Speed, Emission 1, Emission 2, Alpha", Vector) = (1,1,1,1)
        _FXParams("Fresnel Mult, Fresnel Pow, Distort, Alpha", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "ForceNoShadowCasting"="True"}
        LOD 200
        ZWrite Off
        Cull Back

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex, _EmissionMap, _MetallicGlossMap, _BumpMap, _FXMap, _FXNormal;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_FXMap;
            float2 uv_FXNormal;
            float3 viewDir;
        };

        half _Glossiness, _Metallic, _FXNormalStrength;
        half4 _Color, _EmissionColor, _FlowParams, _FXEmissionColor, _FXParams;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //fx block
            float fresnel = 1 - saturate(pow(max(dot( normalize(IN.viewDir.xyz), normalize(o.Normal)), 0.0) * _FXParams.x, _FXParams.y));

            half4 fx = tex2D (_FXMap, IN.uv_FXMap + _Time[0] * _FlowParams.x);
            half4 fx2 = tex2D (_FXMap, IN.uv_FXMap - _Time[0] * _FlowParams.x);
            half4 fxEmission = (fx.r * _FlowParams.y + fx2.b * _FlowParams.z + fresnel) * _FXEmissionColor;

            half fxAlpha = saturate(fx.g + fx2.a) * _FlowParams.w;

            half3 fxNormal = UnpackNormal(tex2D (_FXNormal, IN.uv_FXNormal + _Time[0] * _FlowParams.x)) * _FXNormalStrength;

            
            // Albedo comes from a texture tinted by color
            half4 c = tex2D (_MainTex, IN.uv_MainTex + fxNormal.xy * _FXParams.z) * _Color;
            half4 mSmooth = tex2D (_MetallicGlossMap, IN.uv_MainTex);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic * mSmooth.r;
            o.Smoothness = _Glossiness * mSmooth.a;
            o.Normal = normalize(UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex)) + fxNormal);

            o.Alpha = saturate((c.a + Luminance(fxEmission)) * fxAlpha + fresnel * _FXParams.w);
            o.Emission = saturate(tex2D (_EmissionMap, IN.uv_MainTex) * _EmissionColor + fxEmission);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
