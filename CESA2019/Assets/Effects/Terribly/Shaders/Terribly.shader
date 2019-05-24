Shader "Effect/Hidden/Terribly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Threshold("Threshold", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

		CGINCLUDE
		#pragma vertex vert
        #pragma fragment frag

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

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		float _Threshold;
		float _Intensity;
		

		// メインテクスチャからサンプリングしてRGBのみ返す
        half3 sampleMain(float2 uv)
		{
            return tex2D(_MainTex, uv).rgb;
        }

        // 対角線上の4点からサンプリングした色の平均値を返す
        half3 sampleBox (float2 uv, float delta) 
		{
            float4 offset = _MainTex_TexelSize.xyxy * float2(-delta, delta).xxyy;
            half3 sum = sampleMain(uv + offset.xy) + sampleMain(uv + offset.zy) + sampleMain(uv + offset.xw) + sampleMain(uv + offset.zw);
            return sum * 0.25;
        }

		fixed2 random2(fixed2 st)
		{
			st = fixed2( dot(st,fixed2(127.1,311.7)),
							dot(st,fixed2(269.5,183.3)) );
			return -1.0 + 2.0*frac(sin(st)*43758.5453123);
		}

		float perlinNoise(fixed2 st) 
		{
			fixed2 p = floor(st);
			fixed2 f = frac(st);
			fixed2 u = f*f*(3.0-2.0*f);

			float v00 = random2(p+fixed2(0,0));
			float v10 = random2(p+fixed2(1,0));
			float v01 = random2(p+fixed2(0,1));
			float v11 = random2(p+fixed2(1,1));

			return lerp( lerp( dot( v00, f - fixed2(0,0) ), dot( v10, f - fixed2(1,0) ), u.x ),
							lerp( dot( v01, f - fixed2(0,1) ), dot( v11, f - fixed2(1,1) ), u.x ), 
							u.y)+0.5f;
		}

		v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }
		ENDCG

		// ダウンサンプリング用のパス
        Pass
        {
            CGPROGRAM
            // ダウンサンプリング時には1ピクセル分ずらした対角線上の4点からサンプリング
            fixed4 frag (v2f i) : SV_Target
            {
                half4 col = 1;
                col.rgb = sampleBox(i.uv, 1);
                return col;
            }
            ENDCG
        }

        // アップサンプリング用のパス
        Pass
        {
            CGPROGRAM
            // アップサンプリング時には0.5ピクセル分ずらした対角線上の4点からサンプリング
            fixed4 frag (v2f i) : SV_Target
            {
                half4 col = 1;
                col.rgb = sampleBox(i.uv, 0.5);
                return col;
            }
            ENDCG
        }

		// 画面を少し赤くする
		Pass
        {
            CGPROGRAM
            fixed4 frag (v2f i) : SV_Target
            {
				float c = perlinNoise(i.uv * (100 + sin(_Time)));
                fixed4 col = tex2D(_MainTex, i.uv);

				float2 offset = float2(0.5, 0.5);
				float d = distance(offset, i.uv);
				d = max(0, d - _Intensity);
				//d += 1;

				float r = 1;
				//r = (c < _Threshold)?1:0;	
				r = r * d;
				col.r += r;
				
                return col;
            }
            ENDCG
        }
    }
}
