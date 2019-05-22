﻿//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		PlayerMove.cs
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
/// </brief> プレイヤークラス
///
///-----------------------------------------------------------------------------
public class PlayerMove : MonoBehaviour
{
    private float _vec;
    private Vector3 _myPos;
    private float _scaleX;
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        _vec = 3;
        _scaleX = 1;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        _myPos = gameObject.GetComponent<RectTransform>().anchoredPosition;

        if (_myPos.x >= 974f || _myPos.x <= -543f)
        {
            _vec *= -1;
            _scaleX *= -1;
        }

        float posX = _myPos.x + _vec;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, _myPos.y, _myPos.z);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(_scaleX, 1f, 1f);
    }
}
