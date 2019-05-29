//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   SelectButton.cs
//!
//! @brief  セレクトボタン関連のCsファイル
//!
//! @date   2019/5/22
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
//! @brief セレクトボタンクラス
//!
//----------------------------------------------------------------------
public class SelectButton : MonoBehaviour
{
    public UnityEngine.GameObject _selectButton; // セレクトボタンのオブジェクト
    public UnityEngine.GameObject _backTitleButton; // タイトルに戻るボタン
    public UnityEngine.GameObject _backGroundImage; // 背景の画像
    public UnityEngine.GameObject _stage1Line; // ステージ1までの線
    public string _titleSceneName; // タイトルシーンの名前
    public string _playSceneName; // プレイシーンの名前
    
    
    // 初期化処理
    void Start()
    {
        //_audio = GetComponent<AudioSource>();

        if (SharedData._stageMaxNum > 1)
        {
            _stage1Line.GetComponent<Image>().color = Color.yellow;
            if (SharedData._stageMaxNum >= 2)
            {
                // 背景の読み込み
                _backGroundImage.GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("Texture/backGround" + (SharedData._stageMaxNum).ToString());
            }
        }
    }

    // ボタンを押した時の処理
    public void ButtonClick(string name)
    {
        if (SharedData._stageMaxNum >= int.Parse(name) + 1)
        {
            // intに変換する
            SharedData._stageNum = int.Parse(name) + 1;
            int num = int.Parse(name);
            int num2 = int.Parse(name) % 6;
            // 5以下だった
            if (num > 5)
            {
                // 増量
                num2++;
            }
            // ホームに戻るボタンの非表示
            _backTitleButton.SetActive(false);
            // セレクトボタンの表示
            _selectButton.SetActive(true);
            // 選択されたステージデータの番号の取得
            _selectButton.transform.GetChild(0).GetComponent<Text>().text =
                ((num / 6) + 1).ToString() + "-" + num2.ToString();
        }
    }

    // チュートリアルボタンを押した他時の処理
    public void TutorialButton(string name)
    {
        // ホームに戻るボタンの非表示
        _backTitleButton.SetActive(false);
        // セレクトボタンの表示
        _selectButton.SetActive(true);
        // 選択されたステージデータの番号の取得
        _selectButton.transform.GetChild(0).GetComponent<Text>().text = "Tutorial";
    }

    // タイトルに戻るボタンを押した時の処理 
    public void BackTitle()
    {
        // タイトルシーンに移動
        UnityEngine.SceneManagement.SceneManager.LoadScene(_titleSceneName);
    }

    // セレクトの戻るボタンを押した時の処理
    public void BackSelect()
    {
        // セレクトボタンを非表示
        _selectButton.SetActive(false);

        // ホームに戻るボタンの表示
        _backTitleButton.SetActive(true);
    }

    // スタートボタンの処理
    public void StartButton()
    {
        // プレイシーンに移動
        UnityEngine.SceneManagement.SceneManager.LoadScene(_playSceneName);
    }
}