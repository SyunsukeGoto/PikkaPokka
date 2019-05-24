//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		PlayerMove.cs
/// 
/// <date>		2019/5/22
/// 
/// <author>	後藤　駿介
//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///-----------------------------------------------------------------------------
/// 
/// </brief> プレイヤークラス
///
///-----------------------------------------------------------------------------
namespace Goto
{
    public class PlayerMove : MonoBehaviour
    {
        private float _vec;                 // 移動量
        private Vector3 _myPos;             // 自分の座標
        [SerializeField]
        private float _maxPos;              // 右端の座標
        [SerializeField]
        private float _minPos;              // 左端の座標
        private Animator _animator;         // プレイヤーのアニメーター
        [SerializeField]
        private GameObject _starObject;
        private string _imageName;

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {
            _vec = 3;       // 移動量の設定
            _animator = GetComponent<Animator>();
            _imageName = "ChasedPlayer_1";
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            // 自分座標の更新
            _myPos = gameObject.GetComponent<RectTransform>().anchoredPosition;

            // 移動範囲外を超えたら移動量を逆にする
            if (_myPos.x >= _maxPos || _myPos.x <= _minPos)
            {
                _vec *= -1;
            }

            if(_vec < 0)
            {
                _animator.SetBool("Chased", true);
                _starObject.SetActive(false);
            }
            if (_vec > 0)
            {
                _animator.SetBool("Chased", false);
            }

            //if(gameObject.ima)
            Image img = GameObject.Find("Canvas/Player").GetComponent<Image>();

            if (img.sprite.name == _imageName)
            {
                _starObject.SetActive(true);
            }
            else
            {
                _starObject.SetActive(false);
            }

            // プレイヤーの移動等の更新の反映
            float posX = _myPos.x + _vec;
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, _myPos.y, _myPos.z);
        }
    }
}
