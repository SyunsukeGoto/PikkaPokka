//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   TutorialManager.cs
//!
//! @brief  チュートリアルマネージャー関連のCsファイル
//!
//! @date   2019/5/28
//!
//! @author オクムラ イヤゴ
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の使用 ==========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------
//!
//! @brief チュートリアルマネージャークラス
//!
//----------------------------------------------------------------------
public class TutorialManager : MonoBehaviour
{
    string _texName = "Texture/tutorialBoard";
    int _count = 0;
    bool tutorial = false;

    // 初期化処理
    void Start()
    {

        if (SharedData.GetStageNum() == 0)
        {
            tutorial = true;
            this.gameObject.SetActive(true);
            this.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(_texName + _count.ToString());
            Time.timeScale = 0;
        }
        else
        {
            tutorial = false;
            this.gameObject.SetActive(false);
        }
    }
    
    // 更新処理
    void Update()
    {
        if (tutorial)
        {
            UnityEngine.GameObject go = GameObject.Find("HPGreenImage");
            go.GetComponent<Image>().fillAmount = 1.0f;
            if (_count < 6)
            {
                if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown("joystick button 1")))
                {
                    _count++;
                    this.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(_texName + _count.ToString());

                }

                if (Input.GetKeyDown(KeyCode.Z) || (Input.GetKeyDown("joystick button 0")))
                {
                    if (_count > 0)
                    {
                        _count--;
                        this.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(_texName + _count.ToString());
                    }

                }
            }

            else
            {
                this.gameObject.SetActive(false);
                Time.timeScale = 1;
                tutorial = false;
            }
        }
    }

    public bool Tutorial
    {
        get { return tutorial; }
    }
}
