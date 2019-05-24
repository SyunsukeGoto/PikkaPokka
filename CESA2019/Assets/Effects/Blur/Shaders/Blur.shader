//
// Blur.Shader
// Actor: Tamamura Shuuki
//

// ブラーシェーダー
Shader "Custom/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Param("Param", Float) = 0
    }
    SubShader
    {
        Tags 
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
		}

		// 共通で使用
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
		float _Param;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }
		ENDCG

		// ぼかし
        Pass
        {
            CGPROGRAM
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
				col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(_Param, 0));
				col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-_Param, 0));
				col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(0, _Param));
				col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(0, -_Param));
				col /= 4;
                return col;
            }
            ENDCG
        }
    }
}
