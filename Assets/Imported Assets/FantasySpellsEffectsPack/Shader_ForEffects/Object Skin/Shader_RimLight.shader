Shader "GAPH Custom Shader/Object Skin/Rim Light" {
    Properties {
        [HDR]_Color("Color",Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _NormalTex("Normal Texture",2D) = "black"{}
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimScale("Rim Power", Range(0.1,8.0)) = 1.0
        _RimStrenth("Rim Strength", float) = 1.0

        [Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc ("BlendSrc", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendDst ("BlendDst", float) = 1
    }
        Category
        {
            Tags{ "Queue" = "Transparent" "RenderType" = "Tranparent"  "IgnoreProjector" = "True" }
            Blend [_BlendSrc] [_BlendDst]
            ColorMask RGB
            LOD 200

            SubShader {
                Pass{

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0

                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _NormalTex;

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    float2 texcoord2 : TEXCOORD1;
                    float3 viewDir : TEXCOORD2;
                };

                struct v2f {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    float2 texcoord2 : TEXCOORD1;
                    float3 viewDir : TEXCOORD2;
                    float3 normal : TEXCOORD3;
					UNITY_FOG_COORDS(4)
                };

                float4 _MainTex_ST;
                float4 _NormalTex_ST;
                half4 _Color;
                half4 _RimColor;
                half _RimScale;
                half _RimStrenth;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.texcoord2 = TRANSFORM_TEX(v.texcoord2, _NormalTex);
                    o.viewDir = WorldSpaceViewDir(v.vertex);
                    o.normal = UnityObjectToWorldNormal(v.vertex);
					UNITY_TRANSFER_FOG(o, o.vertex);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half2 distort = UnpackNormal(tex2D(_NormalTex, i.texcoord2)).rg;
                    i.normal.xy += distort.xy;
                    half4 tex = tex2D(_MainTex, i.texcoord.xy + distort.xy) * _Color;

                    half rim = pow(1.0 - saturate(dot(normalize(i.viewDir), i.normal)),_RimScale);
                    half4 res = float4(0,0,0,0);
                    res.rbg += tex.rgb + _RimColor.rgb * rim * _RimStrenth;
                    res.a += tex.a;
                    res.a += _RimColor.a * rim;

					UNITY_APPLY_FOG_COLOR(i.fogCoord, res, half4(0, 0, 0, 0));
                    return res;
                }
                ENDCG
            }
        }
    }
}