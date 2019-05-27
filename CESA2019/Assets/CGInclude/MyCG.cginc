//
// MyCG.cginc
// Actor: Tamamura Shuuki
//
// シェーダーに使用する汎用関数
//

// -----------------------------------------------------
// ランダムな値を返す
// -----------------------------------------------------
float rand(float2 co)
{
	return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
}

// -----------------------------------------------------
// グレースケールに変換
// -----------------------------------------------------
float toGray(float4 c)
{
	return (c.r*0.3 + c.g*0.6 + c.b*0.1);
}		

// -----------------------------------------------------
// 回転
// -----------------------------------------------------
float3 rotate(float3 p, float angle, float3 axis) 
{
	float3 a = normalize(axis);
	float s = sin(angle);
	float c = cos(angle);
	float r = 1.0 - c;
	float3x3 m = float3x3(
		a.x * a.x * r + c,
		a.y * a.x * r + a.z * s,
		a.z * a.x * r - a.y * s,
		a.x * a.y * r - a.z * s,
		a.y * a.y * r + c,
		a.z * a.y * r + a.x * s,
		a.x * a.z * r + a.y * s,
		a.y * a.z * r - a.x * s,
		a.z * a.z * r + c
		);
	return mul(p, m);
}

// -----------------------------------------------------
// 拡大・縮小
// -----------------------------------------------------
float3 scale(float3 p, float3 s)
{
	float3x3 m = float3x3(
		s.x, 0, 0,
		0, s.y, 0,
		0, 0, s.z
		);
		return mul(p, m);
}			

// -----------------------------------------------------
// 平行移動
// -----------------------------------------------------
float4 trans(float3 p, float3 t)
{
	float4x4 m = float4x4(
		1, 0, 0, t.x,
		0, 1, 0, t.y,
		0, 0, 1, t.z,
		0, 0, 0, 1
		);
		return mul(p, m);
}		