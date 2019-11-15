Shader "AR/AbsoluteUVFalloff"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _ScaleX ("ScaleX", Range(0.0,10.0)) = 1.0
        _ScaleZ ("ScaleZ", Range(0.0,10.0)) = 1.0
        _FadeX ("FadeX", Range (0.001,0.499)) = 0.1
        _FadeY ("FadeY", Range (0.001,0.499)) = 0.1
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
                float2 worlduv : TEXCOORD1;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _Color;
            half _ScaleX;
            half _ScaleZ;
            half _FadeX;
            half _FadeY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // calc world uv
                o.worlduv = mul (unity_ObjectToWorld, v.vertex).xz;
                o.worlduv.x *= _ScaleX;
                o.worlduv.y *= _ScaleZ;

                // calc local uv
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color; //tex2D(_MainTex, i.worlduv);

                // set alpha by texture
                col.a = tex2D(_MainTex, i.worlduv).r * _Color.a;

                // set alpha by fadeout
                half fadeAlpha = saturate(i.uv.x / _FadeX);                   // start x
                fadeAlpha *= saturate(1-((i.uv.x - (1-_FadeX)) / _FadeX));    // end x
                fadeAlpha *= saturate(i.uv.y / _FadeY);                       // start y
                fadeAlpha *= saturate(1-((i.uv.y - (1-_FadeY)) / _FadeY));    // end y

                // use fade value as square
                col.a *= fadeAlpha * fadeAlpha;

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
