// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GAPH Custom Shader/Distortion Effect/Distortion Texture/Distortion Effect(WithMask)" {
	Properties {
	}

	Category {

		SubShader{
				Pass{
				Name "BASE"
					
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma fragmentoption ARB_precision_hint_fastest
					#pragma multi_compile_particles
					#include "UnityCG.cginc"

					struct appdata_t {

					};

					struct v2f {

					};

					fixed4 _TintColor;

					v2f vert (appdata_t v)
					{
					
					}

					half4 frag( v2f i ) : COLOR
					{
						return 0;
					}
				ENDCG
			}
		}
	}
}