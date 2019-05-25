//
// Outline.shader
// Actor: Tamamura Shuuki
//

// 輪郭を表示する
Shader "Effect/Custom/Outline"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0, 0, 0, 1)
		_OutlineSize("Outline size", Float) = 0.1
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
			float3 normal : NORMAL;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
			float3 normal : NORMAL;
        };

		sampler2D _MainTex;
		float4 _Color;
		float _OutlineSize;

		ENDCG

		Tags 
		{ 
			"RenderType"="Transparent" 
			"IgnoreProjector"="True"
			"Queue"="Transparent"
		}

		// アウトラインを適用するパス
        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Front
			//Cull Off

            CGPROGRAM
            v2f vert (appdata v)
            {
                v2f o;
				float3 vertex = v.vertex.xyz + (v.normal * max(0, _OutlineSize));
                o.vertex = UnityObjectToClipPos(vertex);
                o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }

		// 通常のレンダリングパス
		Pass
        {
			Cull Back

            CGPROGRAM
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
