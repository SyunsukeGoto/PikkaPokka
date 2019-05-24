Shader "Effect/RockCrush/Rock"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Param("Param", Float)=0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100

		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
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
			float _Param;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				float2 offset = float2(0.5, 0.5);
				float d = distance(offset, i.uv);
				float r = _Param;
				if(d > r)
				{
					col.a = 0;
				}
                return col;
            }
            ENDCG
        }
    }
}
