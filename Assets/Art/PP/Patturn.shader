Shader "PostProcessing/Patturn"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PatturnTex ("Patturn Texture", 2D) = "white" {}

        _HighlightColor("Highlight Color", Color) = (1,1,1,1)
        _Color("Color", Color) = (0.75,0.75,0.75,1)
        _ShadowColor("Highlight Color", Color) = (0.5,0.5,0.5,1)

        _Step1("Step 1", Range(0,1)) = 0.2
        _Step2("Step 2", Range(0,1)) = 0.4

        _Granularity("Granularity", Range(20, 1000)) = 100
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
        Tags {"RenderType" = "Opaque"}
        LOD 100

        Pass
        {
            Tags {"RenderType" = "ForwardBase"}
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _PatturnTex;
            float4 _PatturnTex_ST;

            fixed4 _Color;
            fixed4 _HighlightColor;
            fixed4 _ShadowColor;

            fixed _Step1;
            fixed _Step2;
            fixed _Granularity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                o.normal = UnityObjectToWorldNormal(v.normal);

                // We need to calculate the position in clip space
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // The diffuse lighting, the normal dot-producted with the light's direction
                half nl = max(0,dot(i.normal, _WorldSpaceLightPos0.xyz));
                
                // sample texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                //divide the perspective to get the screen coordinates
                float2 wcoord = (_Granularity * i.screenPos.xy / i.screenPos.w);

                // We need to sample  the patturn texture in the screen space
                fixed4 patturn = tex2D(_PatturnTex, wcoord);

                // Finally we will be using the step() function to choose between the highlight, base and shadow color
                float highlight = step(_Step2, 0.5 * (patturn + nl));
                float lighting = step(_Step1, 0.5 * (patturn + nl));

                return col * (_HighlightColor * highlight + _Color * lighting + _ShadowColor * (1-lighting)) * (1-highlight);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
