Shader "GAPH Custom Shader/Object Skin/Object Skin(With Animation)" {
	Properties{
		[HDR]_Color("Color",Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_ObjectTex("Object Texture",2D) = "white"{}
		_TexNormal("Texture Normal",2D) = "black"{}
		_Speed("Offset Speed",Float) = 1
		_CutOut("CutOut",Range(0,1)) = 1

		[Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("BlendSrc", float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("BlendDst", float) = 1

	}
		Category
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Tranparent"  "IgnoreProjector" = "True" }
		Blend[_BlendSrc][_BlendDst]
		ColorMask RGB
		LOD 200

		SubShader{
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_particles
#pragma target 3.0

#include "UnityCG.cginc"

		sampler2D _MainTex;
	sampler2D _TexNormal;
	sampler2D _ObjectTex;


	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord2 :TEXCOORD1;
		float2 texcoord3 : TEXCOORD2;
		float3 normal : NORMAL;
		float3 viewDir : TEXCOORD3;
	};

	struct v2f {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord2 : TEXCOORD1;
		float2 texcoord3 : TEXCOORD2;
		float3 normal : NORMAL;
		float3 viewDir : TEXCOORD3;
	};

	half4 _MainTex_ST;
	half4 _TexNormal_ST;
	half4 _ObjectTex_ST;
	half4 _Color;
	half _Speed;
	half _CutOut;

	v2f vert(appdata_t v)
	{
		v2f o;

		o.vertex = UnityObjectToClipPos(v.vertex);

		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.texcoord2 = TRANSFORM_TEX(v.texcoord, _TexNormal);
		o.texcoord3 = TRANSFORM_TEX(v.texcoord, _ObjectTex);
		o.viewDir = WorldSpaceViewDir(v.vertex);
		o.normal = UnityObjectToWorldNormal(v.vertex);

		return o;
	}

	half4 frag(v2f i) : SV_Target
	{
		half2 distort = UnpackNormal(tex2D(_TexNormal, i.texcoord2)).rg;
		half objtex = tex2D(_ObjectTex, i.texcoord3);

		half4 tex = tex2D(_MainTex, i.texcoord.xy + distort.x / 6 - (_Speed * _Time.xx*1.25)) * _Color;
		tex *= tex2D(_MainTex, i.texcoord.xy + distort.y / 4 + (_Speed * _Time.xx*0.75) + float2(1,0.5f));
		tex = pow(tex,0.8);

		half4 res = float4(0,0,0,0);
		res = objtex;
		res.rbg += tex.rgb;
		res.a += tex.a;

		return res;
	}
		ENDCG
	}
	}
	}
}