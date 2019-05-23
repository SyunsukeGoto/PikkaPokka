using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハンマーのコントローラー
/// </summary>
public class hammerController : MonoBehaviour
{
    // プレイヤー
    public GameObject player;
    // 現在の状態
    Momoya.PlayerController.HammerState state = Momoya.PlayerController.HammerState.NONE;
    // 当たっている状態
    bool hitState = false;
    
    /// <summary>
    /// ハンマーの状態の取得
    /// </summary>
    /// <param name="_state"></param>
    public bool GetHammerState(Momoya.PlayerController.HammerState _state){
        return (_state != Momoya.PlayerController.HammerState.NONE) ? true : false;
    }
}
