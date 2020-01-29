Shader "Custom/Terrain"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
            float4 color : COLOR;
        };

        float2 hash( float2 p ) // replace this by something better
        {
            p = float2( dot(p,float2(127.1,311.7)), dot(p,float2(269.5,183.3)) );
            return -1.0 + 2.0*frac(sin(p)*43758.5453123);
        }

        float noise( in float2 p )
        {
            p = p / 5;
            const float K1 = 0.366025404; // (sqrt(3)-1)/2;
            const float K2 = 0.211324865; // (3-sqrt(3))/6;

            float2  i = floor( p + (p.x+p.y)*K1 );
            float2  a = p - i + (i.x+i.y)*K2;
            float m = step(a.y,a.x); 
            float2  o = float2(m,1.0-m);
            float2  b = a - o + K2;
            float2  c = a - 1.0 + 2.0*K2;
            float3  h = max( 0.5-float3(dot(a,a), dot(b,b), dot(c,c) ), 0.0 );
            float3  n = h*h*h*h*float3( dot(a,hash(i+0.0)), dot(b,hash(i+o)), dot(c,hash(i+1.0)));
            return dot( n, float3(70, 70, 70) );
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            o.Albedo = IN.color + float4(1, 1, 0.5f, 1) * noise(IN.worldPos.xz) / 25 - float4(1, 1, 0.5f, 1) * noise(IN.worldPos.xy) / 25;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
