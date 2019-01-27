Shader "Custom/UnlitGradientVertical"
{
    Properties
    {
        _Color2 ("TopColor", Color) = (0.0,0.0,0.0,1.0)
        _Color ("BottomColor", Color) = (1.0,1.0,1.0,1.0)

        _TopPos("TopPos", Range(-2.0,2.0)) = 1.0
        _BottomPos("BottomPos", Range(-2.0,2.0)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            fixed4 _Color;
            fixed4 _Color2;

            fixed _TopPos;
            fixed _BottomPos;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 t = saturate((i.uv.y - _BottomPos) / (_TopPos - _BottomPos));
                t = smoothstep(0.0, 1.0, t);
                fixed4 col = lerp(_Color, _Color2, t);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
