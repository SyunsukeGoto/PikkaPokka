//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		RotationFerrisWheel.cs
/// 
/// <date>		2019/5/22
/// 
/// <author>	後藤　駿介
//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-----------------------------------------------------------------------------
/// 
/// </brief> 観覧車回転クラス
///
///-----------------------------------------------------------------------------
public class RotationFerrisWheel : MonoBehaviour
{
    private float _zAngle;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        _zAngle = 1f;   
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0f, 0f, _zAngle)); 
    }
}
