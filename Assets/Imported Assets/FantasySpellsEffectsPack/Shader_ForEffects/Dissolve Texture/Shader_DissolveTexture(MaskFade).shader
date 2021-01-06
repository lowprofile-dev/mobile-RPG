// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GAPH Custom Shader/Dissolve Texture/Dissolve Texture(MaskFade)" {
	Properties{
		[HDR]_TintColor("Color",Color) = (1,1,1,1)
		_MainTex("Main Texture",2D) = "white"{}
		_Mask("Mask Texture",2D) = "white"{}
		_ColorFactor("Color Factor",float) = 1
		_CutOut("CutOut Factor",Range(0,1)) = 0
		_InvFade("Soft Particle Factor", Range(0.01,3.0)) = 1.0
	}
		Category{
				Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
				Blend SrcAlpha One
				Cull Off
				ZWrite Off

				SubShader {
					Pass {
						CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag
						#pragma target 3.0
						#pragma multi_compile_fog

						#include "UnityCG.cginc"

						sampler2D _MainTex;
						sampler2D _Mask;
						fixed4 _TintColor;
						float _ColorFactor;
						float _CutOut;

						struct appdata_t {
							float4 vertex: POSITION;
							half4 color : COLOR;
							float2 texcoord : TEXCOORD0;
						};

						struct v2f {
							float4 vertex : SV_POSITION;
							half4 color : COLOR;
							float2 texcoord : TEXCOORD0;
							float2 uvmask : TEXCOORD1;
							UNITY_FOG_COORDS(2)
							#ifdef SOFTPARTICLES_ON
								float4 projPos : TEXCOORD3;
							#endif
						};

						float4 _MainTex_ST;
						float4 _Mask_ST;

						v2f vert(appdata_t v)
						{
							v2f o;
							
							#ifdef SHADER_API_D3D11
								o.vertex = UnityObjectToClipPos(v.vertex);
							#else 
								o.vertex = UnityObjectToClipPos(v.vertex);
							#endif
							
							#ifdef SOFTPARTICLES_ON
								o.projPos = ComputeScreenPos(o.vertex);
								COMPUTE_EYEDEPTH(o.projPos.z);
							#endif
							o.color = v.color;
							o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
							o.uvmask = TRANSFORM_TEX(v.texcoord, _Mask);
							UNITY_TRANSFER_FOG(o, o.vertex);
							return o;
						}

						sampler2D _CameraDepthTexture;
						float _InvFade;

						fixed4 frag(v2f i) : SV_Target
						{
							#ifdef SOFTPARTICLES_ON
								float sceneZ = LinearEyeDepth(SAMPLER_DEPTH_TEXTURE_PROJ(_CameraDepthTexture,UNITY_PROJ_COORD(i.projPos)));
								float partZ = i.projPos.z;
								float fade = saturate(_InvFade *  (sceneZ - partZ));
								i.color.a *= _InvFade;
							#endif

							half4 tex = tex2D(_MainTex, i.texcoord);
							//Get mask info with cutout value for cutout texture 
							half4 mask = saturate(tex2D(_Mask, i.uvmask) - _CutOut); 

							//Set draw texture using color, color factor info
							half4 res = tex * i.color * _TintColor * _ColorFactor;
							//Set alpha using mask info
							half alpha = res.a * mask.a;
							res *= alpha;
							
							UNITY_APPLY_FOG_COLOR(i.fogCoord, res, half4(0, 0, 0, 0));
							return res;
						}
				ENDCG
			}
		}
	}
}