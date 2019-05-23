//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		TransitionToSelectScene.cs
/// 
/// <date>		2019/5/22
/// 
/// <author>	後藤　駿介
//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///-----------------------------------------------------------------------------
/// 
/// </brief> シーン遷移クラス
///
///-----------------------------------------------------------------------------
namespace Goto
{
    public class TransitionToSelectScene : MonoBehaviour
    {
        [SerializeField]
        private string _selectSceneName;


        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {

        }

        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                SceneManager.LoadScene(_selectSceneName);
            }
        }
    }
}