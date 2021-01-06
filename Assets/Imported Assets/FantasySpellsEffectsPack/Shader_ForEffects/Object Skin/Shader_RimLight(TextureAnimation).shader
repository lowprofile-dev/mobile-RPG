Shader "GAPH Custom Shader/Object Skin/Rim Light (With Texture Animation)" {
	Properties {
		[HDR]_Color("Color",Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
        _TexNormal("Animation Normal Map",2D) = "black"{}
		_NormalTex("Rim Normal Map",2D) = "black"{}
        _Speed("Offset Speed",Float) = 1
        _CutOut("CutOut",Range(0,1)) = 0
		[HDR]_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimScale("Rim Power", float) = 1.0
		_RimStrength("Rim Strength", float) = 1.0
		_TexStrength("Texture Strength",float) =1.0

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
                #pragma multi_compile_particles
				#pragma target 3.0

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _NormalTex;
                sampler2D _TexNormal;

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
                    float3 normal : NORMAL;
					float3 viewDir : TEXCOORD2;
				};

				struct v2f {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float2 texcoord2 : TEXCOORD1;
                    float2 texcoord3 : TEXCOORD2;
					float3 viewDir : TEXCOORD3;
					float3 normal : TEXCOORD4;
					UNITY_FOG_COORDS(5)
				};

				half4 _MainTex_ST;
				half4 _NormalTex_ST;
                half4 _TexNormal_ST;
				half4 _Color;
				half4 _RimColor;
				half _RimScale;
				half _RimStrength;
                half _Speed;
                half _CutOut;
				half _TexStrength;

				v2f vert(appdata_t v)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);

					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.texcoord2 = TRANSFORM_TEX(v.texcoord, _TexNormal);
					o.texcoord3 = TRANSFORM_TEX(v.texcoord, _NormalTex);
					o.viewDir = WorldSpaceViewDir(v.vertex);
					o.normal = UnityObjectToWorldNormal(v.vertex);
					UNITY_TRANSFER_FOG(o, o.vertex);
					return o;
				}

				half4 frag(v2f i) : SV_Target
				{
					half2 distort = UnpackNormal(tex2D(_TexNormal, i.texcoord2 * 2.0f)).rg;
                    half2 rimnormal = UnpackNormal(tex2D(_NormalTex, i.texcoord3)).rg;
					//Add rimnormal data to original normal texture value
					i.normal.xy += rimnormal.xy;

					//Animate main texture using time and distort value.
					half4 tex = tex2D(_MainTex, i.texcoord.x + distort.x / 5 - (_Speed * _Time / 8)) * _Color * 2.0f;
					tex *= tex2D(_MainTex, i.texcoord.y + distort.y / 5 + (_Speed * _Time / 8));
					tex *= tex2D(_MainTex, i.texcoord.xy + distort.xy / 9 + (_Speed * _Time / 12));
					tex *= tex2D(_MainTex, i.texcoord.xy + distort.xy / 9 - (_Speed * _Time / 12));
                    tex = pow(tex,0.9) * _TexStrength;

					//Set rim light with changed normal info
					half rim = 1.0 - saturate(dot(normalize(i.viewDir), i.normal));
					half4 res = float4(1,1,1,1);
					//Compose all data with animate tex and rim light
                    res.rgb *= saturate(_RimColor.rgb * pow(rim,_RimScale) * _RimStrength);
					res += tex * 2 * _TexStrength;
					//In now, just change alpha value using cutout value
					res.a = saturate(_RimColor.a) - _CutOut;
					UNITY_APPLY_FOG_COLOR(i.fogCoord, res, half4(0, 0, 0, 0));
					return res;
				}
				ENDCG
			}
		}
	}
}