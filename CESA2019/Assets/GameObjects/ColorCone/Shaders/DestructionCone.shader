//
// ColorCone.shader
//

Shader "Custom/DestructionCone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 0, 0, 1)
		_Destruction("Destruction", Range(0.0, 1.0)) = 0.0
		_RotationFactor("Rotation factor", Float) = 90
		_DispersionFactor("Dispersion factor", Float) = 2
		_Scale("Scale", Float) = 1
    }
    SubShader
    {
		// 各パス共通で使用
		CGINCLUDE
		#pragma vertex vert
        #pragma fragment frag
		#pragma geometry geom

        #include "UnityCG.cginc"
		#include "Assets/CGInclude/MyCG.cginc"

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
			float3 lightDir : TEXCOORD1;
        };

        sampler2D _MainTex;
		float4 _Color;					// 色
		float _Destruction;				// 破壊度合
		float _RotationFactor;			// 回転具合
		float _DispersionFactor;		// 散らばり具合
		float _Scale;

        appdata vert (appdata v)
        {
            return v;
        }

		[maxvertexcount(3)]
		void geom(triangle appdata input[3], inout TriangleStream<v2f> stream)
		{
			// ポリゴンの中心を計算
			float3 center = (input[0].vertex + input[1].vertex + input[2].vertex) / 3;

			// ポリゴンの法線を計算
			float3 v0 = input[1].vertex.xyz - input[0].vertex.xyz;
			float3 v1 = input[2].vertex.xyz - input[1].vertex.xyz;
			float3 normal = normalize(cross(v0, v1));

			// 破壊パラメータをそのまま使用
			float destruction = _Destruction;

			// ポリゴン位置を乱数で生成
			float r = 2.0 * (rand(center.xy) - 0.5);
			float r3 = r.xxx;
			float3 up = float3(0, 1, 0);

			[unroll]
			for (int i = 0; i < 3; i++)
			{
				appdata v = input[i];
				v2f o;

				#if (DEBUG != true)
				// 単位を変化
				float dest = max(0, sin(_Time * 50));
				destruction = dest;
				#endif

				// 中心位置を起点にスケールを変化
				//v.vertex.xyz = (v.vertex.xyz - center) * (1.0 - destruction) + center;
				v.vertex.xyz = (v.vertex.xyz - center) * (_Scale) + center;
				//v.vertex.xyz = scale(v.vertex.xyz, float3(_Scale, _Scale, _Scale));
				// 中心位置を起点に、乱数を用いて回転を変化
				float angle = r3;
				angle = angle * 3.14/180;
				v.vertex.xyz = rotate(v.vertex.xyz - center, angle * destruction * _RotationFactor, v.normal) + center;
				// 法線方向に位置を変化
				float t = _Time;
				v.vertex.xyz += float3(destruction*r3*_DispersionFactor, 0, -destruction) * destruction;

				// 変換
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = normalize(UnityObjectToWorldNormal(v.normal));
				o.lightDir = normalize(WorldSpaceLightDir(v.vertex));
				//o.viewDir = normalize(WorldSpaceViewDir(v.vertex));

				stream.Append(o);
			}

			stream.RestartStrip();
		}
		ENDCG

        Tags 
		{ 
			"RenderType"="Opaque" 
		}

		// 頂点ライティング用のパス
        Pass
        {
			Cull Off

            CGPROGRAM
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;	
				
				float diff = saturate(dot(i.lightDir, i.normal));
				col.rgb *= diff;

                return col;
            }
            ENDCG
        }
    }
}
