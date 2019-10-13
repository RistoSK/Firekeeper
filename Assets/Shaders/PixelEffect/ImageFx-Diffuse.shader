Shader "Dithering Shaders/ImageFx/Diffuse" {
	Properties {
		_MainTex ("Render Input", 2D) = "white" {}
		_ColorCount ("Mixed Color Count", float) = 4
		_PaletteHeight ("Palette Height", float) = 128
		_PaletteTex ("Palette", 2D) = "black" {}
		_DitherSize ("Dither Size (Width/Height)", float) = 8
		_DitherTex ("Dither", 2D) = "black" {}
	}

	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert finalcolor:dither
		#include "CGIncludes/Dithering.cginc"

		sampler2D _MainTex;
		sampler2D _PaletteTex;
		sampler2D _DitherTex;
		float _ColorCount;
		float _PaletteHeight;
		float _DitherSize;

		struct Input {
			float2 uv_MainTex;
			float4 ditherPos;
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.ditherPos = GetDitherPos(v.vertex, _DitherSize);
		}

		void surf(Input i, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, i.uv_MainTex);
			o.Albedo = c.rgb;
		}

		void dither(Input i, SurfaceOutput o, inout fixed4 color) {
			color.rgb = GetDitherColor(color.rgb, _DitherTex, _PaletteTex,
				_PaletteHeight, i.ditherPos, _ColorCount);
		}
		ENDCG
	}

	Fallback "Diffuse"
}