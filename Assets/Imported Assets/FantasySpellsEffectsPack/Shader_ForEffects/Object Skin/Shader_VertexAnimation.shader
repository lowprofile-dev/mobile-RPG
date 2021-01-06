Shader "GAPH Custom Shader/Object Skin/Vertex Animation" {
	Properties {
		[HDR]_Color("Color",Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_NoiseMap("Vertex Animation Noise Map",2D) = "black"{}
		_TexNormal("Texture Animation Normal Map",2D) = "black"{}
        _Speed("Offset Speed",Float) = 1
        _NoiseValue("Noise Scale", Vector) = (1,1,1,0)
		_NoiseScale("Noise Scale", float) = 1
        _ColorStrength("Color Strength", float) = 1.0

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
				sampler2D _NoiseMap;
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
					float3 viewDir : TEXCOORD2;
					float3 normal : TEXCOORD3;
					UNITY_FOG_COORDS(4)
				};

				half4 _MainTex_ST;
                half4 _TexNormal_ST;
				half4 _Color;
                half4 _NoiseValue;
				half _NoiseScale;
                half _Speed;
                half _ColorStrength;

				v2f vert(appdata_t v)
				{
					v2f o;
                    
					//Change local pos to wold and set noise value
					float4 Noise = mul(unity_ObjectToWorld, v.vertex) * _NoiseValue * float4(0.1f, 0.1f, 1.5f, 1);
					//Set noiseTex with normal tex using time & scale info. time is for animate vertex
					float4 NoiseTex = tex2Dlod(_NoiseMap, Noise + float4(float3(_Time.x / 2, _Time.y / 2, _Time.z * 2) * _NoiseScale * 10, 0));
					v.vertex.xyz +=
						v.normal* _NoiseScale +
					//Add changed noise info with normal value to original object vertex.
					(NoiseTex.xyz * _NoiseValue.w - _NoiseValue.w / 2) * (
					//Additionally trigonometric value to original object vertex.
					sin((v.vertex.x + _Time * _NoiseValue.x)*_NoiseValue.y) +
					cos((v.vertex.y + _Time * _NoiseValue.x)*_NoiseValue.y) +
					sin((v.vertex.z + _Time * _NoiseValue.x)*_NoiseValue.y)
					)*_NoiseValue.z*_NoiseScale * 10;

					o.vertex = UnityObjectToClipPos(v.vertex);

					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.texcoord2 = TRANSFORM_TEX(v.texcoord, _TexNormal);
					o.viewDir = WorldSpaceViewDir(v.vertex);
					o.normal = UnityObjectToWorldNormal(v.vertex);
					UNITY_TRANSFER_FOG(o, o.vertex);

					return o;
				}

				half4 frag(v2f i) : SV_Target
				{
					half2 distort = UnpackNormal(tex2D(_TexNormal, i.texcoord2)).rg;
					//Animate main texture using time and distort value.
					half4 tex = tex2D(_MainTex, i.texcoord.xy + distort.x / 6 - (_Speed * _Time / 8)) * _Color;
					tex *= tex2D(_MainTex, i.texcoord.xy + distort.y / 4 + (_Speed * _Time.xx / 12));
                    tex = pow(tex,0.8);

					//Compose all data with animate tex and rim light
                    tex *= _Color*_ColorStrength;
					UNITY_APPLY_FOG_COLOR(i.fogCoord, tex, half4(0, 0, 0, 0));
					return tex;
				}
				ENDCG
			}
		}
	}
}