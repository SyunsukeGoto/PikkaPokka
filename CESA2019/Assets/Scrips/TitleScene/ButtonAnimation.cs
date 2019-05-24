//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		ButtonAnimation.cs
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
/// </brief> ButtonAnimationクラス
///
///-----------------------------------------------------------------------------
namespace Goto
{
    public class ButtonAnimation : MonoBehaviour
    {
        private float _alpha;
        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {
            _alpha = 0f;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            _alpha += 0.03f;

            gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.Sin(_alpha));

        }
    }
}
