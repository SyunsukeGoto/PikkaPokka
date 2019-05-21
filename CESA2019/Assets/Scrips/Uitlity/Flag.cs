using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
/// <file>		Flag.cs
/// 
/// <date>		2019/2/26
/// 
/// <author>	後藤　駿介
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/


///-----------------------------------------------------------------------------
/// 
/// </brief> フラグクラス
///
///-----------------------------------------------------------------------------

namespace Goto
{
    public class Flag
    {
        private uint _flag;

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {
            _flag = 0;
        }

        /// <summary>
        /// フラグを立てる
        /// </summary>
        /// <param name="flag">対象とのフラグ</param>
        public void OnFlag(uint flag)
        {
            _flag |= flag;
        }

        /// <summary>
        /// フラグを伏せる
        /// </summary>
        /// <param name="flag">対象とのフラグ</param>
        public void OffFlag(uint flag)
        {
            _flag &= ~flag;
        }

        /// <summary>
        /// フラグが立っているか
        /// </summary>
        /// <param name="flag">対象とのフラグ</param>
        /// <returns>true or false</returns>
        public bool IsFlag(uint flag)
        {
            return (_flag & flag) != 0;
        }
    }
}
