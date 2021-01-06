Shader "GAPH Custom Shader/Object Skin/Object Border (NormalBorder)" {
	Properties {
        [HDR]_Color("Color",Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _NormalTex("Normal Texture",2D) = "black"{}
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
                sampler2D _NormalTex;

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    float2 texcoord2 : TEXCOORD1;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                    float2 texcoord2 : TEXCOORD1;
                };

                float4 _MainTex_ST;
                float4 _NormalTex_ST;
                half4 _Color;
                half4 _BorderColor;
                half _Cutout;
                half _CutoutBorder;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.texcoord2 = TRANSFORM_TEX(v.texcoord2,_NormalTex);

                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half2 distort = UnpackNormal(tex2D(_NormalTex, i.texcoord2)).rg;
                    half4 tex = tex2D(_MainTex, i.texcoord);
                    half4 tex2 =  tex2D(_MainTex, i.texcoord.xy + distort.xy) * _Color;
                    half4 res = tex;

                    clip(tex.a - _Cutout);
					
                    if(tex.a < _Cutout + _CutoutBorder)
                        res = _BorderColor;
                    else
                        res = tex2;

                    return res;
                }
                ENDCG

            }
        }

    }
}
