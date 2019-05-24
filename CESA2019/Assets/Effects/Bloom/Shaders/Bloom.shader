//
// Bloom.shader
// Actor: Tamamura Shuuki
// 

// ブルームエフェクト
Shader "Custom/Bloom"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
		_SourceTex("Source texture", 2D) = "white"{}
		_Threshold("Threshold", Float) = 0
		_Intensity("Intensity", Float) = 0
    }
    SubShader
    {
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

		sampler2D _MainTex;				// メインで使用
		sampler2D _SourceTex;			// 元画像（最後の合成用）
		float4 _MainTex_TexelSize;		// テクセルサイズ
		float _Threshold;				// ブルームに適用したい色の閾値（敷地より大きい色のみブルームの対象となる）
		float _Intensity;				// 光の強度
				
		// ぼかしに使用 ---------------------------------------------
		// メインテクスチャのRGBのみをサンプリングする
		float3 sampleMain(float2 uv)
		{
			return tex2D(_MainTex, uv).rgb;
		}

		// 対角線上の4点からサンプリングした色の平均値を返す
		float3 sampleBox(float2 uv, float delta)
		{
			float4 offset = _MainTex_TexelSize.xyxy * float2(-delta, delta).xyxy;
			float3 sum = sampleMain(uv + offset.xy) + sampleMain(uv + offset.zy) + sampleMain(uv + offset.xw) + sampleMain(uv + offset.zw);
			return sum / 4;
		}
		// ----------------------------------------------------------

		// 明度の取得
		float getBrightness(float4 color)
		{
			return max(color.r, max(color.g, color.b));
		}

		// 頂点シェーダを各パス共通で使用
        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }
		ENDCG

		Tags 
		{ 
			"RenderType"="Opaque" 
		}

		// ブルームを適用するピクセル抽出用
        Pass
        {
            CGPROGRAM
            float4 frag (v2f i) : SV_Target
            {
                float4 col = 1;
				col.rgb = sampleBox(i.uv, 1);
				float brightness = getBrightness(col);

				// 明度が閾値（_Threshold）より大きいピクセルだけをブルームの対象にする
				float contribution = max(0, brightness - _Threshold);
				contribution /= max(brightness, 0.00001);

                return col;
            }
            ENDCG
        }

		// ダウンサンプリング用
		// ダウンサンプリング時には1ピクセル分ずらした対角線上の4点からサンプリング
		Pass
		{
			Blend One One

			CGPROGRAM
			float4 frag (v2f i) : SV_Target
            {
                float4 col = 1;
				col.rgb = sampleBox(i.uv, 1.0);
                return col;
            }
			ENDCG
		}

		// アップサンプリング用
		// アップサンプリング時には0.5ピクセル分ずらした対角線上の4点からサンプリング
		Pass
		{
			Blend One One

			CGPROGRAM
			float4 frag (v2f i) : SV_Target
            {
                float4 col = 1;
				col.rgb = sampleBox(i.uv, 0.5);
                return col;
            }
			ENDCG
		}

		// 元画像との合成用
		Pass
		{
			CGPROGRAM
			float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_SourceTex, i.uv);
				col.rgb += sampleBox(i.uv, 0.5) * _Intensity;
                return col;
            }
			ENDCG
		}
    }
}
