//
// VanishGhostSmoke.shader
// Actor: Tamamura Shuuki
//

// お化けがドロンした時の煙
Shader "Effect/VanishGhost/Smoke"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

        Pass
        {
            Lighting Off
			Alphatest Greater [_Cutoff]
			SetTexture [_MainTex] { combine texture } 
        }
    }
}
