Shader "SciFi Weapons/Transparent Shadow Collector"
{
    Properties
    {
        _ShadowIntensity ("Shadow Intensity", Range (0, 1)) = 0.6
    }
 
 
    SubShader
    {
 
        Tags {"Queue"="Geometry" }
 
        Pass
        {
            Tags {"LightMode" = "ForwardBase" }
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
 
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            float _ShadowIntensity;
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                LIGHTING_COORDS(0,1)
            };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
             
                return o;
            }
            half4 frag(v2f i) : SV_TARGET
            {
                float attenuation = LIGHT_ATTENUATION(i);
                return half4(0,0,0,(1-attenuation)*_ShadowIntensity);
            }
            ENDCG
        }
 
    }
    Fallback "VertexLit"
}