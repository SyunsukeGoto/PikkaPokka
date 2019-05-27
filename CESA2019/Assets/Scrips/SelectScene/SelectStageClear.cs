﻿//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   SelectStageClear.cs
//!
//! @brief  セレクトステージクリア関連のCsファイル
//!
//! @date   2019/5/23
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
//! @brief セレクトステージクリアクラス
//!
//----------------------------------------------------------------------
public class SelectStageClear : MonoBehaviour
{
    public UnityEngine.GameObject _selectButton; // セレクトボタンのオブジェクト
    public UnityEngine.GameObject _backTitleButton; // タイトルに戻るボタン
    public UnityEngine.GameObject _backSelectButton; // セレクトに戻るボタン
    public UnityEngine.GameObject _StartButton; // スタートボタン
    public UnityEngine.GameObject[] _satgeButton; // ステージのボタン
    public UnityEngine.GameObject _board; // 看板
    public UnityEngine.GameObject _backGround; // 背景
    public UnityEngine.GameObject _hummer; // トンカチ
    Vector3 _backGroundPos; // 背景のの位置
    int _selectNum; // 選択してる番号
    float _count; // カウント
    float _backGroundMoveDirection;// 背景の進む方向
    float _backGroundRDir; // 背景の右方向
    float _backGroundLDir; // 背景の左方向
    float _stageSurfaceNum; // ステージの面番号
    public string _titleSceneName; // タイトルシーンの名前
    public string _playSceneName; // プレイシーンの名前
    int _hummerCount; // ハンマーのカウント
    public int _hummerMaxCount; // ハンマーの最大カウント
    bool _hummerFlag; // ハンマーのフラグ
    // セレクトの状態
    enum SeletState
    {
        StageSelect,
        StartSelect,
        BackSelect,
        BackTitle,
        StartScene,
        TitleScene,
    }
    SeletState _selectState; // セレクトの状態
    SeletState _selectTmp; // セレクトの位置保管

    // 初期化処理
    void Start()
    {
        // ハンマーのフラグの初期化
        _hummerFlag = false;
        // ハンマーカウントの初期化
        _hummerCount = 0;
        // セレクトの状態の初期化
        _selectState = SeletState.StageSelect;
        _selectTmp = SeletState.StageSelect;
        // ステージの面の初期化
        _stageSurfaceNum = 0;
        // 背景の方向の初期化
        _backGroundRDir = -803.0f;
        _backGroundLDir = 803.0f;
        _backGroundMoveDirection = 0.0f;
        // 選択している番号の初期化
        _selectNum = 0;
        // カウントの初期化
        _count = 1.0f;
        // 位置の初期化
        _backGroundPos = _backGround.transform.localPosition;
        // トンカチの位置の初期化
        _hummer.transform.localPosition = _satgeButton[_selectNum].transform.localPosition
            + new Vector3(861, 38, 0);
        // 釘の初期化
        NailStart();
        // ボードの初期化
        _board.transform.localPosition = new Vector3(
            _satgeButton[SharedData._stageMaxNum - 1].transform.localPosition.x,
            _satgeButton[SharedData._stageMaxNum - 1].transform.localPosition.y,
            _satgeButton[SharedData._stageMaxNum - 1].transform.localPosition.z);
    }

    // 更新処理
    void Update()
    {
        // カウントが2以下だったら
        if (_count < 2.0f)
        {
            // カウントを増量する
            _count += 0.01f;
        }
        // カウントが1以上だったら
        if (_count > 1.0f)
        {
            // フラグがfalseだったら
            if (_hummerFlag == false)
            {
                // セレクトの状態
                switch (_selectState)
                {
                    // ステージセレクト状態だったら
                    case (SeletState.StageSelect):
                        // ステージのセレクト処理
                        StageSelectMode();
                        break;
                    // バックタイトル状態だったら
                    case (SeletState.BackTitle):
                        // バックタイトル処理
                        BackTitleMode();
                        break;
                    // スタートセレクト状態だったら
                    case (SeletState.StartSelect):
                        // スタートセレクト処理
                        StartSelectMode();
                        break;
                    // セレクトの戻る状態
                    case (SeletState.BackSelect):
                        // セレクトの戻る処理
                        BackSelectMode();
                        break;
                    // スタートシーン状態だったら
                    case (SeletState.StartScene):
                        // プレイシーンに移動
                        StartButton();
                        break;
                    // タイトルシーンだったら
                    case (SeletState.TitleScene):
                        // タイトルシーンに移動
                        BackTitle();
                        break;
                }
            }
        }

        // フラグがtrueだったら
        if(_hummerFlag)
        {
            // ハンマーの角度を変える
            _hummer.transform.localRotation =
                Quaternion.Euler(_hummer.transform.localRotation.x,
                _hummer.transform.localRotation.y, 80.0f);
            // ハンマーカウントを増量
            _hummerCount++;
            // 最大カウントを超えたら
            if(_hummerCount > _hummerMaxCount)
            {
                // 状態を変える
                _selectState = _selectTmp;
                // カウントを戻す
                _hummerCount = 0;
                // フラグを変える
                _hummerFlag = false;
            }
        }
        else
        {
            // ハンマーの角度を変える
            _hummer.transform.localRotation =
                Quaternion.Euler(_hummer.transform.localRotation.x,
                _hummer.transform.localRotation.y, 20.0f);
        }

        // 背景をずらす
        _backGround.transform.localPosition = Vector3.Lerp(
            _backGroundPos, _backGroundPos + new Vector3(_backGroundMoveDirection, 0, 0), _count);
    }
    // 釘の初期化処理
    private void NailStart()
    {
        // 共有ステージのクリア番号が1以上だったら
        if (SharedData._stageMaxNum > 1)
        {
            // 釘の画像押された画像に変える
            _satgeButton[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/nail2");
            // 釘の位置をずらす
            _satgeButton[0].transform.localPosition =
                        new Vector3(_satgeButton[0].transform.localPosition.x + 21,
                        _satgeButton[0].transform.localPosition.y - 7,
                        _satgeButton[0].transform.localPosition.z);
            // 配列の数分回す
            for (int i = 1; i < _satgeButton.Length; i++)
            {
                // クリアしている釘のステージ
                if (int.Parse(_satgeButton[i].name) + 1 < SharedData._stageMaxNum)
                {
                    // 画像を変える
                    _satgeButton[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/nail2");
                    // 位置を変える
                    _satgeButton[i].transform.localPosition =
                        new Vector3(_satgeButton[i].transform.localPosition.x + 21,
                        _satgeButton[i].transform.localPosition.y - 7,
                        _satgeButton[i].transform.localPosition.z);
                }
            }
        }
    }

    // ボタンを押した時の処理
    private void ButtonClick(string name)
    {
        if (SharedData._stageMaxNum >= int.Parse(name) + 1)
        {
            // intに変換する
            SharedData._stageNum = int.Parse(name) + 1;
            // int型に変換する
            int num = int.Parse(name);
            // ホームに戻るボタンの非表示
            _backTitleButton.SetActive(false);
            // セレクトボタンの表示
            _selectButton.SetActive(true);
            // 選択されたステージデータの番号の取得
            _selectButton.transform.GetChild(0).GetComponent<Text>().text =
                _satgeButton[num].transform.GetChild(0).GetComponent<Text>().text;
        }
    }
    // タイトルに戻る処理 
    public void BackTitle()
    {
        // タイトルシーンに移動
        UnityEngine.SceneManagement.SceneManager.LoadScene(_titleSceneName);
    }

    // セレクトの戻るボタンを押した時の処理
    private void BackSelect()
    {
        // セレクトボタンを非表示
        _selectButton.SetActive(false);

        // ホームに戻るボタンの表示
        _backTitleButton.SetActive(true);
    }

    // スタートボタンの処理
    private void StartButton()
    {
        // プレイシーンに移動
        UnityEngine.SceneManagement.SceneManager.LoadScene(_playSceneName);
    }

    // ステージのセレクト処理
    private void StageSelectMode()
    {
        // ステージセレクトの戻る時の処理
        BackSelect();
        // 右キーを押したら
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_selectNum == 5 || _selectNum == 10)
            {
                _count = 0;
                _backGroundPos = _backGround.transform.localPosition;
                _backGroundMoveDirection = _backGroundRDir;
                _stageSurfaceNum++;
            }
            // 選択している番号が最大以下だったら
            if (_selectNum < _satgeButton.Length - 1)
            {
                // 増量する
                _selectNum++;
            }
        }
        // 左キーを押したら
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_selectNum == 6 || _selectNum == 11)
            {
                _count = 0;
                _backGroundPos = _backGround.transform.localPosition;
                _backGroundMoveDirection = _backGroundLDir;
                _stageSurfaceNum--;
            }
            // 選択してる番号は0以上だったら
            if (_selectNum > 0)
            {
                // 減量する
                _selectNum--;
            }
        }
        // スペースキーを押したら
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //  セレクトの状態を変える
            _selectTmp = SeletState.StartSelect;
            // フラグを変える
            _hummerFlag = true;
        }
        // 上キーを押したら
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //  セレクトの状態を変える
            _selectState = SeletState.BackTitle;
        }

        // ハンマーの位置を更新する
        _hummer.transform.localPosition = _satgeButton[_selectNum].transform.localPosition
        + new Vector3(861 + (_stageSurfaceNum * _backGroundRDir), 38, 0);
    }

    // スタートセレクト状態の処理
    private void StartSelectMode()
    {
        // ボタンを押した時の処理
        ButtonClick(_selectNum.ToString());
        // ハンマーの位置を更新する
        _hummer.transform.localPosition = _StartButton.transform.localPosition
        + new Vector3(100, 40, 0);
        // スペースキーを押したら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 状態を変える
            _selectTmp = SeletState.StartScene;
            // フラグを変える
            _hummerFlag = true;
        }
        // 上キーもしくは左を押したら
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 状態を変える
            _selectState = SeletState.BackSelect;
        }
    }

    // バックタイトル状態の処理
    private void BackTitleMode()
    {
        // ハンマーの位置を更新する
        _hummer.transform.localPosition = _backTitleButton.transform.localPosition
        + new Vector3(60, 40, 0);
        // スペースキーを押したら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 状態を変える
            _selectTmp = SeletState.TitleScene;
            // フラグを変える
            _hummerFlag = true;
        }
        // 下キーを押した
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // セレクトの状態を変える
            _selectState = SeletState.StageSelect;
        }

    }

    // バックセレクト状態の処理
    private void BackSelectMode()
    {
        // ハンマーの位置を更新する
        _hummer.transform.localPosition = _backSelectButton.transform.localPosition
        + new Vector3(60, 40, 0);
        // スペースキーを押したら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 状態を変える
            _selectTmp = SeletState.StageSelect;
            // フラグを変える
            _hummerFlag = true;
        }
        // 下キーもしくは右を押したら
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 状態を変える
            _selectState = SeletState.StartSelect;
        }
    }
}