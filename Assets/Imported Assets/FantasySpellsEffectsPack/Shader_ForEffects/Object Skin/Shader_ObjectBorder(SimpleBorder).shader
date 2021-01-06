Shader "GAPH Custom Shader/Object Skin/Object Border (SimpleBorder)" {
	Properties {
        [HDR]_Color("Color",Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _Cutout("_Cutout",Range(0,1)) = 0
        _CutoutBorder("_Cutout Border",Range(0,1)) = 0.02
        [HDR]_BorderColor("Border Color", Color) = (1,1,1,1)
    }
    Category
    {
        Tags{ "RenderType"="Tranparent"  "IgnoreProjector" = "True"}
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

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                float4 _MainTex_ST;
                half4 _Color;
                half4 _BorderColor;
                half _Cutout;
                half _CutoutBorder;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 tex = tex2D(_MainTex, i.texcoord) * _Color;
                    half4 res = tex;

                    clip(tex.a - _Cutout);
                    if(tex.a < _Cutout + _CutoutBorder)
                        res = _BorderColor;
                    else
                        res = tex;

                    return res;
                }
                ENDCG

            }
        }

    }
}
