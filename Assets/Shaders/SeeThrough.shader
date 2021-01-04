Shader "Custom/SeeThrough"
{
	Properties
	{
		_Position("Player Position", Vector) = (0.5, 0.5, 0, 0)
		_Size("Size", Float) = 1
		Vector1_6234116B("Smoothness", Range(0, 1)) = 0.5
		Vector1_BC40FEC9("Opacity", Range(0, 1)) = 1
	}
		SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Transparent"
			"Queue" = "Transparent+0"
		}

		Pass
		{
			Name "Universal Forward"
			Tags
			{
				"LightMode" = "UniversalForward"
			}

		// Render State
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		Cull Back
		ZTest LEqual
		ZWrite On
		// ColorMask: <None>


		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		// Pragmas
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x
		#pragma target 2.0
		#pragma multi_compile_fog
		#pragma multi_compile_instancing

		// Keywords
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
		#pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
		#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile _ _SHADOWS_SOFT
		#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define _NORMAL_DROPOFF_TS 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define ATTRIBUTES_NEED_TEXCOORD1
		#define VARYINGS_NEED_POSITION_WS 
		#define VARYINGS_NEED_NORMAL_WS
		#define VARYINGS_NEED_TANGENT_WS
		#define VARYINGS_NEED_VIEWDIRECTION_WS
		#define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
		#define SHADERPASS_FORWARD

		// Includes
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
		#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

		// --------------------------------------------------
		// Graph

		// Graph Properties
		CBUFFER_START(UnityPerMaterial)
		float2 _Position;
		float _Size;
		float Vector1_6234116B;
		float Vector1_BC40FEC9;
		CBUFFER_END

			// Graph Functions

			void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
			{
				Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
			}

			void Unity_Add_float2(float2 A, float2 B, out float2 Out)
			{
				Out = A + B;
			}

			void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
			{
				Out = UV * Tiling + Offset;
			}

			void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
			{
				Out = A * B;
			}

			void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
			{
				Out = A - B;
			}

			void Unity_Divide_float(float A, float B, out float Out)
			{
				Out = A / B;
			}

			void Unity_Multiply_float(float A, float B, out float Out)
			{
				Out = A * B;
			}

			void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
			{
				Out = A / B;
			}

			void Unity_Length_float2(float2 In, out float Out)
			{
				Out = length(In);
			}

			void Unity_OneMinus_float(float In, out float Out)
			{
				Out = 1 - In;
			}

			void Unity_Saturate_float(float In, out float Out)
			{
				Out = saturate(In);
			}

			void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
			{
				Out = smoothstep(Edge1, Edge2, In);
			}

			// Graph Vertex
			// GraphVertex: <None>

			// Graph Pixel
			struct SurfaceDescriptionInputs
			{
				float3 TangentSpaceNormal;
				float3 WorldSpacePosition;
				float4 ScreenPosition;
			};

			struct SurfaceDescription
			{
				float3 Albedo;
				float3 Normal;
				float3 Emission;
				float Metallic;
				float Smoothness;
				float Occlusion;
				float Alpha;
				float AlphaClipThreshold;
			};

			SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
			{
				SurfaceDescription surface = (SurfaceDescription)0;
				float _Property_6EA5BC04_Out_0 = Vector1_6234116B;
				float4 _ScreenPosition_FA01CDAC_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
				float2 _Property_9BCB7C86_Out_0 = _Position;
				float2 _Remap_F4A1407A_Out_3;
				Unity_Remap_float2(_Property_9BCB7C86_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_F4A1407A_Out_3);
				float2 _Add_C0A42221_Out_2;
				Unity_Add_float2((_ScreenPosition_FA01CDAC_Out_0.xy), _Remap_F4A1407A_Out_3, _Add_C0A42221_Out_2);
				float2 _TilingAndOffset_3501B537_Out_3;
				Unity_TilingAndOffset_float((_ScreenPosition_FA01CDAC_Out_0.xy), float2 (1, 1), _Add_C0A42221_Out_2, _TilingAndOffset_3501B537_Out_3);
				float2 _Multiply_6D84CB8F_Out_2;
				Unity_Multiply_float(_TilingAndOffset_3501B537_Out_3, float2(2, 2), _Multiply_6D84CB8F_Out_2);
				float2 _Subtract_BB974EC8_Out_2;
				Unity_Subtract_float2(_Multiply_6D84CB8F_Out_2, float2(1, 1), _Subtract_BB974EC8_Out_2);
				float _Divide_F34ADB9F_Out_2;
				Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_F34ADB9F_Out_2);
				float _Property_68DC0C48_Out_0 = _Size;
				float _Multiply_F1804ACD_Out_2;
				Unity_Multiply_float(_Divide_F34ADB9F_Out_2, _Property_68DC0C48_Out_0, _Multiply_F1804ACD_Out_2);
				float2 _Vector2_664FE60B_Out_0 = float2(_Multiply_F1804ACD_Out_2, _Property_68DC0C48_Out_0);
				float2 _Divide_6D134715_Out_2;
				Unity_Divide_float2(_Subtract_BB974EC8_Out_2, _Vector2_664FE60B_Out_0, _Divide_6D134715_Out_2);
				float _Length_7DFBCE90_Out_1;
				Unity_Length_float2(_Divide_6D134715_Out_2, _Length_7DFBCE90_Out_1);
				float _OneMinus_C8CC3629_Out_1;
				Unity_OneMinus_float(_Length_7DFBCE90_Out_1, _OneMinus_C8CC3629_Out_1);
				float _Saturate_B48F813D_Out_1;
				Unity_Saturate_float(_OneMinus_C8CC3629_Out_1, _Saturate_B48F813D_Out_1);
				float _Smoothstep_87B5A74E_Out_3;
				Unity_Smoothstep_float(0, _Property_6EA5BC04_Out_0, _Saturate_B48F813D_Out_1, _Smoothstep_87B5A74E_Out_3);
				float _Property_1018870A_Out_0 = Vector1_BC40FEC9;
				float _Multiply_4D84DC2E_Out_2;
				Unity_Multiply_float(_Smoothstep_87B5A74E_Out_3, _Property_1018870A_Out_0, _Multiply_4D84DC2E_Out_2);
				float _OneMinus_DE66BE6C_Out_1;
				Unity_OneMinus_float(_Multiply_4D84DC2E_Out_2, _OneMinus_DE66BE6C_Out_1);
				surface.Albedo = IsGammaSpace() ? float3(0.7353569, 0.7353569, 0.7353569) : SRGBToLinear(float3(0.7353569, 0.7353569, 0.7353569));
				surface.Normal = IN.TangentSpaceNormal;
				surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
				surface.Metallic = 0;
				surface.Smoothness = 0.5;
				surface.Occlusion = 1;
				surface.Alpha = _OneMinus_DE66BE6C_Out_1;
				surface.AlphaClipThreshold = 0;
				return surface;
			}

			// --------------------------------------------------
			// Structs and Packing

			// Generated Type: Attributes
			struct Attributes
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};

			// Generated Type: Varyings
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float3 positionWS;
				float3 normalWS;
				float4 tangentWS;
				float3 viewDirectionWS;
				#if defined(LIGHTMAP_ON)
				float2 lightmapUV;
				#endif
				#if !defined(LIGHTMAP_ON)
				float3 sh;
				#endif
				float4 fogFactorAndVertexLight;
				float4 shadowCoord;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			// Generated Type: PackedVaryings
			struct PackedVaryings
			{
				float4 positionCS : SV_POSITION;
				#if defined(LIGHTMAP_ON)
				#endif
				#if !defined(LIGHTMAP_ON)
				#endif
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float3 interp03 : TEXCOORD3;
				float2 interp04 : TEXCOORD4;
				float3 interp05 : TEXCOORD5;
				float4 interp06 : TEXCOORD6;
				float4 interp07 : TEXCOORD7;
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			// Packed Type: Varyings
			PackedVaryings PackVaryings(Varyings input)
			{
				PackedVaryings output = (PackedVaryings)0;
				output.positionCS = input.positionCS;
				output.interp00.xyz = input.positionWS;
				output.interp01.xyz = input.normalWS;
				output.interp02.xyzw = input.tangentWS;
				output.interp03.xyz = input.viewDirectionWS;
				#if defined(LIGHTMAP_ON)
				output.interp04.xy = input.lightmapUV;
				#endif
				#if !defined(LIGHTMAP_ON)
				output.interp05.xyz = input.sh;
				#endif
				output.interp06.xyzw = input.fogFactorAndVertexLight;
				output.interp07.xyzw = input.shadowCoord;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				output.cullFace = input.cullFace;
				#endif
				return output;
			}

			// Unpacked Type: Varyings
			Varyings UnpackVaryings(PackedVaryings input)
			{
				Varyings output = (Varyings)0;
				output.positionCS = input.positionCS;
				output.positionWS = input.interp00.xyz;
				output.normalWS = input.interp01.xyz;
				output.tangentWS = input.interp02.xyzw;
				output.viewDirectionWS = input.interp03.xyz;
				#if defined(LIGHTMAP_ON)
				output.lightmapUV = input.interp04.xy;
				#endif
				#if !defined(LIGHTMAP_ON)
				output.sh = input.interp05.xyz;
				#endif
				output.fogFactorAndVertexLight = input.interp06.xyzw;
				output.shadowCoord = input.interp07.xyzw;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				output.cullFace = input.cullFace;
				#endif
				return output;
			}

			// --------------------------------------------------
			// Build Graph Inputs

			SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
			{
				SurfaceDescriptionInputs output;
				ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



				output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


				output.WorldSpacePosition = input.positionWS;
				output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
			#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
			#else
			#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
			#endif
			#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

				return output;
			}


			// --------------------------------------------------
			// Main

			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "ShadowCaster"
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

				// Render State
				Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
				Cull Back
				ZTest LEqual
				ZWrite On
				// ColorMask: <None>


				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				// Debug
				// <None>

				// --------------------------------------------------
				// Pass

				// Pragmas
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
				#pragma multi_compile_instancing

				// Keywords
				// PassKeywords: <None>
				// GraphKeywords: <None>

				// Defines
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _NORMAL_DROPOFF_TS 1
				#define ATTRIBUTES_NEED_NORMAL
				#define ATTRIBUTES_NEED_TANGENT
				#define VARYINGS_NEED_POSITION_WS 
				#define SHADERPASS_SHADOWCASTER

				// Includes
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
				#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

				// --------------------------------------------------
				// Graph

				// Graph Properties
				CBUFFER_START(UnityPerMaterial)
				float2 _Position;
				float _Size;
				float Vector1_6234116B;
				float Vector1_BC40FEC9;
				CBUFFER_END

					// Graph Functions

					void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
					{
						Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
					}

					void Unity_Add_float2(float2 A, float2 B, out float2 Out)
					{
						Out = A + B;
					}

					void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
					{
						Out = UV * Tiling + Offset;
					}

					void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
					{
						Out = A * B;
					}

					void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
					{
						Out = A - B;
					}

					void Unity_Divide_float(float A, float B, out float Out)
					{
						Out = A / B;
					}

					void Unity_Multiply_float(float A, float B, out float Out)
					{
						Out = A * B;
					}

					void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
					{
						Out = A / B;
					}

					void Unity_Length_float2(float2 In, out float Out)
					{
						Out = length(In);
					}

					void Unity_OneMinus_float(float In, out float Out)
					{
						Out = 1 - In;
					}

					void Unity_Saturate_float(float In, out float Out)
					{
						Out = saturate(In);
					}

					void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
					{
						Out = smoothstep(Edge1, Edge2, In);
					}

					// Graph Vertex
					// GraphVertex: <None>

					// Graph Pixel
					struct SurfaceDescriptionInputs
					{
						float3 TangentSpaceNormal;
						float3 WorldSpacePosition;
						float4 ScreenPosition;
					};

					struct SurfaceDescription
					{
						float Alpha;
						float AlphaClipThreshold;
					};

					SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
					{
						SurfaceDescription surface = (SurfaceDescription)0;
						float _Property_6EA5BC04_Out_0 = Vector1_6234116B;
						float4 _ScreenPosition_FA01CDAC_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
						float2 _Property_9BCB7C86_Out_0 = _Position;
						float2 _Remap_F4A1407A_Out_3;
						Unity_Remap_float2(_Property_9BCB7C86_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_F4A1407A_Out_3);
						float2 _Add_C0A42221_Out_2;
						Unity_Add_float2((_ScreenPosition_FA01CDAC_Out_0.xy), _Remap_F4A1407A_Out_3, _Add_C0A42221_Out_2);
						float2 _TilingAndOffset_3501B537_Out_3;
						Unity_TilingAndOffset_float((_ScreenPosition_FA01CDAC_Out_0.xy), float2 (1, 1), _Add_C0A42221_Out_2, _TilingAndOffset_3501B537_Out_3);
						float2 _Multiply_6D84CB8F_Out_2;
						Unity_Multiply_float(_TilingAndOffset_3501B537_Out_3, float2(2, 2), _Multiply_6D84CB8F_Out_2);
						float2 _Subtract_BB974EC8_Out_2;
						Unity_Subtract_float2(_Multiply_6D84CB8F_Out_2, float2(1, 1), _Subtract_BB974EC8_Out_2);
						float _Divide_F34ADB9F_Out_2;
						Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_F34ADB9F_Out_2);
						float _Property_68DC0C48_Out_0 = _Size;
						float _Multiply_F1804ACD_Out_2;
						Unity_Multiply_float(_Divide_F34ADB9F_Out_2, _Property_68DC0C48_Out_0, _Multiply_F1804ACD_Out_2);
						float2 _Vector2_664FE60B_Out_0 = float2(_Multiply_F1804ACD_Out_2, _Property_68DC0C48_Out_0);
						float2 _Divide_6D134715_Out_2;
						Unity_Divide_float2(_Subtract_BB974EC8_Out_2, _Vector2_664FE60B_Out_0, _Divide_6D134715_Out_2);
						float _Length_7DFBCE90_Out_1;
						Unity_Length_float2(_Divide_6D134715_Out_2, _Length_7DFBCE90_Out_1);
						float _OneMinus_C8CC3629_Out_1;
						Unity_OneMinus_float(_Length_7DFBCE90_Out_1, _OneMinus_C8CC3629_Out_1);
						float _Saturate_B48F813D_Out_1;
						Unity_Saturate_float(_OneMinus_C8CC3629_Out_1, _Saturate_B48F813D_Out_1);
						float _Smoothstep_87B5A74E_Out_3;
						Unity_Smoothstep_float(0, _Property_6EA5BC04_Out_0, _Saturate_B48F813D_Out_1, _Smoothstep_87B5A74E_Out_3);
						float _Property_1018870A_Out_0 = Vector1_BC40FEC9;
						float _Multiply_4D84DC2E_Out_2;
						Unity_Multiply_float(_Smoothstep_87B5A74E_Out_3, _Property_1018870A_Out_0, _Multiply_4D84DC2E_Out_2);
						float _OneMinus_DE66BE6C_Out_1;
						Unity_OneMinus_float(_Multiply_4D84DC2E_Out_2, _OneMinus_DE66BE6C_Out_1);
						surface.Alpha = _OneMinus_DE66BE6C_Out_1;
						surface.AlphaClipThreshold = 0;
						return surface;
					}

					// --------------------------------------------------
					// Structs and Packing

					// Generated Type: Attributes
					struct Attributes
					{
						float3 positionOS : POSITION;
						float3 normalOS : NORMAL;
						float4 tangentOS : TANGENT;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : INSTANCEID_SEMANTIC;
						#endif
					};

					// Generated Type: Varyings
					struct Varyings
					{
						float4 positionCS : SV_POSITION;
						float3 positionWS;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID;
						#endif
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
						#endif
					};

					// Generated Type: PackedVaryings
					struct PackedVaryings
					{
						float4 positionCS : SV_POSITION;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID;
						#endif
						float3 interp00 : TEXCOORD0;
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
						#endif
					};

					// Packed Type: Varyings
					PackedVaryings PackVaryings(Varyings input)
					{
						PackedVaryings output = (PackedVaryings)0;
						output.positionCS = input.positionCS;
						output.interp00.xyz = input.positionWS;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						output.cullFace = input.cullFace;
						#endif
						return output;
					}

					// Unpacked Type: Varyings
					Varyings UnpackVaryings(PackedVaryings input)
					{
						Varyings output = (Varyings)0;
						output.positionCS = input.positionCS;
						output.positionWS = input.interp00.xyz;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						output.cullFace = input.cullFace;
						#endif
						return output;
					}

					// --------------------------------------------------
					// Build Graph Inputs

					SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
					{
						SurfaceDescriptionInputs output;
						ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



						output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


						output.WorldSpacePosition = input.positionWS;
						output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
					#else
					#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
					#endif
					#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

						return output;
					}


					// --------------------------------------------------
					// Main

					#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
					#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

					ENDHLSL
				}

				Pass
				{
					Name "DepthOnly"
					Tags
					{
						"LightMode" = "DepthOnly"
					}

						// Render State
						Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
						Cull Back
						ZTest LEqual
						ZWrite On
						ColorMask 0


						HLSLPROGRAM
						#pragma vertex vert
						#pragma fragment frag

						// Debug
						// <None>

						// --------------------------------------------------
						// Pass

						// Pragmas
						#pragma prefer_hlslcc gles
						#pragma exclude_renderers d3d11_9x
						#pragma target 2.0
						#pragma multi_compile_instancing

						// Keywords
						// PassKeywords: <None>
						// GraphKeywords: <None>

						// Defines
						#define _SURFACE_TYPE_TRANSPARENT 1
						#define _NORMAL_DROPOFF_TS 1
						#define ATTRIBUTES_NEED_NORMAL
						#define ATTRIBUTES_NEED_TANGENT
						#define VARYINGS_NEED_POSITION_WS 
						#define SHADERPASS_DEPTHONLY

						// Includes
						#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
						#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
						#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
						#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
						#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

						// --------------------------------------------------
						// Graph

						// Graph Properties
						CBUFFER_START(UnityPerMaterial)
						float2 _Position;
						float _Size;
						float Vector1_6234116B;
						float Vector1_BC40FEC9;
						CBUFFER_END

							// Graph Functions

							void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
							{
								Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
							}

							void Unity_Add_float2(float2 A, float2 B, out float2 Out)
							{
								Out = A + B;
							}

							void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
							{
								Out = UV * Tiling + Offset;
							}

							void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
							{
								Out = A * B;
							}

							void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
							{
								Out = A - B;
							}

							void Unity_Divide_float(float A, float B, out float Out)
							{
								Out = A / B;
							}

							void Unity_Multiply_float(float A, float B, out float Out)
							{
								Out = A * B;
							}

							void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
							{
								Out = A / B;
							}

							void Unity_Length_float2(float2 In, out float Out)
							{
								Out = length(In);
							}

							void Unity_OneMinus_float(float In, out float Out)
							{
								Out = 1 - In;
							}

							void Unity_Saturate_float(float In, out float Out)
							{
								Out = saturate(In);
							}

							void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
							{
								Out = smoothstep(Edge1, Edge2, In);
							}

							// Graph Vertex
							// GraphVertex: <None>

							// Graph Pixel
							struct SurfaceDescriptionInputs
							{
								float3 TangentSpaceNormal;
								float3 WorldSpacePosition;
								float4 ScreenPosition;
							};

							struct SurfaceDescription
							{
								float Alpha;
								float AlphaClipThreshold;
							};

							SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
							{
								SurfaceDescription surface = (SurfaceDescription)0;
								float _Property_6EA5BC04_Out_0 = Vector1_6234116B;
								float4 _ScreenPosition_FA01CDAC_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
								float2 _Property_9BCB7C86_Out_0 = _Position;
								float2 _Remap_F4A1407A_Out_3;
								Unity_Remap_float2(_Property_9BCB7C86_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_F4A1407A_Out_3);
								float2 _Add_C0A42221_Out_2;
								Unity_Add_float2((_ScreenPosition_FA01CDAC_Out_0.xy), _Remap_F4A1407A_Out_3, _Add_C0A42221_Out_2);
								float2 _TilingAndOffset_3501B537_Out_3;
								Unity_TilingAndOffset_float((_ScreenPosition_FA01CDAC_Out_0.xy), float2 (1, 1), _Add_C0A42221_Out_2, _TilingAndOffset_3501B537_Out_3);
								float2 _Multiply_6D84CB8F_Out_2;
								Unity_Multiply_float(_TilingAndOffset_3501B537_Out_3, float2(2, 2), _Multiply_6D84CB8F_Out_2);
								float2 _Subtract_BB974EC8_Out_2;
								Unity_Subtract_float2(_Multiply_6D84CB8F_Out_2, float2(1, 1), _Subtract_BB974EC8_Out_2);
								float _Divide_F34ADB9F_Out_2;
								Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_F34ADB9F_Out_2);
								float _Property_68DC0C48_Out_0 = _Size;
								float _Multiply_F1804ACD_Out_2;
								Unity_Multiply_float(_Divide_F34ADB9F_Out_2, _Property_68DC0C48_Out_0, _Multiply_F1804ACD_Out_2);
								float2 _Vector2_664FE60B_Out_0 = float2(_Multiply_F1804ACD_Out_2, _Property_68DC0C48_Out_0);
								float2 _Divide_6D134715_Out_2;
								Unity_Divide_float2(_Subtract_BB974EC8_Out_2, _Vector2_664FE60B_Out_0, _Divide_6D134715_Out_2);
								float _Length_7DFBCE90_Out_1;
								Unity_Length_float2(_Divide_6D134715_Out_2, _Length_7DFBCE90_Out_1);
								float _OneMinus_C8CC3629_Out_1;
								Unity_OneMinus_float(_Length_7DFBCE90_Out_1, _OneMinus_C8CC3629_Out_1);
								float _Saturate_B48F813D_Out_1;
								Unity_Saturate_float(_OneMinus_C8CC3629_Out_1, _Saturate_B48F813D_Out_1);
								float _Smoothstep_87B5A74E_Out_3;
								Unity_Smoothstep_float(0, _Property_6EA5BC04_Out_0, _Saturate_B48F813D_Out_1, _Smoothstep_87B5A74E_Out_3);
								float _Property_1018870A_Out_0 = Vector1_BC40FEC9;
								float _Multiply_4D84DC2E_Out_2;
								Unity_Multiply_float(_Smoothstep_87B5A74E_Out_3, _Property_1018870A_Out_0, _Multiply_4D84DC2E_Out_2);
								float _OneMinus_DE66BE6C_Out_1;
								Unity_OneMinus_float(_Multiply_4D84DC2E_Out_2, _OneMinus_DE66BE6C_Out_1);
								surface.Alpha = _OneMinus_DE66BE6C_Out_1;
								surface.AlphaClipThreshold = 0;
								return surface;
							}

							// --------------------------------------------------
							// Structs and Packing

							// Generated Type: Attributes
							struct Attributes
							{
								float3 positionOS : POSITION;
								float3 normalOS : NORMAL;
								float4 tangentOS : TANGENT;
								#if UNITY_ANY_INSTANCING_ENABLED
								uint instanceID : INSTANCEID_SEMANTIC;
								#endif
							};

							// Generated Type: Varyings
							struct Varyings
							{
								float4 positionCS : SV_POSITION;
								float3 positionWS;
								#if UNITY_ANY_INSTANCING_ENABLED
								uint instanceID : CUSTOM_INSTANCE_ID;
								#endif
								#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
								uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
								#endif
								#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
								uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
								#endif
								#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
								FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
								#endif
							};

							// Generated Type: PackedVaryings
							struct PackedVaryings
							{
								float4 positionCS : SV_POSITION;
								#if UNITY_ANY_INSTANCING_ENABLED
								uint instanceID : CUSTOM_INSTANCE_ID;
								#endif
								float3 interp00 : TEXCOORD0;
								#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
								uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
								#endif
								#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
								uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
								#endif
								#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
								FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
								#endif
							};

							// Packed Type: Varyings
							PackedVaryings PackVaryings(Varyings input)
							{
								PackedVaryings output = (PackedVaryings)0;
								output.positionCS = input.positionCS;
								output.interp00.xyz = input.positionWS;
								#if UNITY_ANY_INSTANCING_ENABLED
								output.instanceID = input.instanceID;
								#endif
								#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
								output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
								#endif
								#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
								output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
								#endif
								#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
								output.cullFace = input.cullFace;
								#endif
								return output;
							}

							// Unpacked Type: Varyings
							Varyings UnpackVaryings(PackedVaryings input)
							{
								Varyings output = (Varyings)0;
								output.positionCS = input.positionCS;
								output.positionWS = input.interp00.xyz;
								#if UNITY_ANY_INSTANCING_ENABLED
								output.instanceID = input.instanceID;
								#endif
								#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
								output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
								#endif
								#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
								output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
								#endif
								#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
								output.cullFace = input.cullFace;
								#endif
								return output;
							}

							// --------------------------------------------------
							// Build Graph Inputs

							SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
							{
								SurfaceDescriptionInputs output;
								ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



								output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


								output.WorldSpacePosition = input.positionWS;
								output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
							#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
							#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
							#else
							#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
							#endif
							#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

								return output;
							}


							// --------------------------------------------------
							// Main

							#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
							#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

							ENDHLSL
						}

						Pass
						{
							Name "Meta"
							Tags
							{
								"LightMode" = "Meta"
							}

								// Render State
								Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
								Cull Back
								ZTest LEqual
								ZWrite On
								// ColorMask: <None>


								HLSLPROGRAM
								#pragma vertex vert
								#pragma fragment frag

								// Debug
								// <None>

								// --------------------------------------------------
								// Pass

								// Pragmas
								#pragma prefer_hlslcc gles
								#pragma exclude_renderers d3d11_9x
								#pragma target 2.0

								// Keywords
								#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
								// GraphKeywords: <None>

								// Defines
								#define _SURFACE_TYPE_TRANSPARENT 1
								#define _NORMAL_DROPOFF_TS 1
								#define ATTRIBUTES_NEED_NORMAL
								#define ATTRIBUTES_NEED_TANGENT
								#define ATTRIBUTES_NEED_TEXCOORD1
								#define ATTRIBUTES_NEED_TEXCOORD2
								#define VARYINGS_NEED_POSITION_WS 
								#define SHADERPASS_META

								// Includes
								#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
								#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
								#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
								#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
								#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
								#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

								// --------------------------------------------------
								// Graph

								// Graph Properties
								CBUFFER_START(UnityPerMaterial)
								float2 _Position;
								float _Size;
								float Vector1_6234116B;
								float Vector1_BC40FEC9;
								CBUFFER_END

									// Graph Functions

									void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
									{
										Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
									}

									void Unity_Add_float2(float2 A, float2 B, out float2 Out)
									{
										Out = A + B;
									}

									void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
									{
										Out = UV * Tiling + Offset;
									}

									void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
									{
										Out = A * B;
									}

									void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
									{
										Out = A - B;
									}

									void Unity_Divide_float(float A, float B, out float Out)
									{
										Out = A / B;
									}

									void Unity_Multiply_float(float A, float B, out float Out)
									{
										Out = A * B;
									}

									void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
									{
										Out = A / B;
									}

									void Unity_Length_float2(float2 In, out float Out)
									{
										Out = length(In);
									}

									void Unity_OneMinus_float(float In, out float Out)
									{
										Out = 1 - In;
									}

									void Unity_Saturate_float(float In, out float Out)
									{
										Out = saturate(In);
									}

									void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
									{
										Out = smoothstep(Edge1, Edge2, In);
									}

									// Graph Vertex
									// GraphVertex: <None>

									// Graph Pixel
									struct SurfaceDescriptionInputs
									{
										float3 TangentSpaceNormal;
										float3 WorldSpacePosition;
										float4 ScreenPosition;
									};

									struct SurfaceDescription
									{
										float3 Albedo;
										float3 Emission;
										float Alpha;
										float AlphaClipThreshold;
									};

									SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
									{
										SurfaceDescription surface = (SurfaceDescription)0;
										float _Property_6EA5BC04_Out_0 = Vector1_6234116B;
										float4 _ScreenPosition_FA01CDAC_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
										float2 _Property_9BCB7C86_Out_0 = _Position;
										float2 _Remap_F4A1407A_Out_3;
										Unity_Remap_float2(_Property_9BCB7C86_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_F4A1407A_Out_3);
										float2 _Add_C0A42221_Out_2;
										Unity_Add_float2((_ScreenPosition_FA01CDAC_Out_0.xy), _Remap_F4A1407A_Out_3, _Add_C0A42221_Out_2);
										float2 _TilingAndOffset_3501B537_Out_3;
										Unity_TilingAndOffset_float((_ScreenPosition_FA01CDAC_Out_0.xy), float2 (1, 1), _Add_C0A42221_Out_2, _TilingAndOffset_3501B537_Out_3);
										float2 _Multiply_6D84CB8F_Out_2;
										Unity_Multiply_float(_TilingAndOffset_3501B537_Out_3, float2(2, 2), _Multiply_6D84CB8F_Out_2);
										float2 _Subtract_BB974EC8_Out_2;
										Unity_Subtract_float2(_Multiply_6D84CB8F_Out_2, float2(1, 1), _Subtract_BB974EC8_Out_2);
										float _Divide_F34ADB9F_Out_2;
										Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_F34ADB9F_Out_2);
										float _Property_68DC0C48_Out_0 = _Size;
										float _Multiply_F1804ACD_Out_2;
										Unity_Multiply_float(_Divide_F34ADB9F_Out_2, _Property_68DC0C48_Out_0, _Multiply_F1804ACD_Out_2);
										float2 _Vector2_664FE60B_Out_0 = float2(_Multiply_F1804ACD_Out_2, _Property_68DC0C48_Out_0);
										float2 _Divide_6D134715_Out_2;
										Unity_Divide_float2(_Subtract_BB974EC8_Out_2, _Vector2_664FE60B_Out_0, _Divide_6D134715_Out_2);
										float _Length_7DFBCE90_Out_1;
										Unity_Length_float2(_Divide_6D134715_Out_2, _Length_7DFBCE90_Out_1);
										float _OneMinus_C8CC3629_Out_1;
										Unity_OneMinus_float(_Length_7DFBCE90_Out_1, _OneMinus_C8CC3629_Out_1);
										float _Saturate_B48F813D_Out_1;
										Unity_Saturate_float(_OneMinus_C8CC3629_Out_1, _Saturate_B48F813D_Out_1);
										float _Smoothstep_87B5A74E_Out_3;
										Unity_Smoothstep_float(0, _Property_6EA5BC04_Out_0, _Saturate_B48F813D_Out_1, _Smoothstep_87B5A74E_Out_3);
										float _Property_1018870A_Out_0 = Vector1_BC40FEC9;
										float _Multiply_4D84DC2E_Out_2;
										Unity_Multiply_float(_Smoothstep_87B5A74E_Out_3, _Property_1018870A_Out_0, _Multiply_4D84DC2E_Out_2);
										float _OneMinus_DE66BE6C_Out_1;
										Unity_OneMinus_float(_Multiply_4D84DC2E_Out_2, _OneMinus_DE66BE6C_Out_1);
										surface.Albedo = IsGammaSpace() ? float3(0.7353569, 0.7353569, 0.7353569) : SRGBToLinear(float3(0.7353569, 0.7353569, 0.7353569));
										surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
										surface.Alpha = _OneMinus_DE66BE6C_Out_1;
										surface.AlphaClipThreshold = 0;
										return surface;
									}

									// --------------------------------------------------
									// Structs and Packing

									// Generated Type: Attributes
									struct Attributes
									{
										float3 positionOS : POSITION;
										float3 normalOS : NORMAL;
										float4 tangentOS : TANGENT;
										float4 uv1 : TEXCOORD1;
										float4 uv2 : TEXCOORD2;
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : INSTANCEID_SEMANTIC;
										#endif
									};

									// Generated Type: Varyings
									struct Varyings
									{
										float4 positionCS : SV_POSITION;
										float3 positionWS;
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : CUSTOM_INSTANCE_ID;
										#endif
										#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
										uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
										#endif
										#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
										uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
										#endif
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
										#endif
									};

									// Generated Type: PackedVaryings
									struct PackedVaryings
									{
										float4 positionCS : SV_POSITION;
										#if UNITY_ANY_INSTANCING_ENABLED
										uint instanceID : CUSTOM_INSTANCE_ID;
										#endif
										float3 interp00 : TEXCOORD0;
										#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
										uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
										#endif
										#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
										uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
										#endif
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
										#endif
									};

									// Packed Type: Varyings
									PackedVaryings PackVaryings(Varyings input)
									{
										PackedVaryings output = (PackedVaryings)0;
										output.positionCS = input.positionCS;
										output.interp00.xyz = input.positionWS;
										#if UNITY_ANY_INSTANCING_ENABLED
										output.instanceID = input.instanceID;
										#endif
										#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
										output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
										#endif
										#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
										output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
										#endif
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										output.cullFace = input.cullFace;
										#endif
										return output;
									}

									// Unpacked Type: Varyings
									Varyings UnpackVaryings(PackedVaryings input)
									{
										Varyings output = (Varyings)0;
										output.positionCS = input.positionCS;
										output.positionWS = input.interp00.xyz;
										#if UNITY_ANY_INSTANCING_ENABLED
										output.instanceID = input.instanceID;
										#endif
										#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
										output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
										#endif
										#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
										output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
										#endif
										#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
										output.cullFace = input.cullFace;
										#endif
										return output;
									}

									// --------------------------------------------------
									// Build Graph Inputs

									SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
									{
										SurfaceDescriptionInputs output;
										ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



										output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


										output.WorldSpacePosition = input.positionWS;
										output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
									#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
									#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
									#else
									#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
									#endif
									#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

										return output;
									}


									// --------------------------------------------------
									// Main

									#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
									#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

									ENDHLSL
								}

								Pass
								{
										// Name: <None>
										Tags
										{
											"LightMode" = "Universal2D"
										}

										// Render State
										Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
										Cull Back
										ZTest LEqual
										ZWrite Off
										// ColorMask: <None>


										HLSLPROGRAM
										#pragma vertex vert
										#pragma fragment frag

										// Debug
										// <None>

										// --------------------------------------------------
										// Pass

										// Pragmas
										#pragma prefer_hlslcc gles
										#pragma exclude_renderers d3d11_9x
										#pragma target 2.0
										#pragma multi_compile_instancing

										// Keywords
										// PassKeywords: <None>
										// GraphKeywords: <None>

										// Defines
										#define _SURFACE_TYPE_TRANSPARENT 1
										#define _NORMAL_DROPOFF_TS 1
										#define ATTRIBUTES_NEED_NORMAL
										#define ATTRIBUTES_NEED_TANGENT
										#define VARYINGS_NEED_POSITION_WS 
										#define SHADERPASS_2D

										// Includes
										#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
										#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
										#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
										#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
										#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

										// --------------------------------------------------
										// Graph

										// Graph Properties
										CBUFFER_START(UnityPerMaterial)
										float2 _Position;
										float _Size;
										float Vector1_6234116B;
										float Vector1_BC40FEC9;
										CBUFFER_END

											// Graph Functions

											void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
											{
												Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
											}

											void Unity_Add_float2(float2 A, float2 B, out float2 Out)
											{
												Out = A + B;
											}

											void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
											{
												Out = UV * Tiling + Offset;
											}

											void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
											{
												Out = A * B;
											}

											void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
											{
												Out = A - B;
											}

											void Unity_Divide_float(float A, float B, out float Out)
											{
												Out = A / B;
											}

											void Unity_Multiply_float(float A, float B, out float Out)
											{
												Out = A * B;
											}

											void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
											{
												Out = A / B;
											}

											void Unity_Length_float2(float2 In, out float Out)
											{
												Out = length(In);
											}

											void Unity_OneMinus_float(float In, out float Out)
											{
												Out = 1 - In;
											}

											void Unity_Saturate_float(float In, out float Out)
											{
												Out = saturate(In);
											}

											void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
											{
												Out = smoothstep(Edge1, Edge2, In);
											}

											// Graph Vertex
											// GraphVertex: <None>

											// Graph Pixel
											struct SurfaceDescriptionInputs
											{
												float3 TangentSpaceNormal;
												float3 WorldSpacePosition;
												float4 ScreenPosition;
											};

											struct SurfaceDescription
											{
												float3 Albedo;
												float Alpha;
												float AlphaClipThreshold;
											};

											SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
											{
												SurfaceDescription surface = (SurfaceDescription)0;
												float _Property_6EA5BC04_Out_0 = Vector1_6234116B;
												float4 _ScreenPosition_FA01CDAC_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
												float2 _Property_9BCB7C86_Out_0 = _Position;
												float2 _Remap_F4A1407A_Out_3;
												Unity_Remap_float2(_Property_9BCB7C86_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_F4A1407A_Out_3);
												float2 _Add_C0A42221_Out_2;
												Unity_Add_float2((_ScreenPosition_FA01CDAC_Out_0.xy), _Remap_F4A1407A_Out_3, _Add_C0A42221_Out_2);
												float2 _TilingAndOffset_3501B537_Out_3;
												Unity_TilingAndOffset_float((_ScreenPosition_FA01CDAC_Out_0.xy), float2 (1, 1), _Add_C0A42221_Out_2, _TilingAndOffset_3501B537_Out_3);
												float2 _Multiply_6D84CB8F_Out_2;
												Unity_Multiply_float(_TilingAndOffset_3501B537_Out_3, float2(2, 2), _Multiply_6D84CB8F_Out_2);
												float2 _Subtract_BB974EC8_Out_2;
												Unity_Subtract_float2(_Multiply_6D84CB8F_Out_2, float2(1, 1), _Subtract_BB974EC8_Out_2);
												float _Divide_F34ADB9F_Out_2;
												Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_F34ADB9F_Out_2);
												float _Property_68DC0C48_Out_0 = _Size;
												float _Multiply_F1804ACD_Out_2;
												Unity_Multiply_float(_Divide_F34ADB9F_Out_2, _Property_68DC0C48_Out_0, _Multiply_F1804ACD_Out_2);
												float2 _Vector2_664FE60B_Out_0 = float2(_Multiply_F1804ACD_Out_2, _Property_68DC0C48_Out_0);
												float2 _Divide_6D134715_Out_2;
												Unity_Divide_float2(_Subtract_BB974EC8_Out_2, _Vector2_664FE60B_Out_0, _Divide_6D134715_Out_2);
												float _Length_7DFBCE90_Out_1;
												Unity_Length_float2(_Divide_6D134715_Out_2, _Length_7DFBCE90_Out_1);
												float _OneMinus_C8CC3629_Out_1;
												Unity_OneMinus_float(_Length_7DFBCE90_Out_1, _OneMinus_C8CC3629_Out_1);
												float _Saturate_B48F813D_Out_1;
												Unity_Saturate_float(_OneMinus_C8CC3629_Out_1, _Saturate_B48F813D_Out_1);
												float _Smoothstep_87B5A74E_Out_3;
												Unity_Smoothstep_float(0, _Property_6EA5BC04_Out_0, _Saturate_B48F813D_Out_1, _Smoothstep_87B5A74E_Out_3);
												float _Property_1018870A_Out_0 = Vector1_BC40FEC9;
												float _Multiply_4D84DC2E_Out_2;
												Unity_Multiply_float(_Smoothstep_87B5A74E_Out_3, _Property_1018870A_Out_0, _Multiply_4D84DC2E_Out_2);
												float _OneMinus_DE66BE6C_Out_1;
												Unity_OneMinus_float(_Multiply_4D84DC2E_Out_2, _OneMinus_DE66BE6C_Out_1);
												surface.Albedo = IsGammaSpace() ? float3(0.7353569, 0.7353569, 0.7353569) : SRGBToLinear(float3(0.7353569, 0.7353569, 0.7353569));
												surface.Alpha = _OneMinus_DE66BE6C_Out_1;
												surface.AlphaClipThreshold = 0;
												return surface;
											}

											// --------------------------------------------------
											// Structs and Packing

											// Generated Type: Attributes
											struct Attributes
											{
												float3 positionOS : POSITION;
												float3 normalOS : NORMAL;
												float4 tangentOS : TANGENT;
												#if UNITY_ANY_INSTANCING_ENABLED
												uint instanceID : INSTANCEID_SEMANTIC;
												#endif
											};

											// Generated Type: Varyings
											struct Varyings
											{
												float4 positionCS : SV_POSITION;
												float3 positionWS;
												#if UNITY_ANY_INSTANCING_ENABLED
												uint instanceID : CUSTOM_INSTANCE_ID;
												#endif
												#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
												uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
												#endif
												#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
												uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
												#endif
												#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
												FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
												#endif
											};

											// Generated Type: PackedVaryings
											struct PackedVaryings
											{
												float4 positionCS : SV_POSITION;
												#if UNITY_ANY_INSTANCING_ENABLED
												uint instanceID : CUSTOM_INSTANCE_ID;
												#endif
												float3 interp00 : TEXCOORD0;
												#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
												uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
												#endif
												#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
												uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
												#endif
												#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
												FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
												#endif
											};

											// Packed Type: Varyings
											PackedVaryings PackVaryings(Varyings input)
											{
												PackedVaryings output = (PackedVaryings)0;
												output.positionCS = input.positionCS;
												output.interp00.xyz = input.positionWS;
												#if UNITY_ANY_INSTANCING_ENABLED
												output.instanceID = input.instanceID;
												#endif
												#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
												output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
												#endif
												#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
												output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
												#endif
												#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
												output.cullFace = input.cullFace;
												#endif
												return output;
											}

											// Unpacked Type: Varyings
											Varyings UnpackVaryings(PackedVaryings input)
											{
												Varyings output = (Varyings)0;
												output.positionCS = input.positionCS;
												output.positionWS = input.interp00.xyz;
												#if UNITY_ANY_INSTANCING_ENABLED
												output.instanceID = input.instanceID;
												#endif
												#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
												output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
												#endif
												#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
												output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
												#endif
												#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
												output.cullFace = input.cullFace;
												#endif
												return output;
											}

											// --------------------------------------------------
											// Build Graph Inputs

											SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
											{
												SurfaceDescriptionInputs output;
												ZERO_INITIALIZE(SurfaceDescriptionInputs, output);



												output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


												output.WorldSpacePosition = input.positionWS;
												output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
											#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
											#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
											#else
											#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
											#endif
											#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

												return output;
											}


											// --------------------------------------------------
											// Main

											#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
											#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

											ENDHLSL
										}

	}
		CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
												FallBack "Hidden/Shader Graph/FallbackError"
}
