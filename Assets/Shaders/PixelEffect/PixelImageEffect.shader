Shader "Custom/PixelImageEffect"
{
    Properties
    {
		_Threshold("Threshold", Float) = 0.45
		_Strength("Strength", Float) = 0.45
		_Width("Width", Int) = 0.45
		_Height("Height", Int) = 0.45
		_Dither("Dither", 2D) = "white" {}
        _MainTex ("Texture", 2D) = "white" {}
		_Red("Red", Int) = 8
		_Green("Green", Int) = 8
		_Blue("Blue", Int) = 8
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            //#pragma vertex vert
           // #pragma fragment frag
			#pragma exclude_renderers flash
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            //v2f vert (appdata v)
            //{
            //    v2f o;
            //    o.vertex = UnityObjectToClipPos(v.vertex);
            //    o.uv = v.uv;
            //    return o;
            //}






			uniform float _Threshold;
			uniform float _Strength;
			uniform int _Width;
			uniform int _Height;

			uniform sampler2D _Dither;

			float luma(fixed3 color) {
				return dot(color, fixed3(0.299, 0.587, 0.114));
			}

			float luma(fixed4 color) {
				return dot(color.rgb, fixed3(0.299, 0.587, 0.114));
			}

			fixed4 ditherBayer2(fixed2 position, float brightness) {
				int x = fmod(position.x, 4.0);
				int y = fmod(position.y, 4.0);

				int index = x + y * 4;
				float lim = 0.0;

				if (x < 8) {
					if (index == 0) lim = 0.0625;
					if (index == 1) lim = 0.5625;
					if (index == 2) lim = 0.1875;
					if (index == 3) lim = 0.6875;
					if (index == 4) lim = 0.8125;
					if (index == 5) lim = 0.3125;
					if (index == 6) lim = 0.9375;
					if (index == 7) lim = 0.4375;
					if (index == 8) lim = 0.25;
					if (index == 9) lim = 0.75;
					if (index == 10) lim = 0.125;
					if (index == 11) lim = 0.625;
					if (index == 12) lim = 1.0;
					if (index == 13) lim = 0.5;
					if (index == 14) lim = 0.875;
					if (index == 15) lim = 0.375;
				}

				if (brightness < lim * _Strength)
					return 0.0;

				return 1.0;
			}

			fixed3 ditherPattern2(fixed2 position, float brightness) {
				int x = fmod(position.x, _Width);
				int y = fmod(position.y, _Height);

				float lim = 0.0;

				lim = tex2D(_Dither, fixed2(x, y) / _Width).r;

				if (brightness < lim * _Threshold)
					return _Strength;

				return 1.0;
			}

			fixed3 ditherBayer(fixed2 position, fixed3 col) {
				return col * ditherBayer2(position, luma(col));
			}

			fixed3 ditherPattern(fixed2 position, fixed3 col) {
				return col * ditherPattern2(position, luma(col));
			}








            sampler2D _MainTex;
			sampler2D _ScreenCopy;
			uniform int _Red;
			uniform int _Green;
			uniform int _Blue;

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed3 col = tex2D(_MainTex, i.uv).rgb;
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                //return col;
				col = fixed4(ditherPattern(i.pos.xy, col), 1.0);
				fixed4 c = fixed4(0.0, 0.0, 0.0, 1.0);
				c.r = floor(col.r * _Red) / _Red;
				c.g = floor(col.g * _Green) / _Green;
				c.b = floor(col.b * _Blue) / _Blue;
				return c;
            }
            ENDCG
        }
    }
}
