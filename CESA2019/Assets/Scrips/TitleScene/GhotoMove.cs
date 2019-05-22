//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		GhotoMove.cs
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
/// </brief> おばけクラス
///
///-----------------------------------------------------------------------------
public class GhotoMove : MonoBehaviour
{
    private float _angle;
    private float _startPos;
    private float _vec;
    private Vector3 _myPos;
    private float _scaleX;
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        _angle = 0f;
        _startPos = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        _vec = 3;
        _scaleX = 1;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        _myPos = gameObject.GetComponent<RectTransform>().anchoredPosition;

        if(_myPos.x >= 614 || _myPos.x <= -903)
        {
            _vec *= -1;
            _scaleX *= -1;
        }

        float posX = _myPos.x + _vec;
        _angle += 0.05f;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, (Mathf.Sin(_angle) * 20) + _startPos, _myPos.z);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(_scaleX, 1f, 1f);
    }
}

