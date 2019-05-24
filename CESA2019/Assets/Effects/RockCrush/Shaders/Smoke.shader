Shader "Effect/RockCrush/Smoke"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100

        Pass
        {
            Lighting Off
			Alphatest Greater [_Cutoff]
			SetTexture [_MainTex] { combine texture } 

			
        }
    }
}
