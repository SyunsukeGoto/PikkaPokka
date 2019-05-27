//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   ShareData.cs
//!
//! @brief  データの共有関連のCsファイル
//!
//! @date   2019/5/23
//!
//! @author オクムラ イヤゴ
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

// 名前空間の使用 ==========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------
//!
//! @brief データ共有クラス
//!
//----------------------------------------------------------------------
static public class SharedData
{
    // 共有ステージ番号
    static public int _stageNum = 0;

    // 共有ステージのクリア番号
    static public int _stageMaxNum = 5;

    //  共有ステージ番号の取得
    static public int GetStageNum() { return _stageNum; }

    // 共有ステージのクリア番号の取得
    static public int GetStageMaxNum() { return _stageMaxNum; }

    //  共有ステージ番号の代入
    static public void SetStageNum(int stageNum) { _stageNum = stageNum; }

    // 共有ステージのクリア番号の代入
    static public void SetStageMaxNum(int stageMaxNum) { _stageMaxNum = stageMaxNum; }

    // 共有ステージのクリア番号の増量
    static public void AddStageMaxNum() { _stageMaxNum++; }
}