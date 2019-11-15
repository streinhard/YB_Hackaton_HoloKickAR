Shader "AR/AbsoluteUV"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _ScaleX ("ScaleX", Range(0.0,10.0)) = 1.0
        _ScaleZ ("ScaleZ", Range(0.0,10.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+1" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            uniform float4 _Color;
            half _ScaleX;
            half _ScaleZ;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul (unity_ObjectToWorld, v.vertex).xz;
                o.uv.x *= _ScaleX;
                o.uv.y *= _ScaleZ;
                //o.uv = TRANSFORM_TEX (mul (unity_ObjectToWorld, v.vertex), _MainTex).xy;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            } 

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color; //tex2D(_MainTex, i.uv);
                col.a = tex2D(_MainTex, i.uv).r * _Color.a;
                
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}