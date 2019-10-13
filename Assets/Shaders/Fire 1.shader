Shader "Unlit/FireVariant"
{
    Properties
    {
        _Noise ("Noise", 2D) = "white" {}
		_Distortion("Distortion", 2D) = "white" {}
		_Colour1("Colour1", Color) = (0,0,0,0)
		_Colour2("Colour2", Color) = (0,0,0,0)
		_Speed("Speed", float) = 2
			_Cutoff("Cutoff", float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		Cull off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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

            sampler2D _Noise;
			sampler2D _Distortion;
			float4 _Noise_ST;
			float _Speed;
			float _Cutoff;
			float4 _Colour1;
			float4 _Colour2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Noise);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

				fixed3 distort = tex2D(_Distortion, i.uv).rgb;
				fixed2 newUV = (i.uv.x - distort.g, i.uv.y - distort.r);
				fixed4 col = tex2D(_Noise, newUV + (_Time.x * _Speed));
				float gradient = lerp(0, 1, i.uv.y);
				//gradient = pow(gradient, 2);
				col = (col + 0.5) * gradient;
				if (col.r < _Cutoff) {
					clip(-1);
				}
				col = lerp(_Colour1, _Colour2, i.uv.y);
				return col;
            }
            ENDCG
        }
    }
}
