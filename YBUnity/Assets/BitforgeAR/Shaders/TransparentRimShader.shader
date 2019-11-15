Shader "AR/TransparentRimShader"
{
    Properties {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Power", Range(0.0,2.0)) = 0.8
    }
   
    SubShader {
       Tags { "RenderType"="Transparent" "Queue"="Transparent" }
       ZWrite On
       Cull Back
       Blend SrcAlpha OneMinusSrcAlpha
       
       Pass {
        ZWrite On
        ColorMask 0
       }
       
        Pass {
           
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
                
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };
           
            struct v2f {
                float4 pos : SV_POSITION;
                float rim : COLOR;
            };
           
            float _RimPower;

            v2f vert (appdata_base v) {
                v2f o;
                
                o.pos = UnityObjectToClipPos (v.vertex);
                float dotProduct = 1 - dot(v.normal, normalize(ObjSpaceViewDir(v.vertex)));
                o.rim = smoothstep(1 - _RimPower, 1.0, dotProduct);
                return o;
            }
           
            uniform float4 _Color;
            uniform float4 _RimColor;
           
            float4 frag(v2f i) : COLOR {
                return (_RimColor * i.rim) + (_Color * (1 - i.rim));
            }
           
            ENDCG
        }
    }
}