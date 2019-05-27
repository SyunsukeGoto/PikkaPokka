//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   SelectSceneGhostMove.cs
//!
//! @brief  セレクトシーンのお化けの動き関連のCsファイル
//!
//! @date   2019/5/26
//!
//! @author オクムラ イヤゴ
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の使用 ==========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------
//!
//! @brief セレクトシーンのお化けの動きクラス
//!
//----------------------------------------------------------------------
public class SelectSceneGhostMove : MonoBehaviour
{
    Vector3 _pos; // 位置情報
    float _count; // カウント
    float _velY; // Y速度
    float _velX; // X速度
    float _withMaxR; // 右の最大
    float _withMaxL; // 左の最大
    float _sizeX; // 横のサイズ

    // 初期化処理
    void Start()
    {
        // X速度
        _velX = 1.0f;
        // 速度の初期化
        _velY = 0.1f;
        // カウントの初期化
        _count = 0;
        // 位置の初期化
        _pos = this.transform.localPosition;
        // 右の最大の初期化
        _withMaxR = this.transform.localPosition.x + 100.0f;
        // 左の最大の初期化
        _withMaxL = this.transform.localPosition.x - 100.0f;
        // サイズの初期化
        _sizeX = this.transform.localScale.x;
    }

    // 更新処理
    void Update()
    {
        if(_pos.x > _withMaxR || _pos.x < _withMaxL)
        {
            _velX = -_velX;
            _sizeX = -_sizeX;
        }
        // 速度を足す
        _pos.x += _velX;
        // カウントの増量
        _count += _velY;
        // 位置の更新
        this.transform.localPosition = new Vector3(_pos.x, _pos.y + (Mathf.Cos(_count)* 10), _pos.z);
        // サイズの更新
        this.transform.localScale = new Vector3(_sizeX,
            this.transform.localScale.y, this.transform.localScale.z);
    }
}
