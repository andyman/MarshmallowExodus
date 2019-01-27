Shader "Custom/CandyStripedShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _AltColor("AltColor", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BumpMap ("Normalmap", 2D) = "bump" {}

        _FacingX("FacingX", Float) = 1
        _FacingY("FacingY", Float) = 0

        _FacingZ("FacingZ", Float) = 1
        
        _StripeBias("StripeBias", Float) = -0.5
        _StripeFrequency("StripeFrequency", Float) = 1
        
        _Cutoff("Cutoff", Float) = 0.5

        _StripeNoiseFrequency("StripeNoiseFrequency", Float) = 1.0
        _StripeNoiseAmplitude("StripeNoiseAmplitude", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _AltColor;
        float _FacingX, _FacingY, _FacingZ;
        float _StripeNoiseFrequency;
        float _StripeNoiseAmplitude;
        float _StripeBias, _StripeFrequency;
        float _Cutoff;
        
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        // Modulo 289 without a division (only multiplications)
        float  mod289(float x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float2 mod289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float3 mod289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float4 mod289(float4 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        // Permutation polynomial: (34x^2 + x) math.mod 289
        float  permute(float x) { return mod289((34.0 * x + 1.0) * x); }
        float3 permute(float3 x) { return mod289((34.0 * x + 1.0) * x); }
        float4 permute(float4 x) { return mod289((34.0 * x + 1.0) * x); }

        float snoise(float2 v)
        {
            float4 C = float4(0.211324865405187,  // (3.0-math.sqrt(3.0))/6.0
                0.366025403784439,  // 0.5*(math.sqrt(3.0)-1.0)
                -0.577350269189626,  // -1.0 + 2.0 * C.x
                0.024390243902439); // 1.0 / 41.0
            // First corner
            float2 i = floor(v + dot(v, C.yy));
            float2 x0 = v - i + dot(i, C.xx);

            // Other corners
            float2 i1;
            //i1.x = math.step( x0.y, x0.x ); // x0.x > x0.y ? 1.0 : 0.0
            //i1.y = 1.0 - i1.x;
            i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
            // x0 = x0 - 0.0 + 0.0 * C.xx ;
            // x1 = x0 - i1 + 1.0 * C.xx ;
            // x2 = x0 - 1.0 + 2.0 * C.xx ;
            float4 x12 = x0.xyxy + C.xxzz;
            x12.xy -= i1;

            // Permutations
            i = mod289(i); // Avoid truncation effects in permutation
            float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0)) + i.x + float3(0.0, i1.x, 1.0));

            float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
            m = m * m;
            m = m * m;

            // Gradients: 41 points uniformly over a line, mapped onto a diamond.
            // The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)

            float3 x = 2.0 * frac(p * C.www) - 1.0;
            float3 h = abs(x) - 0.5;
            float3 ox = floor(x + 0.5);
            float3 a0 = x - ox;

            // Normalise gradients implicitly by scaling m
            // Approximation of: m *= inversemath.sqrt( a0*a0 + h*h );
            m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);

            // Compute final noise value at P

            float  gx = a0.x * x0.x + h.x * x0.y;
            float2 gyz = a0.yz * x12.xz + h.yz * x12.yw;
            float3 g = float3(gx, gyz);

            return 130.0 * dot(m, g);
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            
            // stripe
            float3 stripePos = IN.worldPos + float3(10000, 0, -10000);
            stripePos.x += snoise(IN.worldPos.xz*_StripeNoiseFrequency) * _StripeNoiseAmplitude;
            stripePos.y += snoise(IN.worldPos.yz*_StripeNoiseFrequency) * _StripeNoiseAmplitude;
            
            stripePos.z += snoise(IN.worldPos.zx*_StripeNoiseFrequency) * _StripeNoiseAmplitude;
            
            float val = (
              (
                frac((stripePos.x*_FacingX + stripePos.y*_FacingY + stripePos.z*_FacingZ) * _StripeFrequency
              )
              + _StripeBias)
            );
       
            val = round(val);
            
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * lerp(_Color, _AltColor, val);            
            o.Albedo = c.rgb;
            
            
            
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpTex));

            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
