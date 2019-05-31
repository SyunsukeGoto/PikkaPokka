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

        [SerializeField]
        private Fade _fade;

        private bool _flag = false;

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {
            _fade.SetFadeIn();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            if (Time.timeScale == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1"))
                {
                    if (!_flag)
                    {
                        //SceneManager.LoadScene(_selectSceneName);
                        _fade.SetFadeOut(_selectSceneName);
                        _flag = true;
                    }
                }
            }
        }
    }
}